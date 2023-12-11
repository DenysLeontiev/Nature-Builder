using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsScroller : MonoBehaviour
{
	[SerializeField] private GameObject[] sliderComponents;
	[SerializeField] private Button nextButton;
	[SerializeField] private Button previousButton;

	private int currentSlideIndex;

	private int maxSliderIndexValue;
	private int minSliderIndexValue;

	private void Start()
	{
		nextButton.onClick.AddListener(HandleNextSliderComponent);
		previousButton.onClick.AddListener(HandlePreviousSliderComponent);

		maxSliderIndexValue = sliderComponents.Length;
		minSliderIndexValue = 0;

		HandleWindowActivation(minSliderIndexValue);
	}

	public void HandleNextSliderComponent()
	{
		currentSlideIndex++;
		if (currentSlideIndex >= maxSliderIndexValue)
		{
			currentSlideIndex = 0;
		}

		HandleWindowActivation(currentSlideIndex);
	}

	public void HandlePreviousSliderComponent()
	{
		currentSlideIndex--;
		if (minSliderIndexValue > currentSlideIndex)
		{
			currentSlideIndex = maxSliderIndexValue - 1;
		}

		HandleWindowActivation(currentSlideIndex);
	}

	private void HandleWindowActivation(int index)
	{
		for (int i = 0; i < sliderComponents.Length; i++)
		{
			if (index == i)
			{
				sliderComponents[i].SetActive(true);
			}
			else
			{
				sliderComponents[i].SetActive(false);
			}
		}
	}
}
