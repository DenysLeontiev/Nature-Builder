using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : PlantBase
{
    private float maxTime = 5f;
    private float currentTime;

    private void Start()
    {
        AddBehaviour();
        SetPlantState(PlantState.Planted);
    }

    private void Update()
    {
        HandleStates();
    }

    protected override void AddBehaviour()
    {
        stateActions[PlantState.Planted] = PlantedState;
        stateActions[PlantState.Grown] = GrownState;
        stateActions[PlantState.Old] = OldState;
        stateActions[PlantState.Dead] = DeadState;
    }

    protected override void PlantedState()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            currentTime = maxTime;
            SetPlantState(PlantState.Grown);
            Debug.Log("I am Planted");
        }
    }

    protected override void GrownState()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            currentTime = maxTime;
            SetPlantState(PlantState.Old);
            Debug.Log("I am Grown");
        }
    }

    protected override void OldState()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            currentTime = maxTime;
            SetPlantState(PlantState.Dead);
            Debug.Log("I am Old");
        }
    }

    protected override void DeadState()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            currentTime = maxTime;
            Debug.Log("I am Dead");
            Destroy(gameObject);
        }
    }
}
