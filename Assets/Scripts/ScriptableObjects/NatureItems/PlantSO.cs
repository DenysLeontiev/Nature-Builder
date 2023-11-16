using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantSO", menuName = "Nature/Plant")]
public class PlantSO : ScriptableObject
{
    public string PlantName;
    [TextArea(2, 4)]
    public string PlantDescription;

    public float DelayBetweenSpawnTime;
    public int MoneyToSpawn;
    public int MoneyReward;

    public GameObject TransparentObjectIndicator;

    public GameObject PlantedPrefab;
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
