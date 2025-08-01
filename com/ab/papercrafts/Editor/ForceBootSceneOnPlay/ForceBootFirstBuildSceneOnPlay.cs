using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace com.ab.papercrafts
{
    [InitializeOnLoad]
    public static class ForceBootFirstBuildSceneOnPlay
    {
        static EditorBuildSettingsScene _bootScene;

        const string EDITOR_PATH = "Tools/Boot Scene Redirect/Enable";
        const string ENABLED_KEY = "ForceBootSceneOnPlay_Enabled";

        const string LAST_OPENED_SCENE_KEY = "ForceBootSceneOnPlay_LastOpenedScene";

        static ForceBootFirstBuildSceneOnPlay() =>
            EditorApplication.playModeStateChanged += OnPlayModeChanged;

        static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (!IsEnabled())
                return;

            UpdateBootScene();

            switch (state)
            {
                case PlayModeStateChange.ExitingEditMode:
                    SaveCurrentSceneAndOpenBoot();
                    break;

                case PlayModeStateChange.EnteredEditMode:
                    RestorePreviousScene();
                    break;
            }
        }

        static void SaveCurrentSceneAndOpenBoot()
        {
            var currentScene = SceneManager.GetActiveScene();

            if (!currentScene.path.Equals(_bootScene.path))
            {
                EditorPrefs.SetString(LAST_OPENED_SCENE_KEY, currentScene.path);

                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    EditorSceneManager.OpenScene(_bootScene.path);
                else
                    EditorApplication.isPlaying = false;
            }
        }

        static void RestorePreviousScene()
        {
            string lastScenePath = EditorPrefs
                .GetString(LAST_OPENED_SCENE_KEY, "");

            if (!string.IsNullOrEmpty(lastScenePath) &&
                lastScenePath != _bootScene.path &&
                System.IO.File.Exists(lastScenePath))
                EditorSceneManager.OpenScene(lastScenePath);

            EditorPrefs.DeleteKey(LAST_OPENED_SCENE_KEY);
        }

        [MenuItem(EDITOR_PATH, true)]
        static bool ToggleEnabledValidate() => true;

        [MenuItem(EDITOR_PATH)]
        static void ToggleEnabled()
        {
            bool enabled = !IsEnabled();
            EditorPrefs.SetBool(ENABLED_KEY, enabled);
            Menu.SetChecked(EDITOR_PATH, enabled);
        }

        [InitializeOnLoadMethod]
        static void InitMenuCheck() =>
            Menu.SetChecked(EDITOR_PATH, IsEnabled());

        static bool IsEnabled() => EditorPrefs.GetBool(ENABLED_KEY, true);

        static void UpdateBootScene()
        {
            var scenes = EditorBuildSettings.scenes;

            if (scenes.Length > 0)
                _bootScene = scenes[0];
            else
                throw new InvalidOperationException(
                    $"PaperCraft::{nameof(ForceBootFirstBuildSceneOnPlay)}::UpdateBootScene: " +
                    $"Couldn't find it any scene in EditorBuildSettings. " +
                    $"Check if there is at least one added scene in Build profile scene list");
        }
    }
}