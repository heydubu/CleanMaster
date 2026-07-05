using System.Collections.Generic;
using UnityEngine;

namespace FootNailGame
{
    [CreateAssetMenu(fileName = "WorldData", menuName = "FootNailGame/World")]
    public class WorldData : ScriptableObject
    {
        public string worldId;
        public string displayName;
        public string countryCode;
        public Sprite mapBackground;
        public List<StageGroupData> stageGroups = new List<StageGroupData>();

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(worldId))
                Debug.LogWarning("[WorldData] " + name + " is missing worldId.", this);

            var seen = new HashSet<StageGroupData>();
            for (int i = 0; i < stageGroups.Count; i++)
            {
                StageGroupData group = stageGroups[i];
                if (group == null)
                {
                    Debug.LogWarning("[WorldData] " + name + " has a null StageGroupData at index " + i + ".", this);
                    continue;
                }

                if (!seen.Add(group))
                    Debug.LogWarning("[WorldData] " + name + " has a duplicate StageGroupData reference: " + group.name + ".", this);
            }
        }
    }
}
