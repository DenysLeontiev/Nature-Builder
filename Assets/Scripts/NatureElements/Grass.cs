using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : PlantBase
{
    private new void Start()
    {
        base.Start();
    }

    private new void Update()
    {
        base.Update();
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
            currentTime = plantSO.GrownTime;
            SetPlantState(PlantState.Grown);
        }
    }

    protected override void GrownState()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            currentTime = plantSO.OldTime;
            SetPlantState(PlantState.Old);
        }
    }

    protected override void OldState()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            currentTime = plantSO.DeadTime;
            SetPlantState(PlantState.Dead);
        }
    }

    protected override void DeadState()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
