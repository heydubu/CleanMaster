using UnityEditor;
using UnityEngine;

namespace FootNailGame
{
    public static class SaveManagerTestMenu
    {
        [MenuItem("FootNailGame/Test/Run SaveManager Tests")]
        public static void RunTests()
        {
            int passed = 0;
            int total = 0;

            total++; if (Test_LoadWithNoFile()) passed++;
            total++; if (Test_DefaultDataCreated()) passed++;
            total++; if (Test_SaveAndReloadCoins()) passed++;
            total++; if (Test_RecoverFromCorruptedFile()) passed++;
            total++; if (Test_DeleteSaveRecreatesDefault()) passed++;
            total++; if (Test_CustomerProgressNoDuplicate()) passed++;
            total++; if (Test_NailDesignNoDuplicate()) passed++;
            total++; if (Test_GameSessionClearAll()) passed++;
            total++; if (Test_SceneLoaderBlocksInvalidInGame()) passed++;

            Debug.Log("[SaveManagerTestMenu] " + passed + "/" + total + " tests passed.");

            // Leave a clean default save behind so this test run does not pollute real play data.
            SaveManager.DeleteSave();
        }

        private static bool Test_LoadWithNoFile()
        {
            SaveManager.DeleteSave();
            SaveManager.Load();
            bool ok = SaveManager.CurrentData != null;
            LogResult("Load with no save file creates data", ok);
            return ok;
        }

        private static bool Test_DefaultDataCreated()
        {
            PlayerSaveData data = SaveManager.CurrentData;
            bool ok = data.saveVersion == 1 && data.salonLevel == 1 && data.coins == 0 &&
                      data.customerProgress != null && data.stageGroupProgress != null &&
                      data.unlockedNailDesignIds != null && data.toolProgress != null &&
                      data.salonUpgradeProgress != null && data.rouletteProgress != null;
            LogResult("Default save data is well-formed", ok);
            return ok;
        }

        private static bool Test_SaveAndReloadCoins()
        {
            SaveManager.CurrentData.coins = 1234;
            SaveManager.Save();
            SaveManager.Load();
            bool ok = SaveManager.CurrentData.coins == 1234;
            LogResult("Coins persist after Save/Load", ok);
            return ok;
        }

        private static bool Test_RecoverFromCorruptedFile()
        {
            string path = System.IO.Path.Combine(Application.persistentDataPath, "foot_nail_save.json");
            System.IO.File.WriteAllText(path, "{ this is not valid json ");
            SaveManager.Load();
            bool ok = SaveManager.CurrentData != null && SaveManager.CurrentData.saveVersion >= 1;
            LogResult("Recovers from corrupted JSON", ok);
            return ok;
        }

        private static bool Test_DeleteSaveRecreatesDefault()
        {
            SaveManager.CurrentData.coins = 999;
            SaveManager.Save();
            SaveManager.DeleteSave();
            bool ok = SaveManager.CurrentData.coins == 0;
            LogResult("DeleteSave recreates default data", ok);
            return ok;
        }

        private static bool Test_CustomerProgressNoDuplicate()
        {
            SaveManager.DeleteSave();
            CustomerProgressData first = SaveManager.GetOrCreateCustomerProgress("customer_001_mina");
            CustomerProgressData second = SaveManager.GetOrCreateCustomerProgress("customer_001_mina");
            bool ok = ReferenceEquals(first, second) && SaveManager.CurrentData.customerProgress.Count == 1;
            LogResult("GetOrCreateCustomerProgress does not duplicate", ok);
            return ok;
        }

        private static bool Test_NailDesignNoDuplicate()
        {
            SaveManager.DeleteSave();
            SaveManager.CurrentData.unlockedNailDesignIds.Add("nail_korea_flower");
            SaveManager.CurrentData.unlockedNailDesignIds.Add("nail_korea_flower");
            SaveManager.Save();
            SaveManager.Load();

            int count = 0;
            foreach (string id in SaveManager.CurrentData.unlockedNailDesignIds)
            {
                if (id == "nail_korea_flower")
                    count++;
            }

            bool ok = count == 1;
            LogResult("Duplicate unlockedNailDesignIds are removed on load", ok);
            return ok;
        }

        private static bool Test_GameSessionClearAll()
        {
            GameSession.SetSelectedStageIndex(3);
            GameSession.ClearAll();
            bool ok = GameSession.SelectedStageIndex == 0 && !GameSession.HasValidInGameSelection();
            LogResult("GameSession.ClearAll resets selection state", ok);
            return ok;
        }

        private static bool Test_SceneLoaderBlocksInvalidInGame()
        {
            GameSession.ClearAll();
            bool before = SceneLoader.IsLoading;
            SceneLoader.LoadInGame();
            bool ok = !before && !SceneLoader.IsLoading;
            LogResult("SceneLoader blocks InGame load without a valid selection", ok);
            return ok;
        }

        private static void LogResult(string testName, bool ok)
        {
            if (ok)
                Debug.Log("[SaveManagerTestMenu] PASS - " + testName);
            else
                Debug.LogError("[SaveManagerTestMenu] FAIL - " + testName);
        }
    }
}
