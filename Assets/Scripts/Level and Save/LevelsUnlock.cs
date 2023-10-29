using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelsUnlock : MonoBehaviour
{
    public GameObject levelButtonStorage;
    public static int levelNumber = 4;
    public bool[] levelsUnlocked = new bool[levelNumber];
    public int levelScenesStartIndex = 1;
    
    void Awake()
    {
        LoadLevels();
    }

    public void LoadLevels()
    {
        PlayerData data = SaveSystem.LoadDataOfPlayer();
        levelsUnlocked = data.levelsUnlocked;

        //make locked levels non-interactable
        for (int i = 0; i < levelButtonStorage.transform.childCount; i++)
        {
            if (!levelsUnlocked[i])
            {
                levelButtonStorage.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }
    }
}
