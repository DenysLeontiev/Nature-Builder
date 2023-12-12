using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class YellowChicken : AnimalBase
{
	[SerializeField] private float rigidbodyEnambleTime = 0.5f;

	private Grass[] thingsToEat = new Grass[0];
	private Grass currentGrassToEat;

	private new void Start()
	{
		StartCoroutine(EnableRigidbody());

		base.Start();
	}

	private IEnumerator EnableRigidbody()
	{
		yield return new WaitForSeconds(rigidbodyEnambleTime);
		gameObject.AddComponent<Rigidbody>();
	}
	private void RoamAround()
	{
		if(thingsToEat.Length <= 0)
		{
			if(Vector3.Distance(transform.position, targetPosition) > stopDistance)
			{
				MoveTo(targetPosition);
			}
			else
			{
				SetCurrentState(AnimalState.Idle);
				currentTimeBetweenStates = Random.Range(animalSO.IdleMinTime, animalSO.IdleMaxTime);
				targetPosition = RandomNavmeshLocation(radius);
			}
		}
	}

	private void MoveTo(Vector3 targetPos)
	{
		transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
		transform.LookAt(targetPos);
	}

	protected override void IdleState()
	{
		currentTimeBetweenStates -= Time.deltaTime;
		if (currentTimeBetweenStates < 0)
		{
			SetCurrentState(AnimalState.Walk);
		}
	}

	protected override void WalkState()
	{
		thingsToEat = FindObjectsOfType<Grass>();

		if (thingsToEat.Length > 0)
		{
			if(currentGrassToEat == null)
			{
				targetPosition = thingsToEat[Random.Range(0, thingsToEat.Length - 1)].transform.position;
				currentGrassToEat = thingsToEat[Random.Range(0, thingsToEat.Length - 1)];
			}

			if (Vector3.Distance(transform.position, currentGrassToEat.transform.position) > stopDistance)
			{
				Debug.Log("Approaching");
				MoveTo(currentGrassToEat.transform.position);
			}
			else
			{
				currentGrassToEat = null;
				SetCurrentState(AnimalState.Eat);
				currentTimeBetweenStates = Random.Range(animalSO.EatMinTime, animalSO.EatMaxTime);
			}
		}
		else
		{
			RoamAround();
		}
	}

	protected override void EatState()
	{
		currentTimeBetweenStates -= Time.deltaTime;
		if (currentTimeBetweenStates < 0)
		{
			timeSinceLastEaten = 0;
			SetCurrentState(AnimalState.Walk);
		}
	}

	protected override void RunAwayState()
	{
		throw new System.NotImplementedException();
	}

	protected override void MultiplyState()
	{
		throw new System.NotImplementedException();
	}
}
