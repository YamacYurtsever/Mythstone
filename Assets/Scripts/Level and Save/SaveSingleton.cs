using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSingleton : MonoBehaviour
{
    //Singleton
    public static SaveSingleton Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
        PlayerData data = SaveSystem.LoadDataOfPlayer();
        levelsUnlocked = data.levelsUnlocked;
    }

    public bool[] levelsUnlocked = new bool[LevelsUnlock.levelNumber];
    public int[] scoresForLevel = new int[LevelsUnlock.levelNumber];

    public void InitializeSaveData()
    {
        //Initialize LevelSaveData
        for (int i = 0; i < levelsUnlocked.Length; i++)
        {
            levelsUnlocked[i] = false;
            scoresForLevel[i] = 0;
        }
        levelsUnlocked[0] = true;
    }

    public void LoadData()
    {
        PlayerData data = SaveSystem.LoadDataOfPlayer();
        levelsUnlocked = data.levelsUnlocked;
    }

    public void SaveData()
    {
        SaveSystem.SaveDataOfPlayer(this);
    }

    public void ResetScoreData(int levelNumber)
    {
        scoresForLevel[levelNumber - 1] = 0;
        SaveData();
    }
}
