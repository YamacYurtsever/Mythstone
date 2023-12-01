using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetData : MonoBehaviour
{
    public int levelToBeReset = 0;      // for 1st level value should be 1
    public int starToBeReset = 0;       // for 1st star value should be 1


    public void ResetScoreDataforLevel()
    {
        int levelIndex = levelToBeReset - 1;

        SaveSingleton.Instance.scoresForLevel[levelIndex] = 0; 
        SaveSingleton.Instance.SaveData();
    }

    public void ResetStarDataCompletelyforLevel()
    {
        int levelIndex = levelToBeReset - 1;

        SaveSingleton.Instance.starsForLevel[levelIndex] = 0;
        SaveSingleton.Instance.SaveData();
    }

    public void ResetCertainStarDataforLevel()
    {
        int levelIndex = levelToBeReset - 1;
        int starIndex = starToBeReset - 1;

        bool[] starsOpen = { false, false, false};
        int starNumber = SaveSingleton.Instance.starsForLevel[levelIndex];

        for (int i = 2; i >= 0; i--)
        {
            int power = (int) Mathf.Pow(2, i);
            if (starNumber >= power)
            {
                starNumber -= power;
                starsOpen[i] = true;
            }
        }

        if(starsOpen[starIndex])
        {
            SaveSingleton.Instance.starsForLevel[levelIndex] -= (int) Mathf.Pow(2, starIndex);
            SaveSingleton.Instance.SaveData();
        }
    }
}
