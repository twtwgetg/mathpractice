using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Tools : MonoBehaviour
{
    internal int times;
    public UnityEngine.UI.Button.ButtonClickedEvent eventx;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            eventx.Invoke();
            Main.DispEvent("event_click");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void reset()
    {
        times = 1;
        gameObject.SetActive(true);
    }
}
