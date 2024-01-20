using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioVolSlider : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Slider>().maxValue = AudioManager.Instance.maxvol;
        GetComponent<Slider>().value = AudioManager.Instance.sfxvol;
    }
}
