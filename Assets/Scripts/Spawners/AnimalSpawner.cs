using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NatureSpawner;

public class AnimalSpawner : BaseSpawner
{
	public static AnimalSpawner Instance { get; private set; }

	private AnimalBase currentAnimalToSpawn;

	private void Awake()
	{
		Instance = this;
	}

	private void Update()
	{
		if (currentAnimalToSpawn == null)
		{
			return;
		}

		base.SpawnObject(currentAnimalToSpawn);
		base.ShowCurrentObjectVisual(currentAnimalToSpawn);
		base.HandlePrefabIndicatorVisuals();
	}

	public override (IPlaceable plantBase, float plantCurrentTimeBetweenSpawn) GetCurrentObjectAndTimeBetweenSpawn()
	{
		return (currentAnimalToSpawn, currentTimeBetweenSpawn);
	}

	public void SetObjectToSpawn(AnimalBase animal)
	{
		if (currentAnimalToSpawn == animal)
		{
			return;
		}

		currentAnimalToSpawn = animal;

		currentTimeBetweenSpawn = animal.GetAnimalSO().DelayBetweenSpawnTime;
		timeBetweenSpawnMax = animal.GetAnimalSO().DelayBetweenSpawnTime;

	}

	protected override void ResetCurrentObjectToSpawn()
	{
		currentAnimalToSpawn = null;
		currentGameObjectIndicator.SetActive(false);
	}
}
