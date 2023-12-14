using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class AnimalBase : MonoBehaviour, IPlaceable
{
	[SerializeField] protected float moveSpeed = 1f;
	[SerializeField] protected AnimalSO animalSO;
	[SerializeField] protected float stopDistance = 0.4f;

	[SerializeField] private AnimalBase animalToBear;

	protected AnimalState currentState;
	protected Animator animator;

	protected Gender gender;

	protected float currentTimeBetweenStates;
	protected Vector3 targetPosition;

	protected float timeSinceLastEaten;

	protected float radius = 10f;
	private float growTime;
	private bool isGrown;

	private List<AnimalBase> possibleAnimalsToMultiplyWith = new List<AnimalBase>();
	private AnimalBase animalToMultiplyWith;

	private bool isAnimalTakenForMultiplying = false;

	private void FindPossibleAnimalsToMultiplyWith()
	{
		if(animalToMultiplyWith == null)
		{
			possibleAnimalsToMultiplyWith = FindObjectsOfType<AnimalBase>().Where(a => a.gender != gender &&
																				  a.isAnimalTakenForMultiplying == false &&
																				  a.isGrown == true).ToList();

			if(possibleAnimalsToMultiplyWith.Count > 0)
			{
				var randomAnimal = possibleAnimalsToMultiplyWith[UnityEngine.Random.Range(0, possibleAnimalsToMultiplyWith.Count - 1)];
				SetAnimalToMultiplyWith(randomAnimal);
			}
		}
	}

	private void SetAnimalToMultiplyWith(AnimalBase animal)
	{
		animalToMultiplyWith = animal;

		animal.isAnimalTakenForMultiplying = true;
		isAnimalTakenForMultiplying = true;
	}

	protected void Start()
	{
		animator = GetComponent<Animator>();
		SetCurrentState(AnimalState.Idle);
		currentTimeBetweenStates = UnityEngine.Random.Range(animalSO.IdleMinTime, animalSO.IdleMaxTime);
		targetPosition = RandomNavmeshLocation(radius);

		growTime = animalSO.GrowTime;

		SetRandomGender();
	}


	protected void Update()
	{
		FindPossibleAnimalsToMultiplyWith();

		HandleTimeSinceLastEaten();

		HandleGrowth();

		switch (currentState)
		{
			case AnimalState.Idle:
				IdleState();
				break;
			case AnimalState.Walk:
				WalkState();
				break;
			case AnimalState.Eat:
				EatState();
				break;
			case AnimalState.Multiply:
				MultiplyState();
				break;
			case AnimalState.RunAway:
				RunAwayState();
				break;
		}
	}

	private void HandleTimeSinceLastEaten()
	{
		timeSinceLastEaten += Time.deltaTime;
	}

	private void SetRandomGender()
	{
		int randomNumber = UnityEngine.Random.Range(0,3);
		gender = (Gender)randomNumber;
		Debug.Log(gender);
	}

	public AnimalSO GetAnimalSO()
	{
		return animalSO;
	}

	private void HandleGrowth()
	{
		if (isGrown)
		{
			return;
		}

		growTime -= Time.deltaTime;
		if (growTime <= 0)
		{
			transform.localScale = transform.localScale * animalSO.GrowthRatio;
			isGrown = true;
		}
	}

	protected Vector3 RandomNavmeshLocation(float radius)
	{
		Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
		randomDirection += transform.position;
		NavMeshHit hit;
		Vector3 finalPosition = Vector3.zero;
		if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
		{
			finalPosition = hit.position;
		}
		return finalPosition;
	}

	protected void SetCurrentState(AnimalState state)
	{
		currentState = state;

		animator.SetBool("RunAway", state == AnimalState.RunAway);
		animator.SetBool("Walk", state == AnimalState.Walk);
		animator.SetBool("Eat", state == AnimalState.Eat);
	}

	protected void MoveTo(Vector3 targetPos)
	{
		transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
		transform.LookAt(targetPos);
	}

	protected abstract void IdleState();
	protected abstract void WalkState();
	protected abstract void EatState();
	protected abstract void RunAwayState();
	protected abstract void MultiplyState();


	protected void StartMultiplying()
	{
		if(animalToMultiplyWith != null)
		{
			float distanceBetween = Vector3.Distance(transform.position, animalToMultiplyWith.transform.position);

			float stopDistance = 0.5f;
			if (distanceBetween > stopDistance)
			{
				MoveTo(animalToMultiplyWith.transform.position);
			}
			else
			{
				float probabilityToMultiply = 6;
				if(UnityEngine.Random.Range(0, 10) > probabilityToMultiply)
				{
					//Get middle point between 2 vectors
					Vector3 spawnPos = transform.position + (animalToMultiplyWith.transform.position - transform.position) / 2;
					var bornAnimal = Instantiate(animalToMultiplyWith, spawnPos, Quaternion.identity);
					SetCurrentState(AnimalState.Idle);

					StopMultiplying();
				}
			}
		}
	}

	private void StopMultiplying()
	{
		animalToMultiplyWith.isAnimalTakenForMultiplying = false;
		isAnimalTakenForMultiplying = false;
		animalToMultiplyWith = null;
	}

	public PlaceableSO GetPlaceableSO()
	{
		return animalSO;
	}
}
