using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PrefabGen;

public class PrefabGeneratorEditor : EditorWindow 
{

    [MenuItem("Tools/Prefab Generator Tool")]
    private static void ShowWindow() {
        var window = GetWindow<PrefabGeneratorEditor>();
        window.titleContent = new GUIContent("Prefab Generator Tool");
        window.Show();
    }
    
    private void OnGUI() {
        if (GUILayout.Button("Generate"))
        {
            GeneratePrefab.PrefabGenerator();
        }

        if (GUILayout.Button("Check"))
        {
            PrefabCheck.CheckPrefab();
        }
    }
}

