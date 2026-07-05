using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageLobbyScreen : UIScreen
{
    [SerializeField] private Button selectCustomerButton;
    [SerializeField] private Button backButton;
    [SerializeField] private RouletteWheel rouletteWheel;
    [SerializeField] private Text resultText;
    [SerializeField] private string inGameSceneName = "InGame";
    [SerializeField] private float resultDelay = 1f;

    private void Awake()
    {
        selectCustomerButton.onClick.AddListener(StartRoulette);
        backButton.onClick.AddListener(() => StageFlowManager.Instance.ShowStageMap());
    }

    private void OnEnable()
    {
        selectCustomerButton.interactable = true;
        if (resultText != null)
            resultText.text = string.Empty;
    }

    private void StartRoulette()
    {
        selectCustomerButton.interactable = false;
        rouletteWheel.Spin(OnCustomerSelected);
    }

    private void OnCustomerSelected(int index, string customerName)
    {
        if (resultText != null)
            resultText.text = customerName + " 손님 입장!";

        StartCoroutine(EnterGameAfterDelay());
    }

    private IEnumerator EnterGameAfterDelay()
    {
        yield return new WaitForSeconds(resultDelay);
        SceneManager.LoadScene(inGameSceneName);
    }
}
