using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantSO", menuName = "Nature/Plant")]
public class PlantSO : PlaceableSO
{
    public float PlantedTime;

    public GameObject GrownPrefab;
    public float GrownTime;

    public GameObject OldPrefab;
    public float OldTime;

    public GameObject DeadPrefab;
    public float DeadTime;

    public float GetTotalLifeTime()
    {
        return PlantedTime + GrownTime + OldTime +DeadTime;
    }

}
