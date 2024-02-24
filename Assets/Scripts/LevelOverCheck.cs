using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOverCheck : MonoBehaviour
{
    public float beforeWinDelay = 1.5f;
    public GameObject levelEndScreenPrefab;

    private Transform gemStorage;
    private GemGenerator gemGenerator;
    private SceneLoader sceneLoader;
    private Score scoreScript;
    private GameModeManager gameModeManager;


    private void Awake()
    {
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();
        gemStorage = GameObject.FindGameObjectWithTag("Gem Storage").transform;
        gemGenerator = GameObject.FindGameObjectWithTag("Gem Generator").GetComponent<GemGenerator>();
        scoreScript = GameObject.FindGameObjectWithTag("Score Text").GetComponent<Score>();
        gameModeManager = GameObject.FindGameObjectWithTag("Game Mode Manager").GetComponent<GameModeManager>();
    }

    bool levelEndScreenOpened = false;
    private void Update()
    {
        if ((CheckIfNoGems() && gameModeManager.gemsLeft ||
            gameModeManager.jacksLeft && gameModeManager.jackCount <= 0 ||
            gameModeManager.timeLeft && ((int)(gameModeManager.timeLeftLimit - (Time.time - gameModeManager.startTime)) <= 0)) && !levelEndScreenOpened)
            StartCoroutine(WinAfterWait());

        else if ((gameModeManager.gemsLeft && gameModeManager.jackCount == 0) && !levelEndScreenOpened)
            StartCoroutine(LoseAfterWait());
    }

    public bool CheckIfNoGems()
    {
        if (gemStorage.childCount == 0)
        {
            if (gemGenerator.CheckIfMoreRowsComing() && gameModeManager.timeLeft == false)
                gemGenerator.MoveAndGenerateRowsTrigger();
            else
                return true;
        }
        return false;
    }

    private IEnumerator WinAfterWait()
    {
        levelEndScreenOpened = true;
        yield return new WaitForSeconds(beforeWinDelay);
        WinLevel();
    }

    private IEnumerator LoseAfterWait()
    {
        levelEndScreenOpened = true;
        yield return new WaitForSeconds(beforeWinDelay + 0.5f);
        sceneLoader.LoadStartScene();
    }

    public void WinLevel()
    {
        UnlockNextLevelandSaveLevelandScore();
        // gamemode time 0
        Instantiate(levelEndScreenPrefab);
    }

    private void UnlockNextLevelandSaveLevelandScore()
    {
        // Unlock next level
        int currentSceneIndex = sceneLoader.GetCurrentSceneIndex();
        int firstLevelSceneIndex = SaveSingleton.Instance.levelScenesStartIndex;
        int levelNumber = currentSceneIndex - firstLevelSceneIndex;     // index form (1st level is 0)
        int targetLevel = levelNumber + 1;      // target level means next level (so, used for unlocking NEXT level)

        SaveSingleton.Instance.levelsUnlocked[targetLevel] = true;

        // Save high score
        int lastLevelScore = scoreScript.score;
        SaveScoreIfHighScore(levelNumber, lastLevelScore);

        // Open first star
        OpenFirstStarifClosed(levelNumber);

        // Open second star
        OpenSecondStarIfScoreEnough(levelNumber, lastLevelScore);

        // Open third star, for now this also works with score
        OpenThirdStarIfScoreEnough(levelNumber, lastLevelScore);

        // Save
        SaveSingleton.Instance.SaveData();
    }


    private void OpenFirstStarifClosed(int levelNumber)
    {
        int starNumber1 = SaveSingleton.Instance.starsForLevel[levelNumber];
        if (starNumber1 == 0 || starNumber1 == 2 || starNumber1 == 4 || starNumber1 == 6)
        {
            // Opens first star if closed
            SaveSingleton.Instance.starsForLevel[levelNumber] = starNumber1 + 1;
        }
    }

    private static void OpenSecondStarIfScoreEnough(int levelNumber, int lastLevelScore)
    {
        int starNumber2 = SaveSingleton.Instance.starsForLevel[levelNumber];
        if (lastLevelScore > LevelStarScore.Instance.star2TargetScores[levelNumber] && (starNumber2 == 0 || starNumber2 == 1 || starNumber2 == 4 || starNumber2 == 5))
        {
            // Opens second star if closed
            SaveSingleton.Instance.starsForLevel[levelNumber] = starNumber2 + 2;
        }
    }

    private static void OpenThirdStarIfScoreEnough(int levelNumber, int lastLevelScore)
    {
        int starNumber3 = SaveSingleton.Instance.starsForLevel[levelNumber];
        if (lastLevelScore > LevelStarScore.Instance.star3TargetScores[levelNumber] && (starNumber3 == 0 || starNumber3 == 1 || starNumber3 == 2 || starNumber3 == 3))
        {
            // Opens third star if closed
            SaveSingleton.Instance.starsForLevel[levelNumber] = starNumber3 + 4;
        }
    }

    private void SaveScoreIfHighScore(int levelNumber, int lastLevelScore)
    {
        if (lastLevelScore > SaveSingleton.Instance.scoresForLevel[levelNumber])
        {
            SaveSingleton.Instance.scoresForLevel[levelNumber] = scoreScript.score;
            Debug.Log("New high score for level " + (levelNumber + 1) + ": " + lastLevelScore);
        }
    }
}
