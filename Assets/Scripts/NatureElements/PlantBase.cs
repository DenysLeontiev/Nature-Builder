using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlantBase : MonoBehaviour
{
    [SerializeField] protected PlantSO plantSO;
    protected PlantState currentState = PlantState.Planted;
    protected Dictionary<PlantState, Action> stateActions = new Dictionary<PlantState, Action>();

    protected int currentActiveObjectIndex;

    protected float currentTime;

    protected void Start()
    {
        currentTime = plantSO.PlantedTime;

        AddBehaviour();
        SetPlantState(PlantState.Planted);
    }

    protected void Update()
    {
        HandleStates();
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
                break;
        }
    }

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

    protected abstract void PlantedState();

    protected abstract void GrownState();

    protected abstract void OldState();
    protected abstract void DeadState();

}
