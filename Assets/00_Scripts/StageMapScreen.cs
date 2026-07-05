using UnityEngine;
using UnityEngine.UI;

public class StageMapScreen : UIScreen
{
    [SerializeField] private RectTransform buttonContainer;
    [SerializeField] private Button stageButtonTemplate;
    [SerializeField] private Button backButton;

    private bool _builtOnce;

    private void Awake()
    {
        backButton.onClick.AddListener(() => ScreenManager.Instance.ShowLobby());
        stageButtonTemplate.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        BuildStageButtons();
    }

    private void BuildStageButtons()
    {
        if (_builtOnce)
        {
            for (int i = buttonContainer.childCount - 1; i >= 0; i--)
            {
                Transform child = buttonContainer.GetChild(i);
                if (child == stageButtonTemplate.transform)
                    continue;
                Destroy(child.gameObject);
            }
        }

        _builtOnce = true;

        for (int i = 0; i < StageProgress.TotalStages; i++)
        {
            int stageIndex = i;
            Button button = Instantiate(stageButtonTemplate, buttonContainer);
            button.gameObject.SetActive(true);

            bool unlocked = StageProgress.IsUnlocked(stageIndex);
            button.interactable = unlocked;

            Text label = button.GetComponentInChildren<Text>();
            if (label != null)
            {
                if (unlocked)
                {
                    int stars = StageProgress.GetStars(stageIndex);
                    label.text = (stageIndex + 1) + "\n" + new string('*', stars);
                }
                else
                {
                    label.text = "LOCK";
                }
            }

            button.onClick.AddListener(() => GameManager.Instance.StartStage(stageIndex));
        }
    }
}
