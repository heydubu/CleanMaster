using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FootNailGame
{
    public static class SceneLoader
    {
        public static bool IsLoading { get; private set; }

        public static void LoadMain()
        {
            LoadScene(SceneNames.Main);
        }

        public static void LoadStage()
        {
            LoadScene(SceneNames.Stage);
        }

        public static void LoadInGame()
        {
            if (!GameSession.HasValidInGameSelection())
            {
                Debug.LogError("[SceneLoader] Cannot load InGame: GameSession has no valid stage group / customer selection.");
                return;
            }

            LoadScene(SceneNames.InGame);
        }

        public static void LoadScene(string sceneName)
        {
            if (IsLoading)
            {
                Debug.LogWarning("[SceneLoader] Ignored LoadScene('" + sceneName + "'), a scene transition is already in progress.");
                return;
            }

            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("[SceneLoader] Cannot load a scene with an empty name.");
                return;
            }

            if (!IsValidSceneName(sceneName))
            {
                Debug.LogError("[SceneLoader] '" + sceneName + "' is not registered in Build Settings.");
                return;
            }

            IsLoading = true;

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            if (operation == null)
            {
                Debug.LogError("[SceneLoader] SceneManager.LoadSceneAsync('" + sceneName + "') failed to start.");
                IsLoading = false;
                return;
            }

            operation.completed += OnSceneLoadCompleted;
        }

        private static void OnSceneLoadCompleted(AsyncOperation operation)
        {
            operation.completed -= OnSceneLoadCompleted;
            IsLoading = false;
        }

        private static bool IsValidSceneName(string sceneName)
        {
            int count = SceneManager.sceneCountInBuildSettings;
            for (int i = 0; i < count; i++)
            {
                string path = SceneUtility.GetScenePathByBuildIndex(i);
                string name = Path.GetFileNameWithoutExtension(path);
                if (name == sceneName)
                    return true;
            }

            return false;
        }
    }
}
