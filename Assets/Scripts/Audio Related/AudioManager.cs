using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    // Singleton
    public static AudioManager Instance { get; private set; }

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


    // Singleton Variables
    public int musicvol = 5;
    public int sfxvol = 5;
    public int maxvol = 10;

    public bool musicMuted = false;
    public bool sfxMuted = false;
}
