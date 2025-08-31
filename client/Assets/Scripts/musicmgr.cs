using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicmgr : MonoBehaviour
{
    public AudioClip main, play,other,win,faild;
    public AudioSource source
    {
        get
        {
            return GetComponent<AudioSource>();
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        Main.RegistEvent("gamebegin", (x) =>
        {
            source.clip = main;
            source.Play();
            return null;
        });
        Main.RegistEvent("event_play", (x) =>
        {
            source.clip = play;
            source.Play();
            return null;
        });
        Main.RegistEvent("game_win", (x) =>
        {
            source.clip = win;
            source.Play();
            return null;
        });
        Main.RegistEvent("game_lose", (x) =>
        {
            source.clip = faild;
            source.Play();
            return null;
        });
        Main.RegistEvent("event_music", (a) =>
        {
            source.mute = !bMusicEnable;
            return null;
        });
    }
    private void Start()
    {
        source.mute = !bMusicEnable;
    }
    bool bMusicEnable
    {
        get
        {
            return PlayerPrefs.GetInt("music", 1) == 1;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
