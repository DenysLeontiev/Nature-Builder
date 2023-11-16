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


    public void ResetTimeBetweenSpawn()
    {
        currentTimeBetweenSpawn = plantSO.DelayBetweenSpawnTime;
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

            var instantiatedMoney = Instantiate(moneyPrefab);

            StartCoroutine(MoveMoneyToPosition(instantiatedMoney, transform.position + Vector3.up * 2f, 0.5f));
        }
    }

    private IEnumerator MoveMoneyToPosition(GameObject moneyObject, Vector3 targetPosition, float duration)
    {
        float elapsed = 0f;
        Vector3 startingPosition = moneyObject.transform.position;

        while (elapsed < duration)
        {
            moneyObject.transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the moneyObject is exactly at the target position
        moneyObject.transform.position = targetPosition;
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
