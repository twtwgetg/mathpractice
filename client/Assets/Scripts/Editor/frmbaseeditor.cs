using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(frmbase),true)]
public class frmbaseeditor : Editor
{
    // Start is called before the first frame update
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("��ʾ"))
        {
            frm.show();
        }
        if (GUILayout.Button("����"))
        {
            frm.hide();
        }
    }
    frmbase frm
    {
        get
        {
            return target as frmbase;
        }
    }
}
