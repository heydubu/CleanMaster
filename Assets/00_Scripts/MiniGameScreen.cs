using System.Collections;
using UnityEngine;

public class MiniGameScreen : UIScreen
{
    [SerializeField] private DirtScrapeMiniGame dirtScrapeMiniGame;
    [SerializeField] private float resultDelay = 1f;

    private void Awake()
    {
        dirtScrapeMiniGame.OnCompleted += HandleCompleted;
    }

    public void BeginStage(int stageIndex)
    {
        dirtScrapeMiniGame.ResetGame();
    }

    private void HandleCompleted(float progress)
    {
        StartCoroutine(CompleteAfterDelay(progress));
    }

    private IEnumerator CompleteAfterDelay(float progress)
    {
        yield return new WaitForSeconds(resultDelay);
        GameManager.Instance.CompleteCurrentStage(progress);
    }
}
