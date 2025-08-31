using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelmgr : MonoBehaviour
{
    public static int level {
        get
        {
            return PlayerPrefs.GetInt("level", 1);
        }
        set
        {
            PlayerPrefs.SetInt("level", value);
        }
    }

    internal static void init()
    {
        level = 1;
    } 
    /**
     * 获取关卡时间
     * @param level 关卡
     * @return 时间
     * 关卡时间1级100秒
     * 
     */
    internal static float getTimeAll(int level)
    {
        float t = level / (float)realmaxlevel;
        return Mathf.Lerp(30, 100, t);
    }
    /**
     * 获取卡片最小值
     * */
    internal static int getMin(int level)
    {
        return level;
    }
    static int maxlevel = 50;
    public static int maxcount = 30;

    public static int realmaxlevel
    {
        get
        {
            return maxlevel-maxcount;
        }
    }
    /**
     * 获取卡片数量
     * */
    internal static int getCount(int level)
    {
        float t= level /(float)maxlevel;
        return (int)Mathf.Lerp(15,realmaxlevel,t);
    } 
    internal static int getWid(int level_playing)
    {
        float t = level_playing / (float)(realmaxlevel);
        if (t < 0.2f)
        {
            return 4;
        }
        else if (t < 0.6f)
        {
            return 6;
        }
        else
        {
            return 8;
        }
    }

    internal static int getHei(int level_playing)
    {
        float t = level_playing / (float)realmaxlevel;
        if (t < 0.2f)
        {
            return 8;
        }
        else if (t < 0.6f)
        {
            return 10;
        }
        else
        {
            return 12;
        }
    }

    internal static float getSource(int level_playing)
    {
        float t = level_playing / (float)realmaxlevel;
        return Mathf.Lerp(1000, 10000000, t);
    }
}
