﻿using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
 
public class GeneratePrefab
{
    private static string prefabDirectory = "Assets/Prefabs";
    private static string prefabExtension = ".prefab";
 
    [MenuItem("Tools/Generate prefab")]
    public static void Generate()
    {
        // GameObject selectedGameObject = Selection.activeGameObject;
        // string selectedAssetPath = AssetDatabase.GetAssetPath(selectedGameObject);
        // if (string.IsNullOrEmpty(selectedAssetPath))
        // {
        //     return;
        // }

        if (!Directory.Exists(prefabDirectory))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }
        // string prefabFullDirectory = string.Concat("Assets/", prefabDirectory);
        // string modelParentPath = string.Concat("Assets/Resources", prefabDirectory);
        string modelFullPath = string.Concat(prefabDirectory, "/", "model");
        if (!Directory.Exists(modelFullPath))
        {
            AssetDatabase.CreateFolder(prefabDirectory, "model");
        }
        // GameObject cloneObj = GameObject.Instantiate<GameObject>(selectedGameObject);
        GameObject Obj = new GameObject();
        Obj.name = "TestPrefab";
        string genPrefabFullName = string.Concat(modelFullPath, "/", Obj.name, prefabExtension);
 
        Debug.Log(Application.dataPath);
        //Object prefabObj = PrefabUtility.SaveAsPrefabAsset(Obj, Application.dataPath);
        Object prefabObj = PrefabUtility.SaveAsPrefabAsset(Obj, genPrefabFullName); // need '.prefab' extension.

        GameObject.DestroyImmediate(Obj);
    }

    private class AllPostprocessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            Debug.Log("All Done.");
        }
    }
    private static string Src = "Assets";
    private static string Dest = "Assets/Text";

    [MenuItem("Tools/StartStopAssetEditing")]
    static void CallAssetDatabaseAPIsBetweenStartStopAssetEditing()
    {
        try
        {
            //Place the Asset Database in a state where
            //importing is suspended for most APIs
            AssetDatabase.StartAssetEditing();
            if (!Directory.Exists(Dest))
            {
                AssetDatabase.CreateFolder("Assets", "Text");
            }
            AssetDatabase.CopyAsset("Assets/CopyAsset.txt", "Assets/Text/CopyAsset.txt");
            var error = AssetDatabase.MoveAsset("Assets/MoveAsset.txt", "Assets/Text/MoveAsset.txt"); // if Folder not exit, can't move this after Create
            Debug.Log("Error: " + error);
            // Debug Log: Parent directory is not in asset database
            // need register in assetDatabase
            
        }
        finally
        {
            //By adding a call to StopAssetEditing inside
            //a "finally" block, we ensure the AssetDatabase
            //state will be reset when leaving this function
            AssetDatabase.StopAssetEditing();
        }
    }

    static string modelextension = ".fbx";

    static string modelPath = "Assets/Models";
    
    static string modelName = "FantasyKingdom_Characters";
    static string subModelName = "SM_Chr_Fairy_01";

    [MenuItem("Tools/LoadAssetInEditor")]
    static void LoadModelInEditor()
    {
        // all path can be load in editor script
        // Full Path: "Assets/.../Cube.prefab"
        // GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/model/TestPrefab.prefab");
        // GameObject.Instantiate(obj);
        GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(string.Concat(modelPath, "/", modelName, modelextension));
        var root = new GameObject("Root");
        root.name = string.Concat(subModelName,"_Root");
        GameObject obj = GameObject.Instantiate(model, Vector3.zero, Quaternion.identity, root.transform);
        obj.name = obj.name.Replace("(Clone)",string.Empty);

        // show the model by name
        foreach (Transform ob in obj.transform) 
        {
            //Debug.Log(ob.name);
            if (ob.name == "Root" || ob.name == subModelName)
                continue;
            ob.gameObject.SetActive(false);
        }

        GameObject col = new GameObject("Collider");
        col.AddComponent<BoxCollider>();
        //Debug.Log(string.Concat("Root/", modelName, "/Root/Hips/Spine_01/Spine_02"));
        GameObject obj_Spine2 = GameObject.Find(string.Concat(root.name, modelName, "/Root/Hips/Spine_01/Spine_02"));
        GameObject obj_Neck = GameObject.Find(string.Concat(root.name, modelName, "/Root/Hips/Spine_01/Spine_02/Spine_03/Neck"));
        GameObject obj_Spine1 = GameObject.Find(string.Concat(root.name, modelName, "/Root/Hips/Spine_01"));

        
        GameObject.Instantiate(col, obj_Spine2.transform.position, Quaternion.identity, obj_Spine2.transform).name = "Spine2_Collider";
        GameObject.Instantiate(col, obj_Neck.transform.position, Quaternion.identity, obj_Neck.transform).name = "Neck_Collider";
        GameObject.Instantiate(col, obj_Spine1.transform.position, Quaternion.identity, obj_Spine1.transform).name = "Spine1_Collider";

        string genPrefabFullName = string.Concat(prefabDirectory, "/", root.name, prefabExtension);
        Object prefabObj = PrefabUtility.SaveAsPrefabAsset(root, genPrefabFullName);

        GameObject.DestroyImmediate(root);
        GameObject.DestroyImmediate(obj);
        GameObject.DestroyImmediate(col);
    }
}
