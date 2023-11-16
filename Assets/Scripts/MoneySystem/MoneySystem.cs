using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneySystem : MonoBehaviour
{
    public static MoneySystem Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI currentMoneyTextUI;
    [SerializeField] private int minMoneyAmountToStartGameWith = 1;

    private int moneyAmount;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        moneyAmount = minMoneyAmountToStartGameWith;
        SetCurrentMoneyText(moneyAmount);
    }

    public int GetMoneyAmount()
    {
        return moneyAmount;
    }

    public void AddMoney(int amount)
    {
        moneyAmount += Mathf.Abs(amount);
        SetCurrentMoneyText(moneyAmount);
    }

    public void RemoveMoney(int amount)
    {
        moneyAmount -= Mathf.Abs(amount);

        if(moneyAmount < 0)
        {
            moneyAmount = 0;
        }
        SetCurrentMoneyText(moneyAmount);
    }

    private void SetCurrentMoneyText(int currentMoney)
    {
        currentMoneyTextUI.text = $"Current Money: {currentMoney}";
    }
}
