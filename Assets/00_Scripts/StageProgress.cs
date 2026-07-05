using UnityEngine;

public static class StageProgress
{
    public const int TotalStages = 9;

    private const string UnlockedStageKey = "stage_unlocked_count";

    public static int UnlockedStageCount
    {
        get => Mathf.Clamp(PlayerPrefs.GetInt(UnlockedStageKey, 1), 1, TotalStages);
        private set => PlayerPrefs.SetInt(UnlockedStageKey, Mathf.Clamp(value, 1, TotalStages));
    }

    public static bool IsUnlocked(int stageIndex)
    {
        return stageIndex < UnlockedStageCount;
    }

    public static int GetStars(int stageIndex)
    {
        return PlayerPrefs.GetInt(StarsKey(stageIndex), 0);
    }

    public static void CompleteStage(int stageIndex, int stars)
    {
        int existingStars = GetStars(stageIndex);
        if (stars > existingStars)
            PlayerPrefs.SetInt(StarsKey(stageIndex), stars);

        if (stageIndex + 1 >= UnlockedStageCount)
            UnlockedStageCount = stageIndex + 2;
    }

    private static string StarsKey(int stageIndex)
    {
        return "stage_" + stageIndex + "_stars";
    }
}
