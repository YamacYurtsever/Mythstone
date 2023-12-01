using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonListener : MonoBehaviour
{
    public GameObject levelPopUp;

    private Transform levelsMenu;
    private Transform levelButtonStorage;
    private Button levelButton;
    private int levelOrder = 0;             //1st level level order is 1
    private LevelStarScore levelStarScore;
    
    private void Awake()
    {
        levelButtonStorage = transform.parent;
        levelsMenu = GameObject.FindGameObjectWithTag("Levels Menu").transform;
        levelStarScore = GameObject.FindGameObjectWithTag("LevelsStarScore").GetComponent<LevelStarScore>();
        levelButton = GetComponent<Button>();

        int childCount = levelButtonStorage.childCount;
        for (int i = 0; i < childCount; i++)
        {
            if (levelButtonStorage.GetChild(i) == transform)
            {
                levelOrder = i + 1;
            }
        }

        levelButton.onClick.AddListener(OpenPopUp);
    }

    public void OpenPopUp()
    {
        GameObject newLevelPopUp = Instantiate(levelPopUp, levelsMenu);
        LevelPopUpUI levelPopUpScript = newLevelPopUp.GetComponent<LevelPopUpUI>();
        levelPopUpScript.levelNumber = levelOrder;
        levelPopUpScript.star2TargetScore = levelStarScore.star2TargetScores[levelOrder - 1];
        levelPopUpScript.star3TargetScore = levelStarScore.star3TargetScores[levelOrder - 1];
    }
}
