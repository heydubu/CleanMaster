using UnityEngine;

namespace FootNailGame
{
    [CreateAssetMenu(fileName = "ToolData", menuName = "FootNailGame/Tool")]
    public class ToolData : ScriptableObject
    {
        public string toolId;
        public string displayName;
        public Sprite icon;
        public GameObject toolPrefab;
        public int maxLevel = 1;
        public float baseEfficiency = 1f;
        public float efficiencyPerLevel = 0.1f;
        public int[] upgradeCosts;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(toolId))
                Debug.LogWarning("[ToolData] " + name + " is missing toolId.", this);

            if (maxLevel < 1)
                Debug.LogWarning("[ToolData] " + name + " has maxLevel < 1.", this);

            int expectedCostCount = Mathf.Max(0, maxLevel - 1);
            if (upgradeCosts == null || upgradeCosts.Length < expectedCostCount)
                Debug.LogWarning("[ToolData] " + name + " has fewer upgradeCosts entries than expected (" + expectedCostCount + ").", this);

            if (upgradeCosts != null)
            {
                for (int i = 0; i < upgradeCosts.Length; i++)
                {
                    if (upgradeCosts[i] < 0)
                        Debug.LogWarning("[ToolData] " + name + " has a negative upgradeCosts entry at index " + i + ".", this);
                }
            }
        }
    }
}
