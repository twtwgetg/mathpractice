using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum enum_chengjiu
{
    first,
    addition,
    subtraction,
    liansheng,
    tiancai,
}
public class chengjiu : MonoBehaviour
{
    public enum_chengjiu type;
    GameObject check
    {
        get
        {
            return transform.Find("check").gameObject;
        }
    }
    bool complated
    {
        set
        {
            check.SetActive(value);
            transform.Find("title").GetComponent<TMPro.TextMeshProUGUI>().color = value ? Color.black : Color.gray;
            transform.Find("Image").GetComponent<Image>().color = value ? Color.white : Color.gray;
            transform.Find("dc").gameObject.SetActive(value);
        }
    }
    private void OnEnable()
    {
        if (type == enum_chengjiu.first)
        {
            complated = PlayerPrefs.HasKey(frm_practice.ACHIEVEMENT_BEGINNER);
        }
        else if(type == enum_chengjiu.addition)
        {
            complated = PlayerPrefs.HasKey(frm_practice.ACHIEVEMENT_ADDITION_MASTER); 
        }
        else if(type == enum_chengjiu.subtraction)
        {
            complated = PlayerPrefs.HasKey(frm_practice.ACHIEVEMENT_SUBTRACTION_MASTER); 
        }
        else if(type == enum_chengjiu.liansheng)
        {
            complated =  PlayerPrefs.HasKey(frm_practice.ACHIEVEMENT_STREAK_KING); 
        }
        else if(type == enum_chengjiu.tiancai)
        {
            complated = PlayerPrefs.HasKey(frm_practice.ACHIEVEMENT_MATH_GENIUS); 
        }
        else
        {
            Debug.LogError("δ֪" + type);
        }
    }
}
