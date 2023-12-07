using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AnimalBase : MonoBehaviour, IPlaceable
{
	[SerializeField] protected float moveSpeed = 1f;
	[SerializeField] protected AnimalSO animalSO;
	[SerializeField] protected float stopDistance = 0.4f;

	protected AnimalState currentState;
	protected Animator animator;

	protected float currentTimeBetweenStates;
	protected Vector3 targetPosition;

	protected float timeSinceLastEaten;

	protected float radius = 10f;
	private float growTime;
	private bool isGrown;

	protected void Start()
	{
		animator = GetComponent<Animator>();
		SetCurrentState(AnimalState.Idle);
		currentTimeBetweenStates = Random.Range(animalSO.IdleMinTime, animalSO.IdleMaxTime);
		targetPosition = RandomNavmeshLocation(radius);

		growTime = animalSO.GrowTime;
	}


	protected void Update()
	{
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
		Vector3 randomDirection = Random.insideUnitSphere * radius;
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

	protected abstract void IdleState();
	protected abstract void WalkState();
	protected abstract void EatState();
	protected abstract void RunAwayState();
	protected abstract void MultiplyState();

	public PlaceableSO GetPlaceableSO()
	{
		return animalSO;
	}
}
