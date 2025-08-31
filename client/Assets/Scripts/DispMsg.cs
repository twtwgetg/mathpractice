using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DispMsg : MonoBehaviour
{
    public string msg = "event_";
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Main.DispEvent(msg);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
