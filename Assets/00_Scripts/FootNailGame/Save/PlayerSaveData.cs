using System.Collections.Generic;

namespace FootNailGame
{
    [System.Serializable]
    public class PlayerSaveData
    {
        public int saveVersion = 1;
        public int coins;
        public int salonExp;
        public int salonLevel = 1;
        public List<CustomerProgressData> customerProgress = new List<CustomerProgressData>();
        public List<StageGroupProgressData> stageGroupProgress = new List<StageGroupProgressData>();
        public List<string> unlockedNailDesignIds = new List<string>();
        public List<ToolProgressData> toolProgress = new List<ToolProgressData>();
        public List<SalonUpgradeProgressData> salonUpgradeProgress = new List<SalonUpgradeProgressData>();
        public List<RouletteProgressData> rouletteProgress = new List<RouletteProgressData>();
    }

    [System.Serializable]
    public class CustomerProgressData
    {
        public string customerStageId;
        public bool isCleared;
        public int bestStars;
        public int bestScore;
        public float bestCompletionTime;
        public int clearCount;
    }

    [System.Serializable]
    public class StageGroupProgressData
    {
        public string stageGroupId;
        public bool isUnlocked;
        public int totalStars;
        public bool completionRewardClaimed;
        public int clearCount;
    }

    [System.Serializable]
    public class ToolProgressData
    {
        public string toolId;
        public int level;
    }

    [System.Serializable]
    public class SalonUpgradeProgressData
    {
        public string upgradeId;
        public int level;
    }

    [System.Serializable]
    public class RouletteProgressData
    {
        public string stageGroupId;
        public int remainingFreeSpins;
        public string previousCustomerId;
    }
}
