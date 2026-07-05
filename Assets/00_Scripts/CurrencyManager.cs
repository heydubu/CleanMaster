using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    private const string CoinsKey = "currency_coins";
    private const string GemsKey = "currency_gems";

    public int Coins { get; private set; }
    public int Gems { get; private set; }

    public event Action<int> OnCoinsChanged;
    public event Action<int> OnGemsChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Coins = PlayerPrefs.GetInt(CoinsKey, 0);
        Gems = PlayerPrefs.GetInt(GemsKey, 0);
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        PlayerPrefs.SetInt(CoinsKey, Coins);
        OnCoinsChanged?.Invoke(Coins);
    }

    public bool SpendCoins(int amount)
    {
        if (Coins < amount)
            return false;

        Coins -= amount;
        PlayerPrefs.SetInt(CoinsKey, Coins);
        OnCoinsChanged?.Invoke(Coins);
        return true;
    }

    public void AddGems(int amount)
    {
        Gems += amount;
        PlayerPrefs.SetInt(GemsKey, Gems);
        OnGemsChanged?.Invoke(Gems);
    }
}
