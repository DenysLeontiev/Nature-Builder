using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] private float selfDestructionTime;
    [SerializeField] private float fadeSpeedMin = 1f;
    [SerializeField] private float fadeSpeedMax = 3f;

    private float fadeSpeed;
    private bool isFadeOut;

    private void Start()
    {
        fadeSpeed = Random.Range(fadeSpeedMin, fadeSpeedMax);
    }

    private void Update()
    {
        HandleSelfDestruction();
        MoneyFadeOut();
    }

    private void MoneyFadeOut()
    {
        if (isFadeOut)
        {
            Color moneyColor = transform.GetComponent<Renderer>().material.color;
            float fadeAmount = moneyColor.a - (fadeSpeed * Time.deltaTime);

            moneyColor = new Color(moneyColor.r, moneyColor.g, moneyColor.b, fadeAmount);
            this.GetComponent<Renderer>().material.color = moneyColor;

            if (moneyColor.a < 0)
            {
                AddMoney();
            }
        }
    }

    private void HandleSelfDestruction()
    {
        selfDestructionTime -= Time.deltaTime;
        if (selfDestructionTime <= 0)
        {
            isFadeOut = true;
        }
    }

    private void AddMoney()
    {
        MoneySystem.Instance.AddMoney(1);
        Destroy(gameObject);
    }
}
