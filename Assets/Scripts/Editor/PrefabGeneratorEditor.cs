using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PrefabGen;

public class PrefabGeneratorEditor : EditorWindow 
{
    static PrefabGeneratorEditor instance = null;
    [MenuItem("Tools/Prefab Generator Tool")]
    private static void ShowWindow() {
        instance = GetWindow<PrefabGeneratorEditor>();
        instance.titleContent = new GUIContent("Prefab Generator Tool");
        instance.Show();
    }

    string configPath = "Assets/Text";
    string testPrefabPath = "Assets/Prefabs";   
    
    bool toggle1 = false;
    bool toggle2 = false;
    bool toggle3 = false;
    bool toggle4 = false;
    bool toggle5 = false;
    bool toggle6 = false;
    bool toggle7 = false;
    bool toggle8 = false;
    bool toggle9 = false;
    bool toggle10 = false;
    bool toggle11 = false;

    Vector2 scrollPosition;

    private void OnGUI() {
        // check if prefab already exist
        TextAsset jsonStr = AssetDatabase.LoadAssetAtPath<TextAsset>(string.Concat(configPath,"/Poster.json"));
        RawData data = RawData.Load(jsonStr.text);
        GUILayout.BeginHorizontal();
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(100), GUILayout.Height(100));
            toggle1 = GUILayout.Toggle(toggle1, "1");
            toggle2 = GUILayout.Toggle(toggle2, "2");
            toggle3 = GUILayout.Toggle(toggle3, "3");
            toggle4 = GUILayout.Toggle(toggle4, "4");
            toggle5 = GUILayout.Toggle(toggle5, "5");
            toggle6 = GUILayout.Toggle(toggle6, "6");
            toggle7 = GUILayout.Toggle(toggle7, "7");
            toggle8 = GUILayout.Toggle(toggle8, "8");
            toggle9 = GUILayout.Toggle(toggle9, "9");
            toggle10 = GUILayout.Toggle(toggle10, "10");
            toggle11 = GUILayout.Toggle(toggle11, "11");
        GUILayout.EndScrollView();

        
        //GUILayout.BeginArea(new Rect(10, 10, 100, 100), "operation");

        
        
        if (File.Exists(string.Concat(testPrefabPath, "/", data.prefabName, ".prefab")))
        {
            if (GUILayout.Button("Check"))
            {
                PrefabCheck.CheckPrefab();
            }
        }
        else
        {
            if (GUILayout.Button("Generate"))
            {
                GeneratePrefab.PrefabGenerator();
            }
        }

        //GUILayout.EndArea();
        GUILayout.EndHorizontal();

    }
}

