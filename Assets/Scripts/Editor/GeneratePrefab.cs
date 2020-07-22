using UnityEngine;
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
        Object prefabObj = PrefabUtility.SaveAsPrefabAsset(Obj, genPrefabFullName);

        GameObject.DestroyImmediate(Obj);
    }
 
}
