using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] private float selfDestructionTime;

    private void Update()
    {
        HandleSelfDestruction();
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicked on money!!!");
    }

    private void HandleSelfDestruction()
    {
        selfDestructionTime -= Time.deltaTime;
        if (selfDestructionTime <= 0)
        {
            MoneySystem.Instance.AddMoney(1);
            Destroy(gameObject);
        }
    }
}
