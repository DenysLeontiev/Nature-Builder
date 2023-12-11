using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NatureSpawner : BaseSpawner
{
    public static NatureSpawner Instance { get; private set; }

    private PlantBase currentPlantToSpawn;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(currentPlantToSpawn == null)
        {
            return;
        }

        base.SpawnObject(currentPlantToSpawn);
        base.ShowCurrentObjectVisual(currentPlantToSpawn);
        base.HandlePrefabIndicatorVisuals();
    }

	public override (IPlaceable objBase, float plantCurrentTimeBetweenSpawn) GetCurrentObjectAndTimeBetweenSpawn()
	{
		return (currentPlantToSpawn, currentTimeBetweenSpawn);
	}

	public void SetObjectToSpawn(PlantBase plant)
    {
        if(currentPlantToSpawn == plant)
        {
            return;
        }

        currentPlantToSpawn = plant;

        currentTimeBetweenSpawn = plant.GetPlantSO().DelayBetweenSpawnTime;
        timeBetweenSpawnMax = plant.GetPlantSO().DelayBetweenSpawnTime;

        isGameObjectIndicatorInstantiated = false;

        if(currentGameObjectIndicator != null)
            Destroy(currentGameObjectIndicator.gameObject);
    }

	protected override void ResetCurrentObjectToSpawn()
	{
		currentPlantToSpawn = null;
		currentGameObjectIndicator.SetActive(false);
	}
}
