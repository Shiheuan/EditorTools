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
    public class GeneratePrefab : AssetPostprocessor
    {
        private static string prefabDirectory = "Assets/Prefabs";
        private static string prefabExtension = ".prefab";

        void OnPreprocessModel()
        {
            if (assetImporter.assetPath.Contains(".readable"))
            {
                ModelImporter modelimporter = (ModelImporter)assetImporter;
                modelimporter.isReadable = true;
            }
        }

        static string modelextension = ".fbx";

        static string modelPath = "Assets/Models";

        //[MenuItem("Tools/Prefab Generate")]
        /// param: @Type, @Config {Name, Modellist}
        /// 
        public static void PrefabGenerator(string Type, CharacterConfig config)
        {
            var lastScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            var scene = lastScene.path;
            var tempScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            //if (Type == "Poster")
            TextAsset jsonStr = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Text/Poster.json");
            RawData data = RawData.Load(jsonStr.text);
            
            //get prefab name
            GetPrefabName("Poster", config.Name);
    
            var root = new GameObject("Root");
            root.name = config.Name;

            foreach (RawComponent sub in data.subs)
            {
                switch(sub.Type)
                {
                    case ComponentType.GameObject:
                        if (!string.IsNullOrEmpty(sub.SourceName))
                        {
                            var parent = GetOrCreateObject(root.transform, sub.Parent);
                            var source = config.GetModelPath(sub.SourceName);
                            // get origin model
                            GameObject modelRef = null;

                            // only for body
                            if (data.Readable)
                            {
                                var readableSource = source.Replace(".fbx", ".readable.fbx");
                                if (!File.Exists(string.Concat(modelPath, "/", readableSource)))
                                {
                                    CreateReadableModelAsset(modelPath, source, readableSource);
                                }
                                modelRef = AssetDatabase.LoadAssetAtPath<GameObject>(string.Concat(modelPath, "/", readableSource));
                            }
                            else
                            {
                                modelRef = AssetDatabase.LoadAssetAtPath<GameObject>(string.Concat(modelPath, "/", source));
                            }
                            
                            GameObject model = PrefabUtility.InstantiatePrefab(modelRef) as GameObject;
                            model.transform.SetParent(parent);

                            foreach (Transform ob in model.transform) 
                            {
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
            if (!string.IsNullOrEmpty(scene))
            {
                EditorSceneManager.OpenScene(scene, OpenSceneMode.Single);
            }
        }

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
            }
            return false;
        }

        static void CheckReadableModel(string path)
        {

        }
        static void GetPrefabName(string Type, string Name)
        {

        }
        static void GetTemplate(string Type)
        {

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
    }
}