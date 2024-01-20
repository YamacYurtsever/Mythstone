using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXToggle : MonoBehaviour
{
    private SoundSources soundSources;

    private void Awake()
    {
        soundSources = GameObject.FindGameObjectWithTag("Sound Sources").GetComponent<SoundSources>();
    }

    public void ToggleSFX()
    {
        AudioManager.Instance.sfxMuted = !AudioManager.Instance.sfxMuted;
        SetSoundVolume();
    }

    public void SetSoundVolume()
    {
        soundSources.SetSoundSourcesVolume();
    }
}
