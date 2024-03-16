using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    void Awake(){
        if(instance == null)
            instance = this;
        else{
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        
        foreach(Sound sound in sounds){
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    public void Play(string soundName){
        // print("playing clip " + soundName);
        Sound sound = Array.Find(sounds, sound => sound.name == soundName);
        if(sound == null){
            print("no sound found for name " + soundName);
            return;
        }
        sound.source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
