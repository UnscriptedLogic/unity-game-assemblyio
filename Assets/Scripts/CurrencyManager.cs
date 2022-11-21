using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    [SerializeField] private int startingCurrency = 100;
    [SerializeField] private TextMeshProUGUI currencyTMP;

    private int currentCurrency = 0;

    public int CurrentCurrency => currentCurrency;

    public delegate void OnCurrencyUpdated(int currentAmount);
    public OnCurrencyUpdated onCurrencyUpdated;

    private void Awake()
    {
        instance = this;
        onCurrencyUpdated += CurrencyUpdated;
    }

    private void CurrencyUpdated(int currentAmount)
    {
        currencyTMP.text = currentAmount.ToString();
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

    public bool CanAfford(int amount)
    {
        return currentCurrency >= amount;
    }
}
