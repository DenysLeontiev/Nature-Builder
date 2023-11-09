using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlantBase : MonoBehaviour
{
    [SerializeField] protected PlantSO plantSO;
    [SerializeField] protected GameObject moneyPrefab;

    protected PlantState currentState = PlantState.Planted;
    protected Dictionary<PlantState, Action> stateActions = new Dictionary<PlantState, Action>();

    protected int currentActiveObjectIndex;

    protected float currentTimeBetweenStates;
    protected float currentTimeBetweenSpawn;

    protected void Start()
    {
        currentTimeBetweenStates = plantSO.PlantedTime;

        AddBehaviour();
        SetPlantState(PlantState.Planted);
    }

    protected void Update()
    {
        HandleStates();
    }
    public PlantSO GetPlantSO()
    {
        return plantSO;
    }

    protected void SpawnMoney()
    {
        for (int i = 0; i < plantSO.MoneyReward; i++)
        {
            Vector3 currentPos = transform.position;
            float radiusOffset = 0.5f;

            // Calculate a random point in the upper hemisphere
            Vector3 spawnPos = currentPos + UnityEngine.Random.onUnitSphere * radiusOffset;
            float yOffset = 2f;
            spawnPos.y = Mathf.Abs(spawnPos.y) + yOffset; // Ensure the prefab is spawned in the top part of the sphere

            var instantiatedMoney = Instantiate(moneyPrefab, spawnPos, Quaternion.identity);
        }
    }


    protected virtual void AddBehaviour()
    {
        // default init stuff.
    }

    protected virtual void HandleStates()
    {
        if (stateActions.ContainsKey(currentState))
        {
            stateActions[currentState].Invoke();
        }
    }

    protected virtual void SetPlantState(PlantState state)
    {
        currentState = state;

        switch (state)
        {
            case PlantState.Planted:
                HandleObjectStateVisual();
                break;
            case PlantState.Grown:
                HandleObjectStateVisual();
                break;
            case PlantState.Old:
                HandleObjectStateVisual();
                break;
            case PlantState.Dead:
                HandleObjectStateVisual();
                SpawnMoney();
                break;
        }
    }

    protected abstract void PlantedState();

    protected abstract void GrownState();

    protected abstract void OldState();
    protected abstract void DeadState();


    private void HandleObjectStateVisual()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (currentActiveObjectIndex == i)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        currentActiveObjectIndex++;
    }
}
