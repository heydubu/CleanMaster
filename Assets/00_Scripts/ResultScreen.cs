using UnityEngine;
using UnityEngine.UI;

public class ResultScreen : UIScreen
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text rewardText;
    [SerializeField] private Button continueButton;

    private void Awake()
    {
        continueButton.onClick.AddListener(() => ScreenManager.Instance.ShowStageMap());
    }

    public void ShowResult(int stageIndex, int stars, int coinReward)
    {
        titleText.text = "Stage " + (stageIndex + 1) + " Clear!";
        rewardText.text = "+" + coinReward + " Coins  " + new string('*', stars);
    }
}
