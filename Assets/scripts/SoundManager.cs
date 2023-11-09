using UnityEngine;
using System.Linq;
using System;

public class SoundManager : MonoBehaviour
{
    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip audio;
    }

    public static SoundManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    public Sound[] sfxSound;
    public AudioSource sfxAudioSource;

    public void playSfx(string sfxName)
    {
        foreach (var item in sfxSound.Where(item => item.name == sfxName))
        {
            sfxAudioSource.clip = item.audio;
        }
        sfxAudioSource.Play();
    }
}
