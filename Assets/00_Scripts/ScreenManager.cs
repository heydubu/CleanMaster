using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance { get; private set; }

    [SerializeField] private UIScreen lobbyScreen;
    [SerializeField] private UIScreen stageMapScreen;
    [SerializeField] private UIScreen miniGameScreen;
    [SerializeField] private UIScreen resultScreen;

    private UIScreen _current;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ShowLobby();
    }

    public void ShowLobby() => SwitchTo(lobbyScreen);
    public void ShowStageMap() => SwitchTo(stageMapScreen);
    public void ShowMiniGame() => SwitchTo(miniGameScreen);
    public void ShowResult() => SwitchTo(resultScreen);

    private void SwitchTo(UIScreen screen)
    {
        if (_current != null)
            _current.Hide();

        _current = screen;

        if (_current != null)
            _current.Show();
    }
}
