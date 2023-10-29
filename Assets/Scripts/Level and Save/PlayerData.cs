using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public bool[] levelsUnlocked;

    public PlayerData(SaveSingleton saveSingleton)
    {
        levelsUnlocked = saveSingleton.levelsUnlocked;
    }
}
