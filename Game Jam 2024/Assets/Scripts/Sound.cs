using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string clipName;
    public AudioClip clip;
    public bool loop;

    [Range(0f,1f)]
    public float volume;

    [HideInInspector] public AudioSource soundSource;

}
