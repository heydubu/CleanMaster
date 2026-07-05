using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultScreen : UIScreen
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text rewardText;
    [SerializeField] private Button continueButton;
    [SerializeField] private string stageSceneName = "Stage";

    private void Awake()
    {
        continueButton.onClick.AddListener(() => SceneManager.LoadScene(stageSceneName));
    }

    public void ShowResult(int stageIndex, int stars, int coinReward)
    {
        titleText.text = "Stage " + (stageIndex + 1) + " Clear!";
        rewardText.text = "+" + coinReward + " Coins  " + new string('*', stars);
    }
}
