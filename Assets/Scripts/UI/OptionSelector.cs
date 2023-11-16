using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionSelector : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private PlantBase plantToSpawn;

    [Header("UI Elements")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Image indicatorImage;
    [SerializeField] private TextMeshProUGUI moneyAmountToSpawnText;

    private void Start()
    {
        NatureSpawner.Instance.OnPlantChanged += Instance_OnPlantChanged;
        SetMoneyAmountToSpawnText(plantToSpawn.GetPlantSO().MoneyToSpawn);
    }

    private void Update()
    {
        (PlantBase plant, float plantCurrentTimeBetweenSpawn) plantCurrentTimeData = NatureSpawner.Instance.GetCurrentObjectAndTimeBetweenSpawn();

        if(plantCurrentTimeData.plant == plantToSpawn)
        {
            indicatorImage.fillAmount = plantCurrentTimeData.plantCurrentTimeBetweenSpawn / plantToSpawn.GetPlantSO().DelayBetweenSpawnTime;
        }
        else
        {
            indicatorImage.fillAmount = 1f;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(MoneySystem.Instance.GetMoneyAmount() - plantToSpawn.GetPlantSO().MoneyToSpawn >= 0)
        {
            NatureSpawner.Instance.SetObjectToSpawn(plantToSpawn);
            MoneySystem.Instance.RemoveMoney(plantToSpawn.GetPlantSO().MoneyToSpawn);
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowInfoPanel();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideInfoPanel();
    }

    private void SetMoneyAmountToSpawnText(float amountToSpawn)
    {
        moneyAmountToSpawnText.text = amountToSpawn.ToString();
    }

    private void ShowInfoPanel()
    {
        infoPanel.SetActive(true);
        DisplayCurrentPlantDetails();
    }

    private void HideInfoPanel()
    {
        infoPanel.SetActive(false);
    }

    private void DisplayCurrentPlantDetails()
    {
        PlantSO currentPlantSO = plantToSpawn.GetPlantSO();

        TextMeshProUGUI plantName = infoPanel.transform.Find("PlantName").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI plantReward = infoPanel.transform.Find("PlantReward").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI plantSpawnRate = infoPanel.transform.Find("PlantSpawnRate").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI plantGrowTime = infoPanel.transform.Find("PlantGrowTime").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI plantDescription = infoPanel.transform.Find("DescriptionPanel").Find("DescriptionText").GetComponent<TextMeshProUGUI>();

        plantName.text = $"Name: {currentPlantSO.PlantName}";
        plantReward.text = $"Reward: {currentPlantSO.MoneyReward}";
        plantSpawnRate.text = $"SpawnRate: {currentPlantSO.DelayBetweenSpawnTime} sec";
        plantGrowTime.text = $"LifeTime: {currentPlantSO.GetTotalLifeTime()} sec";
        plantDescription.text = currentPlantSO.PlantDescription;
    }

    private void Instance_OnPlantChanged(object sender, NatureSpawner.OnPlantChangedEventArgs e)
    {

    }
}
