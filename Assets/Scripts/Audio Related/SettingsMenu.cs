using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider musicVolSlider;
    public Slider sfxVolSlider;

    public void SetMusicVolume()
    {
        AudioManager.Instance.musicvol = (int) musicVolSlider.value;
    }

    public void SetSoundVolume()
    {
        AudioManager.Instance.sfxvol = (int)sfxVolSlider.value;
    }
}
