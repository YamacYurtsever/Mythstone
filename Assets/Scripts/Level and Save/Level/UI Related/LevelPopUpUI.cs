using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelPopUpUI : MonoBehaviour
{
    public int star2TargetScore = 0;
    public int star3TargetScore = 0;
    public int levelNumber;     //1st level number equal to 1
    public Button playButton;
    public Button backButton;
    public TextMeshProUGUI levelTitle;
    public List<Transform> starQuestTransforms;
    public Image levelTrophy;

    [Header("Star Sprites")]
    public Sprite starFullSprite;
    public Sprite starEmptySprite;

    private SceneLoader sceneLoader;
    private TextMeshProUGUI star2Text;
    private TextMeshProUGUI star3Text;
    private string star2String1 = "Star Score: ";
    private string star2String2 = "Your Score: ";

    void Start()
    {
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();
        star2Text = starQuestTransforms[1].GetChild(1).GetComponent<TextMeshProUGUI>();
        star3Text = starQuestTransforms[2].GetChild(1).GetComponent<TextMeshProUGUI>();
        WritePlayerScoreToStar2();
        WritePlayerScoreToStar3();

        // Add Listener to Play and Back Buttons so Play Button knows the correct scene index and to Back Button to stay organized
        playButton.onClick.AddListener(GoToLevel);
        backButton.onClick.AddListener(EnableScrollandBackButtonAgainandDestroy);

        // Set name of level of pop up
        levelTitle.text = "Level " + levelNumber;

        // Set stars
        SetStars();

        // Sets trophy active if all 3 stars open
        CheckTrophy();
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

    public void CheckTrophy()
    {
        PlayerData data = SaveSystem.LoadDataOfPlayer();
        List<int> levelStars = data.starsForLevel;
        int levelStarsNumber = levelStars[levelNumber - 1];

        if (levelStarsNumber == 7)
        {
            levelTrophy.enabled = true;
        }
        else levelTrophy.enabled = false;
    }

    public void GoToLevel()
    {
        sceneLoader.LoadScene(SaveSingleton.Instance.levelScenesStartIndex + levelNumber - 1);
    }

    public void EnableScrollandBackButtonAgainandDestroy()
    {
        //enable scroll so player can scroll in level menu and enable back button so player can go to main menu from level menu
        //I did this by using a huge image and a mask component
        
        //then Destroy
        Destroy(gameObject);
    }

    private void WritePlayerScoreToStar2()
    {
        PlayerData data = SaveSystem.LoadDataOfPlayer();
        if (star2TargetScore != 0)
            star2Text.text = star2String1 + star2TargetScore + "\n" + star2String2 + data.scoresForLevel[levelNumber - 1];
        else
            star2Text.text = star2String1 + "XXXX\n" + star2String2 + data.scoresForLevel[levelNumber - 1];
    }

    private void WritePlayerScoreToStar3()
    {
        PlayerData data = SaveSystem.LoadDataOfPlayer();
        if (star3TargetScore != 0)
            star3Text.text = star2String1 + star3TargetScore + "\n" + star2String2 + data.scoresForLevel[levelNumber - 1];
        else
            star3Text.text = star2String1 + "XXXX\n" + star2String2 + data.scoresForLevel[levelNumber - 1];
    }

    //Puts yellow filled star that are shown on the bottom of each level
    private void OpenStar(int starOrder)
    {
        starQuestTransforms[starOrder].GetChild(0).GetComponent<Image>().sprite = starFullSprite;
    }

    //Puts empty star that are shown on the bottom of each level
    private void CloseStar(int starOrder)
    {
        starQuestTransforms[starOrder].GetChild(0).GetComponent<Image>().sprite = starEmptySprite;
    }
}
