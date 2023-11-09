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

        if (plantCurrentTimeData.plant == null)
            return;

        if(plantCurrentTimeData.plant == plantToSpawn)
        {
            indicatorImage.fillAmount = plantCurrentTimeData.plantCurrentTimeBetweenSpawn / plantToSpawn.GetPlantSO().DelayBetweenSpawnTime;
        }
        else
        {
            indicatorImage.fillAmount = 1f;
        }
    }

    private void Instance_OnPlantChanged(object sender, NatureSpawner.OnPlantChangedEventArgs e)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        NatureSpawner.Instance.SetObjectToSpawn(plantToSpawn);
    }
}
