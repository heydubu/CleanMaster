using UnityEngine;

namespace FootNailGame
{
    [CreateAssetMenu(fileName = "StageResultSettings", menuName = "FootNailGame/Result Settings")]
    public class StageResultSettings : ScriptableObject
    {
        public int baseClearScore = 100;
        public int perfectAccuracyBonus = 50;
        public int requestedShapeBonus = 30;
        public int requestedDesignBonus = 30;
        public int timeBonusMaximum = 50;
        public int mistakePenaltyPerCount = 10;
        public int threeStarMinimumScore = 240;
        public int twoStarMinimumScore = 150;
        public int oneStarMinimumScore;

        private void OnValidate()
        {
            if (threeStarMinimumScore < twoStarMinimumScore || twoStarMinimumScore < oneStarMinimumScore)
                Debug.LogWarning("[StageResultSettings] " + name + " star thresholds are not in descending order (three >= two >= one).", this);

            if (baseClearScore < 0 || perfectAccuracyBonus < 0 || requestedShapeBonus < 0 ||
                requestedDesignBonus < 0 || timeBonusMaximum < 0 || mistakePenaltyPerCount < 0)
                Debug.LogWarning("[StageResultSettings] " + name + " has a negative score value.", this);
        }
    }
}
