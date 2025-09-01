using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class frm_yanshi : frmbase
{
    public playctrl ctrl;
    public TextMeshProUGUI source;
    public Button back;
    // Start is called before the first frame update
    private void Awake()
    {
        Main.RegistEvent("gamebegin", (object parm) =>
        {
            hide();
            return null;
        });
        Main.RegistEvent("event_backfromplay", (object parm) =>
        {
            hide();
            return null;
        });
    }
    void Start()
    {
        Main.RegistEvent("event_play", (x) =>
         {


             int[] pars = x as int[];
             if (pars[2] == pars[0] + pars[1]) {
                 source.text =  pars[0] + " + " + pars[1] + " = " + pars[2];
                 ctrl.PlayOnePlusOne(pars[0],pars[1]);
             }
             else
             {
                 source.text = pars[0] + " - " + pars[1] + " = " + pars[2];
                 ctrl.PlayOneSubOne(pars[0],pars[1]);
             }
             show();
             return null;
         });
        
        back.onClick.AddListener(() =>
        {
           
            Main.DispEvent("event_backfromplay");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
