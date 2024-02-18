using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    public AudioSource source;

    public static AudioManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }




        
    }


    private void Start()
    {
        PlaySoundOneShot("Breath");
        PlaySoundOneShot("Theme");
        

    }



    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, x => x.clipName == name);
        if (s == null)
        {
            Debug.Log("Sound not found");
        }

        else
        {
            source = gameObject.AddComponent<AudioSource>();
            source.clip = s.clip;
            source.volume = s.volume;
            source.loop = s.loop;
            source.Play();
        }

    }

    public void PlaySoundOneShot(string name)
    {
        Sound s = Array.Find(sounds, x => x.clipName == name);
       
        if (s == null)
        {
            Debug.Log("Sound not found");
        }


        else
        {
            if (s.soundSource == null)
            {
                s.soundSource = gameObject.AddComponent<AudioSource>();
            }

            if (!s.soundSource.isPlaying)
            {
                s.soundSource.clip = s.clip;
                s.soundSource.volume = s.volume;
                s.soundSource.loop = s.loop;
                s.soundSource.Play();
            }
        }

    }

    public void StopSound(string name)
    {
        Sound s = Array.Find(sounds, x => x.clipName == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }


        else
        {

            if (s.soundSource == null)
            {
                s.soundSource = gameObject.AddComponent<AudioSource>();
            }

            if (s.soundSource.isPlaying)
            {
                s.soundSource.clip = s.clip;
                s.soundSource.Stop();
            }
        }

    }

    public void StopAllAudio()
    {
        foreach (Sound s in sounds)
        {
            if (s.soundSource != null && s.soundSource.isPlaying)
            {
                s.soundSource.Stop();
            }
        }
    }





}
