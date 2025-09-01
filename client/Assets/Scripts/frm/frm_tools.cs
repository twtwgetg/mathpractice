using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class frm_tools : frmbase
{
    public Button main, practice, result, setup;
    private Button _curr;

    Button curr
    {
        get
        {
            return _curr;
        }
        set
        {
            if (_curr != null)
            {
                _curr.GetComponent<Image>().color = Color.white;
                _curr.transform.Find("Image").GetComponent<Image>().color = Color.gray;
                _curr.GetComponentInChildren<TextMeshProUGUI>().color = Color.gray;
            }
            _curr = value;
            if(_curr != null)
            {
                _curr.GetComponent<Image>().color =new Color(0.1568f,0.6196f,0.9607f);
                _curr.transform.Find("Image").GetComponent<Image>().color = Color.white;
                _curr.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            }
        }
    }
    private void Awake()
    {
        Main.RegistEvent("gamebegin", (object parm) =>
        {
            curr = main; 
            return null;
        });
        main.onClick.AddListener(() =>
        {
            curr = main;
            Main.DispEvent("gamebegin");
        });
        practice.onClick.AddListener(() =>
        {
            curr = practice;
            Main.DispEvent("event_mix");
        });
        result.onClick.AddListener(() =>
        {
            curr = result;
            Main.DispEvent("event_chengjiu");
        });
        setup.onClick.AddListener(() => { 
            curr = setup; 
            Main.DispEvent("event_setup");
        });
        Main.RegistEvent("event_chengjiu", (x) =>
        {
            curr = result;
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
