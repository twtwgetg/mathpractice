using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class frm_result : frmbase
{
    public Button back, again;
    public TextMeshProUGUI source, time;
    // Start is called before the first frame update
    private void Start()
    {
        Main.RegistEvent("event_over", (x)=>
        {
            object[]par = x as object[];
            source.text = "得分：" +(int) par[0];
            time.text = $"用时：{(int)par[1]}秒";
            show();
            return null;
        });
        back.onClick.AddListener(() =>
        {
            Main.DispEvent("event_main");
            hide();
        });
        again.onClick.AddListener(() =>
        {
            Main.DispEvent("event_begin");
        });

        Main.RegistEvent("event_begin", (x) =>
        {
            hide();
            return null;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
