using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class extends 
{
    // Start is called before the first frame update
    public static void Clear(this Transform trans)
    {
        while (trans.childCount > 0)
        {
            var c = trans.GetChild(0);
            c.transform.SetParent(null);
            if (Application.isPlaying)
            {
                GameObject.Destroy(c.gameObject);
            }
            else
            {
                GameObject.DestroyImmediate(c.gameObject);
            }
        }
    }
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        var c = go.GetComponent<T>();
        if (c == null)
        {
            c = go.AddComponent<T>();
        }
        return c;
    }
    public static string ReplaceAll(this string str,string p1,string p2)
    {
        string ret = str.Replace(p1, p2);
        while (ret.IndexOf(p1) > -1)
        {
            ret = ret.Replace(p1, p2);
        }
        return ret;
    }
}
