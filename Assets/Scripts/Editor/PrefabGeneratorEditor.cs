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

    Vector2 scrollPosition;

    private void OnGUI() {
        // check if prefab already exist
        // TextAsset jsonStr = AssetDatabase.LoadAssetAtPath<TextAsset>(string.Concat(configPath,"/Poster.json"));
        // RawData data = RawData.Load(jsonStr.text);
        GUILayout.BeginHorizontal();
        
        // if (File.Exists(string.Concat(testPrefabPath, "/", data.prefabName, ".prefab")))
        // {
        //     if (GUILayout.Button("Check"))
        //     {
        //         PrefabCheck.CheckPrefab();
        //     }
        // }
        // else
        // {
            if (GUILayout.Button("Generate"))
            {
                var config = gameConfig.LoadConfig();
                GeneratePrefab.PrefabGenerator("Poster", config["1"]);
            }
        // }

        //GUILayout.EndArea();
        GUILayout.EndHorizontal();

    }
}

