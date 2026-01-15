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
    public AudioSource noteAudioSource;

    public void playSfx(string sfxName)
    {
        foreach (var item in sfxSound.Where(item => item.name == sfxName))
        {
            sfxAudioSource.clip = item.audio;
        }
        sfxAudioSource.Play();
    }
    public void playNotes(string noteName)
    {
        foreach (var item in sfxSound.Where(item => item.name == noteName))
        {
            noteAudioSource.clip = item.audio;
        }
        noteAudioSource.Play();
    }
    public void soundOff()
    {
        noteAudioSource.mute = true;
        sfxAudioSource.mute = true;
    }
    public void soundOn()
    {
        noteAudioSource.mute = false;
        sfxAudioSource.mute = false;
    }
}
