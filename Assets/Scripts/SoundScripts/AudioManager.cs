using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //make this class a singleton
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    //class needs audiosources for music and sfx
    public AudioSource musicSource;
    public AudioSource sfxSource;

    //collection of sounds
    [SerializeField] private Sound[] musicSounds, sfxSounds;

    //make this class a singleton and make it persist through scenes
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(SoundType type)
    {
        Sound s = Array.Find(musicSounds, x => x.type == type);
        if (s == null)
        {
            print("Music Not Found");
        }
        else
        {
            musicSource.clip = s.audioClip;
            musicSource.Play();

        }
    }

    public void PlaySFX(SoundType type)
    {
        Sound s = Array.Find(sfxSounds, x => x.type == type);
        if (s == null)
        {
            print("SFX Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(s.audioClip);
        }
    }

    public float FindSFXDuration(SoundType type)
    {
        Sound s = Array.Find(sfxSounds, x => x.type == type);
        if (s == null)
        {
            print("SFX Not Found");
            return 0;
        }
        else
        {
            return s.audioClip.length;
        }
    }
}
