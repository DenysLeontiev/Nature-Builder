using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
	public static Tooltip Instance;

	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private TextMeshProUGUI descriptionText;
	[SerializeField] private Image spriteImage;

	private RectTransform backgroundRectTransform;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		backgroundRectTransform = GetComponent<RectTransform>();
	}

	private void Update()
    {
        if(gameObject.activeInHierarchy)
		{
			HandlePosition();
		}
	}

	public void SetTooltipInfo(string nameTxt, string descTxt, Sprite sprite)
	{
		nameText.text = nameTxt;
		descriptionText.text = descTxt;
		spriteImage.sprite = sprite;
	}

	private void HandlePosition()
	{
		float width = backgroundRectTransform.rect.width;
		float height = backgroundRectTransform.rect.height;

		Vector3 newPos = new Vector3(Input.mousePosition.x + (width / 2), Input.mousePosition.y - (height / 2), Input.mousePosition.z);

		transform.position = newPos;
	}
}
