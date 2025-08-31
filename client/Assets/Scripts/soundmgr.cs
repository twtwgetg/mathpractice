using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundmgr : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource peng, click,slider;
    void Start()
    {
        Main.RegistEvent("event_lian", (parm) =>
        {
            if (bSoundEnable)
            {
                peng.Play();
            }
            
            return 1;
        });
        Main.RegistEvent("event_click", (parm) =>
        {
            if (bSoundEnable)
            {
                click.Play();
            }
            return 1;
        });
        Main.RegistEvent("event_sound", (parm) =>
        {

            return null;
        });

        Main.RegistEvent("event_slider", (parm) =>
        {
            if (bSoundEnable)
            {
                slider.Play();
            }
            return null;
        });
    }
    bool bSoundEnable
    {
        get
        {
            return PlayerPrefs.GetInt("sound", 1) == 1;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
