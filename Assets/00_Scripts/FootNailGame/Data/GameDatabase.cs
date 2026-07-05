using System.Collections.Generic;
using UnityEngine;

namespace FootNailGame
{
    [CreateAssetMenu(fileName = "GameDatabase", menuName = "FootNailGame/Game Database")]
    public class GameDatabase : ScriptableObject
    {
        public List<WorldData> worlds = new List<WorldData>();
        public List<StageGroupData> stageGroups = new List<StageGroupData>();
        public List<CustomerStageData> customers = new List<CustomerStageData>();
        public List<ToolData> tools = new List<ToolData>();
        public List<NailDesignData> nailDesigns = new List<NailDesignData>();
        public List<SalonUpgradeData> salonUpgrades = new List<SalonUpgradeData>();

        private Dictionary<string, WorldData> _worldById;
        private Dictionary<string, StageGroupData> _stageGroupById;
        private Dictionary<string, CustomerStageData> _customerById;
        private Dictionary<string, ToolData> _toolById;
        private Dictionary<string, NailDesignData> _nailDesignById;
        private Dictionary<string, SalonUpgradeData> _salonUpgradeById;

        private bool _initialized;

        private void OnEnable()
        {
            Initialize();
        }

        public void Initialize()
        {
            _worldById = BuildLookup(worlds, w => w.worldId, "WorldData");
            _stageGroupById = BuildLookup(stageGroups, s => s.stageGroupId, "StageGroupData");
            _customerById = BuildLookup(customers, c => c.customerStageId, "CustomerStageData");
            _toolById = BuildLookup(tools, t => t.toolId, "ToolData");
            _nailDesignById = BuildLookup(nailDesigns, n => n.designId, "NailDesignData");
            _salonUpgradeById = BuildLookup(salonUpgrades, u => u.upgradeId, "SalonUpgradeData");

            _initialized = true;
        }

        public WorldData GetWorld(string id)
        {
            return GetFromLookup(_worldById, id, "WorldData");
        }

        public StageGroupData GetStageGroup(string id)
        {
            return GetFromLookup(_stageGroupById, id, "StageGroupData");
        }

        public CustomerStageData GetCustomer(string id)
        {
            return GetFromLookup(_customerById, id, "CustomerStageData");
        }

        public ToolData GetTool(string id)
        {
            return GetFromLookup(_toolById, id, "ToolData");
        }

        public NailDesignData GetNailDesign(string id)
        {
            return GetFromLookup(_nailDesignById, id, "NailDesignData");
        }

        public SalonUpgradeData GetSalonUpgrade(string id)
        {
            return GetFromLookup(_salonUpgradeById, id, "SalonUpgradeData");
        }

        private static Dictionary<string, T> BuildLookup<T>(List<T> source, System.Func<T, string> idSelector, string typeName) where T : Object
        {
            var lookup = new Dictionary<string, T>();

            for (int i = 0; i < source.Count; i++)
            {
                T item = source[i];
                if (item == null)
                {
                    Debug.LogWarning("[GameDatabase] Null " + typeName + " entry at index " + i + ".");
                    continue;
                }

                string id = idSelector(item);
                if (string.IsNullOrEmpty(id))
                {
                    Debug.LogWarning("[GameDatabase] " + typeName + " '" + item.name + "' has an empty ID and was skipped.");
                    continue;
                }

                if (lookup.ContainsKey(id))
                {
                    Debug.LogError("[GameDatabase] Duplicate " + typeName + " ID '" + id + "' found on '" + item.name + "'. Keeping the first entry.");
                    continue;
                }

                lookup.Add(id, item);
            }

            return lookup;
        }

        private T GetFromLookup<T>(Dictionary<string, T> lookup, string id, string typeName) where T : Object
        {
            if (!_initialized)
                Initialize();

            T result;
            if (lookup == null || string.IsNullOrEmpty(id) || !lookup.TryGetValue(id, out result))
            {
                Debug.LogWarning("[GameDatabase] " + typeName + " with ID '" + id + "' was not found.");
                return null;
            }

            return result;
        }

        [ContextMenu("Validate Database")]
        public void ValidateDatabase()
        {
            Initialize();

            var allIds = new Dictionary<string, string>();

            CheckCrossListDuplicates(worlds, w => w.worldId, "WorldData", allIds);
            CheckCrossListDuplicates(stageGroups, s => s.stageGroupId, "StageGroupData", allIds);
            CheckCrossListDuplicates(customers, c => c.customerStageId, "CustomerStageData", allIds);
            CheckCrossListDuplicates(tools, t => t.toolId, "ToolData", allIds);
            CheckCrossListDuplicates(nailDesigns, n => n.designId, "NailDesignData", allIds);
            CheckCrossListDuplicates(salonUpgrades, u => u.upgradeId, "SalonUpgradeData", allIds);

            Debug.Log("[GameDatabase] Validation complete. Worlds=" + worlds.Count +
                      " StageGroups=" + stageGroups.Count +
                      " Customers=" + customers.Count +
                      " Tools=" + tools.Count +
                      " NailDesigns=" + nailDesigns.Count +
                      " SalonUpgrades=" + salonUpgrades.Count);
        }

        private static void CheckCrossListDuplicates<T>(List<T> source, System.Func<T, string> idSelector, string typeName, Dictionary<string, string> allIds) where T : Object
        {
            foreach (T item in source)
            {
                if (item == null)
                    continue;

                string id = idSelector(item);
                if (string.IsNullOrEmpty(id))
                    continue;

                string existingType;
                if (allIds.TryGetValue(id, out existingType))
                {
                    Debug.LogError("[GameDatabase] ID '" + id + "' is used by both " + existingType + " and " + typeName + ". IDs must be unique across all data types.");
                    continue;
                }

                allIds.Add(id, typeName);
            }
        }
    }
}
