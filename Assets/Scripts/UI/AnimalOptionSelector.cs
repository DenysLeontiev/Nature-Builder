using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimalOptionSelector : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private AnimalBase animalToSpawn;

	[Header("UI Elements")]
	[SerializeField] private Image indicatorImage;
	[SerializeField] private TextMeshProUGUI moneyAmountToSpawnText;
	[SerializeField] private GameObject tooltipPanel;

	private void Start()
	{
		SetMoneyAmountToSpawnText(animalToSpawn.GetAnimalSO().MoneyToSpawn);
	}

	private void Update()
	{
		(IPlaceable animal, float plantCurrentTimeBetweenSpawn) plantCurrentTimeData = AnimalSpawner.Instance.GetCurrentObjectAndTimeBetweenSpawn();

		if ((object)plantCurrentTimeData.animal == animalToSpawn)
		{
			indicatorImage.fillAmount = plantCurrentTimeData.plantCurrentTimeBetweenSpawn / animalToSpawn.GetAnimalSO().DelayBetweenSpawnTime;
		}
		else
		{
			indicatorImage.fillAmount = 1f;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (MoneySystem.Instance.GetMoneyAmount() - animalToSpawn.GetAnimalSO().MoneyToSpawn >= 0)
		{
			AnimalSpawner.Instance.SetObjectToSpawn(animalToSpawn);
			MoneySystem.Instance.RemoveMoney(animalToSpawn.GetAnimalSO().MoneyToSpawn);
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

	public void OnPointerEnter(PointerEventData eventData)
	{
		AnimalSO animalSO = animalToSpawn.GetAnimalSO();

		tooltipPanel.GetComponent<Tooltip>().SetTooltipInfo(animalSO.Name, animalSO.Description, animalSO.Sprite);

		//Tooltip.Instance.SetTooltipInfo(plantSO.PlantName, plantSO.PlantDescription, plantSO.PlantSprite);
		tooltipPanel.SetActive(true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		tooltipPanel.SetActive(false);
	}
}
