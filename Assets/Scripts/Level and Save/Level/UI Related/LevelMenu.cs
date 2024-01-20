using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMenu : MonoBehaviour
{
    public LevelsUnlock levelUnlockScript;

    private void OnEnable()
    {
        SaveSingleton.Instance.LoadData();
        levelUnlockScript.LoadLevels();
    }
}
