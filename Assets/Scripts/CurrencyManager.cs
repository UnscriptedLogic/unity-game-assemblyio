using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    [SerializeField] private int startingCurrency = 100;
    private int currentCurrency = 0;

    public int CurrentCurrency => currentCurrency;

    public delegate void OnCurrencyUpdated(int currentAmount);
    public OnCurrencyUpdated onCurrencyUpdated;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentCurrency = startingCurrency;
        onCurrencyUpdated?.Invoke(currentCurrency);
    }

    public void AddCurrency(int amount)
    {
        currentCurrency += amount;
        onCurrencyUpdated?.Invoke(currentCurrency);
    }

    public void RemoveCurrency(int amount)
    {
        currentCurrency -= amount;
        onCurrencyUpdated?.Invoke(currentCurrency);
    }
}
