using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class frm_main : frmbase
{
    public Button plus, subs, mix, setup;
    private void Awake()
    {
        Main.RegistEvent("gamebegin", (object parm) =>
        {
            show();
            return null;
        });
        plus.onClick.AddListener(() =>
        {
            Main.DispEvent("event_plus");
        });
        subs.onClick.AddListener(() =>
        {
            Main.DispEvent("event_subs");
        });
        mix.onClick.AddListener(() =>
        {
            Main.DispEvent("event_mix");
        });
        setup.onClick.AddListener(() =>
        {
            Main.DispEvent("event_setup");
        });

        Main.RegistEvent("event_plus", (x) => {
            this.hide();
            return null;
        });
        Main.RegistEvent("event_subs", (x) => {
            this.hide();
            return null;
        });
        Main.RegistEvent("event_mix", (x) => {
            this.hide();
            return null;
        });
        Main.RegistEvent("event_setup", (x) => {
            this.hide();
            return null;
        });
        Main.RegistEvent("event_back", (x) => {
            this.show();
            return null;
        });

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
