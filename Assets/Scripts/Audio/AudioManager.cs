using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Texture2D cursor;
    public Camera cam;
    public GameObject sfxContainer, musicContainer;
    public Sound[] sounds;
    public Sound[] music;
    // public Sound[] panelSounds;
    public static AudioManager instance;
    AudioSource currentMusicSource;
    void Awake(){
        if(instance == null)
            instance = this;
        else{
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        cam = FindObjectOfType<Camera>();
        if(cam != null){
        }
        
        foreach(Sound sound in sounds){
            sound.source = sfxContainer.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
        foreach(Sound sound in music){
            sound.source = musicContainer.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    public void PlaySFX(string soundName){
        // print("playing clip " + soundName);
        Sound sound = Array.Find(sounds, sound => sound.name == soundName);
        if(sound == null){
            // print("no sound found for name " + soundName);
            return;
        }
        sound.source.Play();
    }
    
    public void PlayMusic(string musicName){
        Sound sound = Array.Find(music, sound => sound.name == musicName);
        if(sound == null){
            // print("no music found for name " + musicName);
            return;
        }
        if(currentMusicSource != null)
            currentMusicSource.Pause();
        currentMusicSource = sound.source;
        currentMusicSource.Play();
    }

    public void StopMusic(){
        if(currentMusicSource != null)
            currentMusicSource.Stop();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
