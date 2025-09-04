using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class frm_chengjiu : frmbase
{
    public Transform content;
    public TextMeshProUGUI additionAccuracyText; // 加法正确率文本
    public TextMeshProUGUI subtractionAccuracyText; // 减法正确率文本
    public TextMeshProUGUI overallAccuracyText; // 总体正确率文本
    public TextMeshProUGUI mainall;
    public TextMeshProUGUI details,lianxutianshu;
 /// <summary>
    /// 更新并显示统计数据
    /// </summary>
    void UpdateStatisticsDisplay()
    {
        // 获取统计数据
        int totalAddition = PlayerPrefs.GetInt("TotalAdditionQuestions", 0);
        int correctAddition = PlayerPrefs.GetInt("CorrectAdditionQuestions", 0);
        int totalSubtraction = PlayerPrefs.GetInt("TotalSubtractionQuestions", 0);
        int correctSubtraction = PlayerPrefs.GetInt("CorrectSubtractionQuestions", 0);
        
        // 计算正确率
        float additionAccuracy = totalAddition > 0 ? (float)correctAddition / totalAddition * 100 : 0;
        float subtractionAccuracy = totalSubtraction > 0 ? (float)correctSubtraction / totalSubtraction * 100 : 0;
        
        // 计算总体正确率
        int totalAll = totalAddition + totalSubtraction;
        int correctAll = correctAddition + correctSubtraction;
        float overallAccuracy = totalAll > 0 ? (float)correctAll / totalAll * 100 : 0;
        
        // 更新UI显示
        if (additionAccuracyText != null)
        {
            additionAccuracyText.text = $"{(int)additionAccuracy}%";//({correctAddition}/{totalAddition})";
        }
        
        if (subtractionAccuracyText != null)
        {
            subtractionAccuracyText.text = $"{(int)subtractionAccuracy}%";// ({correctSubtraction}/{totalSubtraction})";
        }
        
        if (overallAccuracyText != null)
        {
            overallAccuracyText.text = $"{(int)overallAccuracy}%";// ({correctAll}/{totalAll})";
        }
        details.text = $"已答题:{totalAddition+totalSubtraction},正确:{correctAddition+correctSubtraction}";
        mainall.text = $"{(int)overallAccuracy}%";
        Debug.Log($"显示统计数据 - 加法: {additionAccuracy:F1}%, 减法: {subtractionAccuracy:F1}%, 总体: {overallAccuracy:F1}%");


        lianxutianshu.text=PlayerPrefs.GetInt(frm_practice.CONSECUTIVE_DAYS_KEY).ToString();
    }
    protected override void OnShow()
    {
        base.OnShow();
        ShowStatistics();
    }
    /// <summary>
    /// 显示统计数据
    /// </summary>
    void ShowStatistics()
    {
        UpdateStatisticsDisplay();
    }
    private void Awake()
    {
        var x = content.GetComponentsInChildren<Image>();
        foreach(var item in x)
        {
            item.raycastTarget = false; 
        }
        var fx = content.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var item in fx)
        {
            item.raycastTarget = false;
        }
        Main.RegistEvent("event_chengjiu", (object parm) =>
        {
            show();
            return null;
        });
        Main.RegistEvent("gamebegin", (object parm) =>
        {
            hide();
            return null;
        });
        Main.RegistEvent("event_mix", (object parm) =>
        {
            hide();
            return null;
        });
    }
    // Start is called before the first frame update
    void Start()
    {
        var ls =content.GetComponentsInChildren<VerticalLayoutGroup>();
        for(int i = 0; i < ls.Length; i++)
        {
            ls[i].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
