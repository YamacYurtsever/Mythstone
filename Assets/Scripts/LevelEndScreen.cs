using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelEndScreen : MonoBehaviour
{
    public int levelNumber;     //1st level number equal to 1
    public TextMeshProUGUI title;
    public Image star1, star2, star3;
    public Button mainMenuButton, replayButton, nextLevelButton;

    [Header("Star Sprites")]
    public Sprite starFullSprite;
    public Sprite starEmptySprite;

    private SceneLoader sceneLoader;
    private void Start()
    {
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();

        // set level number
        levelNumber = sceneLoader.GetCurrentSceneIndex() - SaveSingleton.Instance.levelScenesStartIndex + 1;

        if (levelNumber < 0) Debug.LogError("Level negative");

        // If there are no next levels no next button

        title.text = "Level " + levelNumber.ToString() + " Cleared!";

        SetStars();

        // Add Listeners to Buttons
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        replayButton.onClick.AddListener(ReplayLevel);
        nextLevelButton.onClick.AddListener(GoToNextLevel);
    }

    private void SetStars()
    {
        PlayerData data = SaveSystem.LoadDataOfPlayer();
        List<int> levelStars = data.starsForLevel;
        int levelStarsNumber = levelStars[levelNumber - 1];
        for (int j = LevelsUnlock.starNumber - 1; j >= 0; j--)
        {
            CloseStar(j);
            int x = (int)Mathf.Pow(2, j);
            if (levelStarsNumber >= x)
            {
                levelStarsNumber -= x;
                OpenStar(j);
            }
        }
    }

    public void GoToMainMenu()
    {
        sceneLoader.LoadStartScene();
    }

    public void GoToNextLevel()
    {
        sceneLoader.LoadScene(sceneLoader.GetCurrentSceneIndex() + 1);
    }

    public void ReplayLevel()
    {
        sceneLoader.LoadScene(sceneLoader.GetCurrentSceneIndex());
    }

    //Puts yellow filled star that are shown on the bottom of each level
    private void OpenStar(int starOrder)
    {
        if (starOrder == 0)
        {
            star1.sprite = starFullSprite;
        }
        else if (starOrder == 1)
        {
            star2.sprite = starFullSprite;
        }
        else if (starOrder == 2)
        {
            star3.sprite = starFullSprite;
        }
    }

    //Puts empty star that are shown on the bottom of each level
    private void CloseStar(int starOrder)
    {
        if (starOrder == 0)
        {
            star1.sprite = starEmptySprite;
        }
        else if (starOrder == 1)
        {
            star2.sprite = starEmptySprite;
        }
        else if (starOrder == 2)
        {
            star3.sprite = starEmptySprite;
        }
    }
}
