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


        //ResetSaveData();
        PlayerData data = SaveSystem.LoadDataOfPlayer();
        levelGamemodes = data.levelGamemodes;
        levelsUnlocked = data.levelsUnlocked;
        scoresForLevel = data.scoresForLevel;
        starsForLevel = data.starsForLevel;
    }


    public const int levelNumber = 30;
    public int levelScenesStartIndex = 1;       //constant but couldnt say const because needed to reference in another script
    public List<bool> levelsUnlocked;
    public List<int> scoresForLevel;
    public List<int> starsForLevel;       //each element belongs to a level, 0 being no stars 1 being first 2 being second, 4 being last star,
                                          //0 to 7 to represent 8 possibilites of combinations of 3 stars.
    public enum Gamemode
    {
        Jack,
        Timed,
        Gem
    }
    
    public List<Gamemode> levelGamemodes;
    [SerializeField] private List<Gamemode> levelGamemodesManual;

    private void ResetSaveData()
    {
        InitializeSaveData();
        SaveData();
    }

    public void InitializeSaveData()
    {
        levelsUnlocked.Clear();
        scoresForLevel.Clear();
        starsForLevel.Clear();
        levelGamemodes.Clear();

        //Initialize LevelSaveData
        for (int i = 0; i < levelNumber; i++)
        {
            levelsUnlocked.Add(false);
            scoresForLevel.Add(0);
            starsForLevel.Add(0);
            levelGamemodes.Add(levelGamemodesManual[i]);
        }
        levelsUnlocked[0] = true;
        
        //Extra data (for me)
        /*levelsUnlocked[1] = true;
        scoresForLevel[0] = 1000;
        starsForLevel[0] = 1;*/
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
