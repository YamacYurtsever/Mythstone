using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStarScore : MonoBehaviour
{
    public List<int> star2TargetScores;
    public List<int> star3TargetScores;
    
    //Singleton
    public static LevelStarScore Instance { get; private set; }

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
    }
}
