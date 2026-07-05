using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private MiniGameScreen miniGameScreen;
    [SerializeField] private ResultScreen resultScreen;

    private const int BaseCoinReward = 100;

    public int CurrentStageIndex { get; private set; } = -1;

    private void Awake()
    {
        Instance = this;
    }

    public void StartStage(int stageIndex)
    {
        CurrentStageIndex = stageIndex;
        ScreenManager.Instance.ShowMiniGame();
        miniGameScreen.BeginStage(stageIndex);
    }

    public void CompleteCurrentStage(float progress)
    {
        int stars = progress >= 0.95f ? 3 : progress >= 0.8f ? 2 : 1;
        int coinReward = BaseCoinReward * stars;

        StageProgress.CompleteStage(CurrentStageIndex, stars);
        CurrencyManager.Instance.AddCoins(coinReward);

        resultScreen.ShowResult(CurrentStageIndex, stars, coinReward);
        ScreenManager.Instance.ShowResult();
    }
}
