using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public List<bool> levelsUnlocked;
    public List<int> scoresForLevel;
    public List<int> starsForLevel;


    public List<SaveSingleton.Gamemode> levelGamemodes;

    public PlayerData(SaveSingleton saveSingleton)
    {
        levelsUnlocked = saveSingleton.levelsUnlocked;
        scoresForLevel = saveSingleton.scoresForLevel;
        starsForLevel = saveSingleton.starsForLevel;
        levelGamemodes = saveSingleton.levelGamemodes;
    }
}