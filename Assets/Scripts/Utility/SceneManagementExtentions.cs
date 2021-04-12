using UnityEngine;
using Debug = UnityEngine.Debug;
using UnityEngine.SceneManagement;

public class SceneIndexLogger : SceneManagerAPI
{
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        SceneManagerAPI.overrideAPI = new SceneIndexLogger();
    }

    protected override int GetNumScenesInBuildSettings()
    {
        Debug.LogWarning("SceneManager.GetNumScenesInBuildSettings() called, please load scenes by path to avoid issues when scenes are reordered.");
        return base.GetNumScenesInBuildSettings();
    }

    protected override Scene GetSceneByBuildIndex(int buildIndex)
    {
        Debug.Log($"SceneManager.GetSceneByBuildIndex(buildIndex = {buildIndex}) called, please load scenes by path to avoid issues when scenes are reordered.");
        return base.GetSceneByBuildIndex(buildIndex);
    }
}