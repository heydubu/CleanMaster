using System.Collections.Generic;
using UnityEngine;

namespace FootNailGame
{
    [CreateAssetMenu(fileName = "StageGroupData", menuName = "FootNailGame/Stage Group")]
    public class StageGroupData : ScriptableObject
    {
        public string stageGroupId;
        public string displayName;
        public int mapNumber;
        public Sprite stageThumbnail;
        public Sprite lobbyBackground;
        public List<TreatmentType> enabledTreatmentTypes = new List<TreatmentType>();
        public StageDifficultyData difficulty = new StageDifficultyData();
        public List<RouletteCustomerEntry> rouletteCustomers = new List<RouletteCustomerEntry>();
        public int unlockRequiredStars;
        public int freeSpinCount;
        public int respinCoinCost;
        public float completedCustomerWeightMultiplier = 1f;
        public float previousCustomerWeightMultiplier = 1f;
        public int firstClearRewardCoins;
        public int repeatClearRewardCoins;
        public int salonExpReward;
        public bool showBeforeAfterResult = true;
        public bool registerSelectedNailDesign = true;
        public bool grantSalonReward = true;
        public NailDesignData completionRewardNailDesign;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(stageGroupId))
                Debug.LogWarning("[StageGroupData] " + name + " is missing stageGroupId.", this);

            if (enabledTreatmentTypes.Count == 0)
                Debug.LogWarning("[StageGroupData] " + name + " has no enabledTreatmentTypes.", this);

            var seenTypes = new HashSet<TreatmentType>();
            foreach (TreatmentType type in enabledTreatmentTypes)
            {
                if (!seenTypes.Add(type))
                    Debug.LogWarning("[StageGroupData] " + name + " has a duplicate TreatmentType in enabledTreatmentTypes: " + type + ".", this);
            }

            if (rouletteCustomers.Count == 0)
                Debug.LogWarning("[StageGroupData] " + name + " has no rouletteCustomers.", this);

            var seenCustomers = new HashSet<CustomerStageData>();
            for (int i = 0; i < rouletteCustomers.Count; i++)
            {
                RouletteCustomerEntry entry = rouletteCustomers[i];
                if (entry == null)
                {
                    Debug.LogWarning("[StageGroupData] " + name + " has a null RouletteCustomerEntry at index " + i + ".", this);
                    continue;
                }

                if (entry.customer == null)
                {
                    Debug.LogWarning("[StageGroupData] " + name + " has a RouletteCustomerEntry with no customer at index " + i + ".", this);
                    continue;
                }

                if (entry.baseWeight <= 0)
                    Debug.LogWarning("[StageGroupData] " + name + " has a RouletteCustomerEntry with baseWeight <= 0 for customer " + entry.customer.name + ".", this);

                if (!seenCustomers.Add(entry.customer))
                    Debug.LogWarning("[StageGroupData] " + name + " has a duplicate roulette customer: " + entry.customer.name + ".", this);
            }

            if (difficulty != null)
            {
                if (difficulty.timeLimit < 0f)
                    Debug.LogWarning("[StageGroupData] " + name + " has a negative difficulty.timeLimit.", this);
                if (difficulty.maxMistakes < 0)
                    Debug.LogWarning("[StageGroupData] " + name + " has a negative difficulty.maxMistakes.", this);
                if (difficulty.targetScale <= 0f)
                    Debug.LogWarning("[StageGroupData] " + name + " has difficulty.targetScale <= 0.", this);
                if (difficulty.toolEfficiencyMultiplier <= 0f)
                    Debug.LogWarning("[StageGroupData] " + name + " has difficulty.toolEfficiencyMultiplier <= 0.", this);
                if (difficulty.dirtCount < 0 || difficulty.damagedAreaCount < 0 || difficulty.targetCount < 0 ||
                    difficulty.simultaneousTargetCount < 0 || difficulty.customerRequestCount < 0)
                    Debug.LogWarning("[StageGroupData] " + name + " has a negative difficulty count value.", this);
                if (difficulty.requiredAccuracyPercent < 0 || difficulty.requiredAccuracyPercent > 100)
                    Debug.LogWarning("[StageGroupData] " + name + " has difficulty.requiredAccuracyPercent out of the 0-100 range.", this);
            }

            if (unlockRequiredStars < 0)
                Debug.LogWarning("[StageGroupData] " + name + " has a negative unlockRequiredStars.", this);
        }
    }
}
