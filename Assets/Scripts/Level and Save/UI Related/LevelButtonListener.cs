using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonListener : MonoBehaviour
{
    private Transform levelButtonStorage;
    private SceneLoader sceneLoader;
    private LevelsUnlock levelsUnlock;
    private Button levelButton;
    private int levelOrder = 0;             //1st level level order is 1

    private void Awake()
    {
        levelButtonStorage = transform.parent;
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();
        levelsUnlock = GameObject.FindGameObjectWithTag("LevelsUnlock").GetComponent<LevelsUnlock>();
        levelButton = GetComponent<Button>();

        int childCount = levelButtonStorage.childCount;
        for (int i = 0; i < childCount; i++)
        {
            if (levelButtonStorage.GetChild(i) == transform)
            {
                levelOrder = i + 1;
            }
        }

        levelButton.onClick.AddListener(GoToLevel);
    }

    public void GoToLevel()
    {
        sceneLoader.LoadScene(levelsUnlock.levelScenesStartIndex + levelOrder - 1);
    }
}
