using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class YellowChicken : AnimalBase
{
	private Grass[] thingsToEat = new Grass[0];
	private Grass currentGrassToEat;

	private new void Start()
	{
		base.Start();
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
				currentGrassToEat = thingsToEat[Random.Range(0, thingsToEat.Length - 1)];
				targetPosition = currentGrassToEat.transform.position;
			}

			if (Vector3.Distance(transform.position, currentGrassToEat.transform.position) > stopDistance)
			{
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

			SetCurrentState(AnimalState.Multiply);
			hasGivenBirth = false;
		}
	}

	protected override void RunAwayState()
	{
		throw new System.NotImplementedException();
	}

	protected override void MultiplyState()
	{
		StartMultiplying();
	}
}
