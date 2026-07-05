using UnityEngine;

public class StageFlowManager : MonoBehaviour
{
    public static StageFlowManager Instance { get; private set; }

    [SerializeField] private UIScreen lobbyScreen;
    [SerializeField] private UIScreen stageMapScreen;

    private UIScreen _current;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ShowStageMap();
    }

    public void ShowLobby() => SwitchTo(lobbyScreen);
    public void ShowStageMap() => SwitchTo(stageMapScreen);

    private void SwitchTo(UIScreen screen)
    {
        if (_current != null)
            _current.Hide();

        _current = screen;

        if (_current != null)
            _current.Show();
    }
}
