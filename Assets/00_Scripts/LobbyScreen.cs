using UnityEngine;
using UnityEngine.UI;

public class LobbyScreen : UIScreen
{
    [SerializeField] private Button startButton;
    [SerializeField] private Text coinsText;
    [SerializeField] private Text gemsText;

    private void Awake()
    {
        startButton.onClick.AddListener(() => ScreenManager.Instance.ShowStageMap());
    }

    private void OnEnable()
    {
        RefreshCurrency();
        CurrencyManager.Instance.OnCoinsChanged += RefreshCurrency;
        CurrencyManager.Instance.OnGemsChanged += RefreshCurrency;
    }

    private void OnDisable()
    {
        CurrencyManager.Instance.OnCoinsChanged -= RefreshCurrency;
        CurrencyManager.Instance.OnGemsChanged -= RefreshCurrency;
    }

    private void RefreshCurrency(int _ = 0)
    {
        coinsText.text = CurrencyManager.Instance.Coins.ToString();
        gemsText.text = CurrencyManager.Instance.Gems.ToString();
    }
}
