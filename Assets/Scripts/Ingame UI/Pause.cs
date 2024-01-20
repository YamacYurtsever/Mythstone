using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public bool paused = false;


    public void PauseTime()
    {
        Time.timeScale = 0;
        paused = true;
    }

    public void ContinueTime()
    {
        Time.timeScale = 1;
        paused = false;
    }
}
