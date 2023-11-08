using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionSelector : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private PlantBase plantToSpawn;
    [SerializeField] private Image indicatorImage;

    private void Start()
    {
        NatureSpawner.Instance.OnPlantChanged += Instance_OnPlantChanged;
    }

    private void Update()
    {
        (PlantBase plant, float plantCurrentTimeBetweenSpawn) plantCurrentTimeData = NatureSpawner.Instance.GetCurrentObjectAndTimeBetweenSpawn();
        if(plantCurrentTimeData.plant == plantToSpawn)
        {
            indicatorImage.fillAmount = plantCurrentTimeData.plantCurrentTimeBetweenSpawn / plantToSpawn.GetPlantSO().delayBetweenSpawnTime;
        }
        else
        {
            indicatorImage.fillAmount = 1f;
        }
    }

    private void Instance_OnPlantChanged(object sender, NatureSpawner.OnPlantChangedEventArgs e)
    {
        if(e.plantBase == plantToSpawn)
        {
            Debug.Log("He he, it is " + plantToSpawn);
            Debug.Log("currentTimeBetweenSpawn => " + e.currentTimeBetweenSpawn);
            Debug.Log("timeBetweenSpawnMax => " + e.timeBetweenSpawnMax);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        NatureSpawner.Instance.SetObjectToSpawn(plantToSpawn);
    }
}
