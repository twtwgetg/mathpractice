using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class frm_setup : frmbase
{
    public Button reset;
    // Start is called before the first frame update
    void Start()
    {
        Main.RegistEvent("event_setup", (x) =>
        {
            show();
            return null;
        });
        Main.RegistEvent("gamebegin", (x) =>
        {
            hide();
            return null;
        });
        Main.RegistEvent("event_mix", (x) =>
        {
            hide();
            return null;
        });
        Main.RegistEvent("event_chengjiu", (x) =>
        {
            hide();
            return null;
        });
        reset.onClick.AddListener(() =>
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
