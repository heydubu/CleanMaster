using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FootNailGame
{
    // Static single-access-point store. No MonoBehaviour/DontDestroyOnLoad is needed because
    // nothing here uses Coroutines or Inspector references.
    public static class SaveManager
    {
        private const string SaveFileName = "foot_nail_save.json";
        private const int CurrentSaveVersion = 1;

        private static PlayerSaveData _currentData;
        private static bool _isLoaded;

        public static PlayerSaveData CurrentData
        {
            get
            {
                if (!_isLoaded)
                    Initialize();

                return _currentData;
            }
            private set { _currentData = value; }
        }

        public static void Initialize()
        {
            if (_isLoaded)
                return;

            Load();
        }

        public static void Load()
        {
            string path = GetSavePath();

            if (!File.Exists(path))
            {
                CurrentData = CreateDefaultSave();
                _isLoaded = true;
                return;
            }

            string json;
            try
            {
                json = File.ReadAllText(path);
            }
            catch (Exception e)
            {
                Debug.LogError("[SaveManager] Failed to read save file: " + e.Message);
                CurrentData = RecoverFromInvalidSave();
                _isLoaded = true;
                return;
            }

            if (string.IsNullOrEmpty(json))
            {
                Debug.LogWarning("[SaveManager] Save file was empty. Recovering with default data.");
                CurrentData = RecoverFromInvalidSave();
                _isLoaded = true;
                return;
            }

            PlayerSaveData data = null;
            try
            {
                data = JsonUtility.FromJson<PlayerSaveData>(json);
            }
            catch (Exception e)
            {
                Debug.LogError("[SaveManager] Failed to parse save file JSON: " + e.Message);
            }

            if (data == null)
            {
                CurrentData = RecoverFromInvalidSave();
            }
            else
            {
                SanitizeData(data);
                CurrentData = data;
            }

            _isLoaded = true;
        }

        public static void Save()
        {
            if (!_isLoaded)
            {
                Debug.LogWarning("[SaveManager] Save called before Load/Initialize. Loading first.");
                Initialize();
            }

            SanitizeData(CurrentData);

            string json;
            try
            {
                json = JsonUtility.ToJson(CurrentData, true);
            }
            catch (Exception e)
            {
                Debug.LogError("[SaveManager] Failed to serialize save data: " + e.Message);
                return;
            }

            string savePath = GetSavePath();
            string tempPath = savePath + ".tmp";
            string backupPath = savePath + ".bak";

            try
            {
                File.WriteAllText(tempPath, json);

                if (File.Exists(savePath))
                {
                    File.Copy(savePath, backupPath, true);
                    File.Delete(savePath);
                }

                File.Move(tempPath, savePath);
            }
            catch (Exception e)
            {
                Debug.LogError("[SaveManager] Failed to save data to '" + savePath + "': " + e.Message);

                if (!File.Exists(savePath) && File.Exists(backupPath))
                {
                    try
                    {
                        File.Copy(backupPath, savePath, true);
                        Debug.LogWarning("[SaveManager] Restored the previous save file from backup after a failed save.");
                    }
                    catch (Exception restoreException)
                    {
                        Debug.LogError("[SaveManager] Failed to restore backup save file: " + restoreException.Message);
                    }
                }
            }
        }

        public static void DeleteSave()
        {
            string path = GetSavePath();

            try
            {
                if (File.Exists(path))
                    File.Delete(path);

                string backupPath = path + ".bak";
                if (File.Exists(backupPath))
                    File.Delete(backupPath);
            }
            catch (Exception e)
            {
                Debug.LogError("[SaveManager] Failed to delete save file: " + e.Message);
            }

            CurrentData = CreateDefaultSave();
            _isLoaded = true;
        }

        public static PlayerSaveData CreateDefaultSave()
        {
            return new PlayerSaveData();
        }

        public static PlayerSaveData RecoverFromInvalidSave()
        {
            Debug.LogWarning("[SaveManager] Recovering from invalid/corrupted save data with a new default save.");
            return CreateDefaultSave();
        }

        public static CustomerProgressData GetOrCreateCustomerProgress(string customerStageId)
        {
            if (string.IsNullOrEmpty(customerStageId))
            {
                Debug.LogWarning("[SaveManager] GetOrCreateCustomerProgress called with an empty ID.");
                return null;
            }

            List<CustomerProgressData> list = CurrentData.customerProgress;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].customerStageId == customerStageId)
                    return list[i];
            }

            var progress = new CustomerProgressData { customerStageId = customerStageId };
            list.Add(progress);
            return progress;
        }

        public static StageGroupProgressData GetOrCreateStageGroupProgress(string stageGroupId)
        {
            if (string.IsNullOrEmpty(stageGroupId))
            {
                Debug.LogWarning("[SaveManager] GetOrCreateStageGroupProgress called with an empty ID.");
                return null;
            }

            List<StageGroupProgressData> list = CurrentData.stageGroupProgress;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].stageGroupId == stageGroupId)
                    return list[i];
            }

            var progress = new StageGroupProgressData { stageGroupId = stageGroupId };
            list.Add(progress);
            return progress;
        }

        public static ToolProgressData GetOrCreateToolProgress(string toolId)
        {
            if (string.IsNullOrEmpty(toolId))
            {
                Debug.LogWarning("[SaveManager] GetOrCreateToolProgress called with an empty ID.");
                return null;
            }

            List<ToolProgressData> list = CurrentData.toolProgress;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].toolId == toolId)
                    return list[i];
            }

            var progress = new ToolProgressData { toolId = toolId };
            list.Add(progress);
            return progress;
        }

        public static SalonUpgradeProgressData GetOrCreateSalonUpgradeProgress(string upgradeId)
        {
            if (string.IsNullOrEmpty(upgradeId))
            {
                Debug.LogWarning("[SaveManager] GetOrCreateSalonUpgradeProgress called with an empty ID.");
                return null;
            }

            List<SalonUpgradeProgressData> list = CurrentData.salonUpgradeProgress;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].upgradeId == upgradeId)
                    return list[i];
            }

            var progress = new SalonUpgradeProgressData { upgradeId = upgradeId };
            list.Add(progress);
            return progress;
        }

        public static RouletteProgressData GetOrCreateRouletteProgress(string stageGroupId)
        {
            if (string.IsNullOrEmpty(stageGroupId))
            {
                Debug.LogWarning("[SaveManager] GetOrCreateRouletteProgress called with an empty ID.");
                return null;
            }

            List<RouletteProgressData> list = CurrentData.rouletteProgress;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].stageGroupId == stageGroupId)
                    return list[i];
            }

            var progress = new RouletteProgressData { stageGroupId = stageGroupId };
            list.Add(progress);
            return progress;
        }

        public static bool IsNailDesignUnlocked(string designId)
        {
            if (string.IsNullOrEmpty(designId))
            {
                Debug.LogWarning("[SaveManager] IsNailDesignUnlocked called with an empty ID.");
                return false;
            }

            return CurrentData.unlockedNailDesignIds.Contains(designId);
        }

        private static void SanitizeData(PlayerSaveData data)
        {
            if (data.customerProgress == null) data.customerProgress = new List<CustomerProgressData>();
            if (data.stageGroupProgress == null) data.stageGroupProgress = new List<StageGroupProgressData>();
            if (data.unlockedNailDesignIds == null) data.unlockedNailDesignIds = new List<string>();
            if (data.toolProgress == null) data.toolProgress = new List<ToolProgressData>();
            if (data.salonUpgradeProgress == null) data.salonUpgradeProgress = new List<SalonUpgradeProgressData>();
            if (data.rouletteProgress == null) data.rouletteProgress = new List<RouletteProgressData>();

            if (data.saveVersion <= 0)
                data.saveVersion = CurrentSaveVersion;

            if (data.salonLevel < 1)
                data.salonLevel = 1;

            if (data.coins < 0)
                data.coins = 0;

            if (data.salonExp < 0)
                data.salonExp = 0;

            RemoveDuplicateNailDesignIds(data);
        }

        private static void RemoveDuplicateNailDesignIds(PlayerSaveData data)
        {
            var seen = new HashSet<string>();
            for (int i = data.unlockedNailDesignIds.Count - 1; i >= 0; i--)
            {
                string id = data.unlockedNailDesignIds[i];
                if (string.IsNullOrEmpty(id) || !seen.Add(id))
                    data.unlockedNailDesignIds.RemoveAt(i);
            }
        }

        private static string GetSavePath()
        {
            return Path.Combine(Application.persistentDataPath, SaveFileName);
        }
    }
}
