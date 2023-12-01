using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public bool paused = false;

    JackGenerator jackGenerator;

    private void Awake()
    {
        jackGenerator = GameObject.FindGameObjectWithTag("Jack Generator").GetComponent<JackGenerator>();
    }

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
