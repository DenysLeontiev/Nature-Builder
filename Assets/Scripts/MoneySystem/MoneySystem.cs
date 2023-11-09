using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySystem : MonoBehaviour
{
    public static MoneySystem Instance { get; private set; }
    private int moneyAmount;

    private void Awake()
    {
        Instance = this;
    }

    public void AddMoney(int amount)
    {
        moneyAmount += Mathf.Abs(amount);
    }

    public void RemoveMoney(int amount)
    {
        moneyAmount -= Mathf.Abs(amount);
        if(moneyAmount < 0)
        {
            moneyAmount = 0;
        }
    }
}
