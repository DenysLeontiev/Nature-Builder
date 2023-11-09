using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChristmasTreeFirst : PlantBase
{
    protected override void AddBehaviour()
    {
        stateActions[PlantState.Planted] = PlantedState;
        stateActions[PlantState.Grown] = GrownState;
        stateActions[PlantState.Old] = OldState;
        stateActions[PlantState.Dead] = DeadState;
    }

    protected override void PlantedState()
    {
        currentTimeBetweenStates -= Time.deltaTime;
        if (currentTimeBetweenStates <= 0)
        {
            currentTimeBetweenStates = plantSO.GrownTime;
            SetPlantState(PlantState.Grown);
        }
    }

    protected override void GrownState()
    {
        currentTimeBetweenStates -= Time.deltaTime;
        if (currentTimeBetweenStates <= 0)
        {
            currentTimeBetweenStates = plantSO.OldTime;
            SetPlantState(PlantState.Old);
        }
    }

    protected override void OldState()
    {
        currentTimeBetweenStates -= Time.deltaTime;
        if (currentTimeBetweenStates <= 0)
        {
            currentTimeBetweenStates = plantSO.DeadTime;
            SetPlantState(PlantState.Dead);
        }
    }

    protected override void DeadState()
    {
        currentTimeBetweenStates -= Time.deltaTime;

        if (currentTimeBetweenStates <= 0)
        {
            Destroy(gameObject);
        }
    }
}
