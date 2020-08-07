using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using YamlDotNet.Serialization;
using System.Text.RegularExpressions;

namespace PrefabGen
{
    public class GeneratePrefab
    {
        private static string prefabDirectory = "Assets/Prefabs";
        private static string prefabExtension = ".prefab";
    
        //[MenuItem("Tools/Test")]
        public static void Test()
        {
            var d = JsonUtility.FromJson<Dictionary<string, int>>(AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Text/dict.json").text);
            foreach (var item in d)
            {
                Debug.Log(string.Concat("key: ", item.Key, ", value: ", item.Value));   
            }

            var dict = new Dictionary<string, int>();
            dict.Add("apple1", 1);
            dict.Add("apple2", 1);
            dict.Add("apple3", 1);
            dict.Add("apple4", 1);


            Debug.Log(JsonUtility.ToJson(Vector3.zero));
        }
        //[MenuItem("Tools/Generate prefab")]
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
                //Debug.Log("All Done.");
                // string des = string.Concat(modelPath, "/", "FantasyKingdom_Characters.readable.fbx");
                // foreach (string str in importedAssets)
                // {
                //     // Debug.Log(str); // >> "Assets/..."
                //     if (str == des){
                //         TextAsset meta = AssetDatabase.LoadAssetAtPath<TextAsset>(string.Concat(des, ".meta"));
                //         Regex attr = new Regex(@"isReadable: \d");
                //         if (attr.IsMatch(meta.text)){
                //             attr.Replace(meta.text, "isReadable: 1");
                //         } 
                //     }
                    
                // }  
                
            }
            void OnPreprocessModel()
            {
                if (assetImporter.assetPath.Contains(".readable"))
                {
                    ModelImporter modelimporter = (ModelImporter)assetImporter;
                    modelimporter.isReadable = true;
                }
            }
        }
        //private static string Src = "Assets";
        private static string Dest = "Assets/Text";

        //[MenuItem("Tools/StartStopAssetEditing")]
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
        
        //static string modelName = "FantasyKingdom_Characters";
        //static string subModelName = "SM_Chr_Fairy_01";

        //static string jsonStr = "{\"modelName\": \"FantasyKingdom_Characters\",\"subModelName\":\"SM_Chr_Fairy_01\"}";

        static bool CreateReadableModelAsset(string path, string name, string new_name)
        {
            string src = string.Concat(path, "/", name);
            string des = string.Concat(path, "/", new_name);
            if (!File.Exists(src)){
                Debug.LogError(string.Concat("Asset [", name,"] doesn't exist."));
                return false;
            }
            if (AssetDatabase.CopyAsset(src, des))
            {
                AssetDatabase.ImportAsset(des);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                // TextAsset meta = AssetDatabase.LoadAssetAtPath<TextAsset>(string.Concat(des, ".meta"));
                // Regex attr = new Regex(@"isReadable: \d");
                // if (attr.IsMatch(meta.text)){
                //     attr.Replace(meta.text, "isReadable: 1");
                // }
            }
            return false;
        }
        //[MenuItem("Tools/Testttt")]
        public static void test()
        {
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(string.Concat(modelPath, "/SM_Chr_Fairy_01_Root.prefab"));
            var comps = obj.GetComponentsInChildren<PlayableDirector>();
            foreach (var com in comps)
            {
                com.playOnAwake = true; // 2019.3.15f can't change to 'true', 'false works fine.
            }
        }
        [MenuItem("Tools/Prefab Generate")]
        public static void PrefabGenerator()
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
            RawData data = RawData.Load(jsonStr.text);

            if (data.Readable)
            {
                if (!File.Exists(string.Concat(modelPath, "/FantasyKingdom_Characters.readable.fbx")))
                {
                    CreateReadableModelAsset(modelPath, "FantasyKingdom_Characters.fbx", "FantasyKingdom_Characters.readable.fbx");
                }
            }
    
            var root = new GameObject("Root");
            root.name = data.prefabName;

            foreach (RawComponent sub in data.subs)
            {
                switch(sub.Type)
                {
                    case ComponentType.GameObject:
                        if (!string.IsNullOrEmpty(sub.SourceName))
                        {
                            var parent = GetOrCreateObject(root.transform, sub.Parent);
                            GameObject modelRef = AssetDatabase.LoadAssetAtPath<GameObject>(string.Concat(modelPath, "/", sub.SourceName, modelextension));
                            
                            GameObject model = PrefabUtility.InstantiatePrefab(modelRef) as GameObject;
                            model.transform.SetParent(parent);
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
                            model.name = sub.Name;
                        }
                        else
                        {
                            var em_parent = GetOrCreateObject(root.transform, sub.Parent);
                            GameObject empty = new GameObject(sub.Name);
                            empty.transform.parent = em_parent;
                        }

                        break;
                    case ComponentType.BoxCollider:
                        var ob_col = GetOrCreateObject(root.transform, sub.Parent);
                        var col = ob_col.gameObject.AddComponent<BoxCollider>();
                        col.transform.localPosition = sub.Params.position;
                        break;
                    case ComponentType.PlayableDirector:
                        var ob_tl = GetOrCreateObject(root.transform, sub.Parent);
                        var tl = ob_tl.gameObject.AddComponent<PlayableDirector>();
                        tl.extrapolationMode = (DirectorWrapMode)sub.Params.directorWrapMode;
                        break;
                }
            }

            string genPrefabFullName = string.Concat(prefabDirectory, "/", root.name, prefabExtension);
            Object prefabObj = PrefabUtility.SaveAsPrefabAsset(root, genPrefabFullName);
            

            GameObject.DestroyImmediate(root);
            EditorSceneManager.OpenScene(scene, OpenSceneMode.Single);
            /**/
        }

        static Transform GetOrCreateObject(Transform root, string path)
        {
            Transform res = null;
            res = root.Find(path);
            if (res == null){
                res = root;
                string[] paths = path.Split('/');
                foreach (var p in paths)
                {
                    root = res.Find(p);
                    if (root == null)
                    {
                        var tmp = new GameObject(p);
                        tmp.transform.parent = res;
                        res = tmp.transform;
                    }
                    else
                    {
                        res = root;
                    }
                }
            }

            return res;
        }

        //[MenuItem("Examples/Instantiate Selected")]
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

        class Contact
        {
            public string Name { get; set; }
            public string PhoneNumber { get; set; }

            public override string ToString() => $"name={Name}, tel={PhoneNumber}";
        }
        static string yamlInput = @"
    - Name: Oz-Ware
    - PhoneNumber: 123456789
    ";
        //[MenuItem("Tools/TestYaml")]
        static void testYamlSerializer()
        {
            var deserializer = new DeserializerBuilder().Build();

            var contacts = deserializer.Deserialize<List<Contact>>(yamlInput);
            Debug.Log(contacts[0]);
        }

        //[MenuItem("Tools/GetGameObjects")]
        static void GetGameObjects()
        {
            //Regex prefabSuffix = new Regex(@"--- !u!\d &\d+.*"); 
            Regex prefabSuffix = new Regex(@"--- !u!(?=\d+)");

            TextAsset pre = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Prefabs/SM_Chr_Fairy_01_Root.prefab");
            
            string text = System.IO.File.ReadAllText(string.Concat(Application.dataPath, "/Prefabs/SM_Chr_Fairy_01_Root.prefab"));
            //MatchCollection matches = prefabSuffix.Matches(text);
            string[] pres = prefabSuffix.Split(text);

            foreach (var m in pres)
                Debug.Log(m.ToString());
        }
    }
}