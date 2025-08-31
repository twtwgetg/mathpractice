using UnityEngine;
using UnityEditor;
using System;

public class PlayerPrefsTools : EditorWindow
{ 
    [MenuItem("Tools/PlayerPrefs/Delete All PlayerPrefs")]
    public static void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        
        Debug.Log("所有 PlayerPrefs 已删除");
    }

    [MenuItem("Tools/PlayerPrefs/Open PlayerPrefs Window")]
    public static void ShowWindow()
    {
        GetWindow<PlayerPrefsTools>("PlayerPrefs 工具");
    }

    void OnGUI()
    {
        GUILayout.Label("PlayerPrefs 工具", EditorStyles.boldLabel);

        if (GUILayout.Button("打开目录"))
        {
            OpenPlayerPrefsDirectory();
        }
        if (GUILayout.Button("删除所有 PlayerPrefs"))
        {
            DeleteAllPlayerPrefs();
        }
        
        if (GUILayout.Button("打印当前关卡"))
        {
            Debug.Log("当前关卡: " + PlayerPrefs.GetInt("level", 1));
        }
    }

    private void OpenPlayerPrefsDirectory()
    {
        //打开持久化目录
        UnityEditor.EditorUtility.OpenWithDefaultApp(Application.persistentDataPath);
    }
}