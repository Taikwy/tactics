using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0, 1f)]
    public float volume = .5f;
    [Range(0, 2f)]
    public float pitch = 1f;
    public bool loop;
    [HideInInspector]
    public AudioSource source;
}
