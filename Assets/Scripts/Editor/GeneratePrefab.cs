using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
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

    static string jsonStr = "{\"modelName\": \"FantasyKingdom_Characters\",\"subModelName\":\"SM_Chr_Fairy_01\"}";

    [SerializeField]
    struct Data
    {
        // excel(json) -> object(.prefab) -> yaml
        // template
        // 1. exl to json - done
        // 2. prefab(yaml) template - how
        // 如果过于灵活，会难以阅读；如果泛用性不强就没必要做这件事了
        public string modelName;
        public string subModelName;
        // sub gameobject info
            // model charactor
                // id(or just identify by name)
                // name
                // parent(the struct)
                // model ref
                // [component]
            // collider
                //...
            // model weapon
                //...
            // model other
                //...
            // timeline object
    }

    [MenuItem("Tools/LoadAssetInEditor")]
    static void LoadModelInEditor()
    {
        // all path can be load in editor script
        // Full Path: "Assets/.../Cube.prefab"
        // GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/model/TestPrefab.prefab");
        // GameObject.Instantiate(obj);
        Data data = new Data();
        data = JsonUtility.FromJson<Data>(jsonStr);

        GameObject modelRef = AssetDatabase.LoadAssetAtPath<GameObject>(string.Concat(modelPath, "/", data.modelName, modelextension));
        
        var root = new GameObject("Root");
        root.name = string.Concat(data.subModelName,"_Root");

        GameObject model = PrefabUtility.InstantiatePrefab(modelRef) as GameObject;
        model.transform.SetParent(root.transform);
        //PrefabUtility.InstantiateAttachedAsset(model); // with (clone)

        // connect prefab instance
        //PrefabUtility.ConnectGameObjectToPrefab(obj, prefab);

        // show the model by name
        foreach (Transform ob in model.transform) 
        {
            //Debug.Log(ob.name);
            if (ob.name == "Root" || ob.name == data.subModelName)
                continue;
            ob.gameObject.SetActive(false);
        }
        
        GameObject col = new GameObject("Collider");
        col.AddComponent<BoxCollider>();

        GameObject obj_Spine2 = GameObject.Find(string.Concat(root.name, data.modelName, "/Root/Hips/Spine_01/Spine_02"));
        GameObject obj_Neck = GameObject.Find(string.Concat(root.name, data.modelName, "/Root/Hips/Spine_01/Spine_02/Spine_03/Neck"));
        GameObject obj_Spine1 = GameObject.Find(string.Concat(root.name, data.modelName, "/Root/Hips/Spine_01"));


        GameObject.Instantiate(col, obj_Spine2.transform.position, Quaternion.identity, obj_Spine2.transform).name = "Spine2_Collider";
        GameObject.Instantiate(col, obj_Neck.transform.position, Quaternion.identity, obj_Neck.transform).name = "Neck_Collider";
        GameObject.Instantiate(col, obj_Spine1.transform.position, Quaternion.identity, obj_Spine1.transform).name = "Spine1_Collider";

        string genPrefabFullName = string.Concat(prefabDirectory, "/", root.name, prefabExtension);
        Object prefabObj = PrefabUtility.SaveAsPrefabAsset(root, genPrefabFullName);
        

        GameObject.DestroyImmediate(root);
        GameObject.DestroyImmediate(col);
        /**/
    }

    [MenuItem("Examples/Instantiate Selected")]
    static void InstantiatePrefab()
    {
        Selection.activeObject = PrefabUtility.InstantiatePrefab(Selection.activeObject as GameObject); 


    }

    [MenuItem("Examples/Instantiate Selected", true)]
    static bool ValidateInstantiatePrefab()
    {
        GameObject go = Selection.activeObject as GameObject;
        if (go == null)
            return false;

        return PrefabUtility.IsPartOfPrefabAsset(go);
    }
}
