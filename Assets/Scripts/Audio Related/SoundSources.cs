using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSources : MonoBehaviour
{
    public List<AudioSource> soundAudioSources;

    // The value volume is set to by us in the scene
    // This value is the max volume, so if we set 0.3 as volume in scene, volume will be 0.3 ingame if player has music settings as 10/10
    private List<float> soundSourcesOriginalVolume;


    private void Awake()
    {
        soundSourcesOriginalVolume = new List<float>();
        soundSourcesOriginalVolume.Clear();
        foreach (AudioSource soundSource in soundAudioSources)
        {
            soundSourcesOriginalVolume.Add(soundSource.volume);
        }

        SetSoundSourcesVolume();
    }

    public void SetSoundSourcesVolume()
    {
        if (AudioManager.Instance.sfxMuted)
        {
            for (int i = 0; i < soundAudioSources.Count; i++)
            {
                soundAudioSources[i].volume = 0;
            }
        }
        else
        {
            for (int i = 0; i < soundAudioSources.Count; i++)
            {
                soundAudioSources[i].volume = soundSourcesOriginalVolume[i] * ((float) AudioManager.Instance.sfxvol / (float) AudioManager.Instance.maxvol);    // Max Vol = 10 for now
            }
        }
    }
}
