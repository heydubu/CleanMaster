using UnityEngine;
using UnityEngine.UI;

public class StageLobbyScreen : UIScreen
{
    [SerializeField] private Button selectCustomerButton;

    private void Awake()
    {
        selectCustomerButton.onClick.AddListener(() => StageFlowManager.Instance.ShowStageMap());
    }
}
