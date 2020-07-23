using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Collections;
using UnityEngine.Playables;
 
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

        // simple
        /*
        Data data = new Data();
        TextAsset jsonStr = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Text/Poster.json");
        data = JsonUtility.FromJson<Data>(jsonStr.text);
        */
        var lastScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        var scene = lastScene.path;
        var tempScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        TextAsset jsonStr = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Text/Poster.json");
        CharacterGen data = CharacterGen.Load(jsonStr.text);
 
        var root = new GameObject("Root");
        root.name = data.characterName;

        foreach (baseSub sub in data.subs)
        {
            switch(sub.Type)
            {
                case "model":
                    GameObject modelRef = AssetDatabase.LoadAssetAtPath<GameObject>(string.Concat(modelPath, "/", sub.SourceName, modelextension));
                    
                    GameObject model = PrefabUtility.InstantiatePrefab(modelRef) as GameObject;
                    model.transform.SetParent(GameObject.Find(sub.Parent).transform);
                    //PrefabUtility.InstantiateAttachedAsset(model); // with (clone)

                    // connect prefab instance
                    //PrefabUtility.ConnectGameObjectToPrefab(obj, prefab);
                    // show the model by name
                    foreach (Transform ob in model.transform) 
                    {
                        //Debug.Log(ob.name);
                        if (ob.name == "Root" || ob.name == sub.Name)
                            continue;
                        ob.gameObject.SetActive(false);
                    }
        
                    break;
                case "collider":
                    GameObject col = new GameObject("Collider");
                    col.AddComponent<BoxCollider>();
                    GameObject col_parent = GameObject.Find(sub.Parent);
                    GameObject.Instantiate(col, col_parent.transform.position, Quaternion.identity, col_parent.transform).name = sub.Name;
                    GameObject.DestroyImmediate(col);
                    break;
                case "timeline":
                    GameObject tl = new GameObject("Timeline");
                    tl.AddComponent<PlayableDirector>();
                    GameObject tl_parent = GameObject.Find(sub.Parent);
                    GameObject.Instantiate(tl, tl_parent.transform.position, Quaternion.identity, tl_parent.transform).name = sub.Name;
                    GameObject.DestroyImmediate(tl);
                    break;
                case "empty":
                    GameObject em = new GameObject("Empty");
                    GameObject em_parent = GameObject.Find(sub.Parent);
                    GameObject.Instantiate(em, em_parent.transform.position, Quaternion.identity, em_parent.transform).name = sub.Name;
                    GameObject.DestroyImmediate(em);
                    break;
            }
        }

        string genPrefabFullName = string.Concat(prefabDirectory, "/", root.name, prefabExtension);
        Object prefabObj = PrefabUtility.SaveAsPrefabAsset(root, genPrefabFullName);
        

        GameObject.DestroyImmediate(root);
        EditorSceneManager.OpenScene(scene, OpenSceneMode.Single);
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
