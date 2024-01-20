using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSources : MonoBehaviour
{
    public List<AudioSource> musicAudioSources;

    // The value volume is set to by us in the scene
    // This value is the max volume, so if we set 0.3 as volume in scene, volume will be 0.3 ingame if player has music settings as 10/10
    private List<float> musicSourcesOriginalVolume;


    private void Awake()
    {
        musicSourcesOriginalVolume = new List<float>();
        musicSourcesOriginalVolume.Clear();
        foreach(AudioSource musicSource in musicAudioSources)
        {
            musicSourcesOriginalVolume.Add(musicSource.volume);
        }

        SetMusicSourcesVolume();
    }

    public void SetMusicSourcesVolume()
    {
        if (AudioManager.Instance.musicMuted)
        {
            for (int i = 0; i < musicAudioSources.Count; i++)
            {
                musicAudioSources[i].volume = 0;
            }
        }
        else
        {
            for (int i = 0; i < musicAudioSources.Count; i++)
            {
                musicAudioSources[i].volume = musicSourcesOriginalVolume[i] * ((float) AudioManager.Instance.musicvol / (float) AudioManager.Instance.maxvol);    // Max Vol = 10 for now
            }
        }
    }
}
