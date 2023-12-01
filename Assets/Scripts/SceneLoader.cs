using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadLoseScene()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadLastUnlockedLevelScene()
    {
        PlayerData data = SaveSystem.LoadDataOfPlayer();
        List<bool> levelsUnlocked = data.levelsUnlocked;

        int lastLevel = 0;      //level 1 last level value is 1
        for (int i = 0; i < levelsUnlocked.Count; i++)
        {
            if(levelsUnlocked[i])
            {
                lastLevel = i + 1;
            }
        }

        SceneManager.LoadScene(SaveSingleton.Instance.levelScenesStartIndex + lastLevel - 1);
    }

    public int GetCurrentSceneIndex()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        return currentSceneIndex;
    }
}