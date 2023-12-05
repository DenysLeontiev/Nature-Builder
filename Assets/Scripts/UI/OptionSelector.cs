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
    [SerializeField] private Image indicatorImage;
    [SerializeField] private TextMeshProUGUI moneyAmountToSpawnText;
    [SerializeField] private GameObject tooltipPanel;

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

    private void SetMoneyAmountToSpawnText(float amountToSpawn)
    {
        moneyAmountToSpawnText.text = amountToSpawn.ToString();
    }

    private void Instance_OnPlantChanged(object sender, NatureSpawner.OnPlantChangedEventArgs e)
    {

    }

	public void OnPointerEnter(PointerEventData eventData)
	{
        PlantSO plantSO = plantToSpawn.GetPlantSO();

		tooltipPanel.GetComponent<Tooltip>().SetTooltipInfo(plantSO.Name, plantSO.Description, plantSO.Sprite);

		//Tooltip.Instance.SetTooltipInfo(plantSO.PlantName, plantSO.PlantDescription, plantSO.PlantSprite);
        tooltipPanel.SetActive(true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		tooltipPanel.SetActive(false);
	}
}
