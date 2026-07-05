using UnityEngine;

namespace FootNailGame
{
    [CreateAssetMenu(fileName = "SalonUpgradeData", menuName = "FootNailGame/Salon Upgrade")]
    public class SalonUpgradeData : ScriptableObject
    {
        public string upgradeId;
        public string displayName;
        public Sprite icon;
        public int maxLevel = 1;
        public int[] upgradeCosts;
        public GameObject[] levelVisuals;
        public float rewardMultiplierPerLevel = 0.1f;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(upgradeId))
                Debug.LogWarning("[SalonUpgradeData] " + name + " is missing upgradeId.", this);

            if (maxLevel < 1)
                Debug.LogWarning("[SalonUpgradeData] " + name + " has maxLevel < 1.", this);

            int expectedCostCount = Mathf.Max(0, maxLevel - 1);
            if (upgradeCosts == null || upgradeCosts.Length < expectedCostCount)
                Debug.LogWarning("[SalonUpgradeData] " + name + " has fewer upgradeCosts entries than expected (" + expectedCostCount + ").", this);

            if (upgradeCosts != null)
            {
                for (int i = 0; i < upgradeCosts.Length; i++)
                {
                    if (upgradeCosts[i] < 0)
                        Debug.LogWarning("[SalonUpgradeData] " + name + " has a negative upgradeCosts entry at index " + i + ".", this);
                }
            }
        }
    }
}
