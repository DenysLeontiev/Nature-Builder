using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlantBase : MonoBehaviour
{
    protected PlantState currentState = PlantState.Planted;
    protected Dictionary<PlantState, Action> stateActions = new Dictionary<PlantState, Action>();

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
    }

    protected abstract void PlantedState();

    protected abstract void GrownState();

    protected abstract void OldState();
    protected abstract void DeadState();

}
