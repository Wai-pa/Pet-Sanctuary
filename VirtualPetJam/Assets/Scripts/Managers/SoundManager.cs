using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public Dictionary<string, AudioSource> sounds = new Dictionary<string, AudioSource>();
    [Tooltip("Keys to store and access sounds")]
    public List<string> keys = new List<string>();
    [Tooltip("The sounds those keys have access to")]
    public List<AudioSource> values = new List<AudioSource>();

    public static SoundManager instance = null;

    void Awake(){
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }

        int i = 0;
        foreach(string key in keys){
            this.sounds.Add(key, values[i]);
            i++;
        }

    }
    public void playSound(string sound){
        this.sounds[sound].PlayDelayed(0);
    }
}
