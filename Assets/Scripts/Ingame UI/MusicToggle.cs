using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicToggle : MonoBehaviour
{
    private MusicSources musicSources;

    private void Awake()
    {
        musicSources = GameObject.FindGameObjectWithTag("Music Sources").GetComponent<MusicSources>();
    }

    public void ToggleMusic()
    {
        AudioManager.Instance.musicMuted = !AudioManager.Instance.musicMuted;
        SetMusicVolume();
    }

    public void SetMusicVolume()
    {
        musicSources.SetMusicSourcesVolume();
    }
}
