using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace PrefabGen
{
    public class PrefabCheck
    {
        [MenuItem("Tools/Check Prefab")]
        public static void CheckPrefab()
        {
            var lastScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            var scene = lastScene.path;
            var tempScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var prefab = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/SM_Chr_Fairy_01_Root.prefab")) as GameObject;
            
            var config = RawData.Load(AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Text/Poster.json").text);
            Check(config, prefab);

            if (!string.IsNullOrEmpty(scene))
                EditorSceneManager.OpenScene(scene, OpenSceneMode.Single);
        }

        //[MenuItem("Tools/GameObject To Json")]
        public static void textToJson()
        {
            var prefab = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/SM_Chr_Fairy_01_Root.prefab")) as GameObject;
            var text = EditorJsonUtility.ToJson(prefab);
            AddTxtTextByFileStream(text);
            /*
             {
                 "GameObject":{
                     "serializedVersion":"6",
                     "m_Layer":0,
                     "m_Name":"SM_Chr_Fairy_01_Root",
                     "m_TagString":"Untagged",
                     "m_Icon":{"instanceID":0},
                     "m_NavMeshLayer":0,
                     "m_StaticEditorFlags":0,
                     "m_IsActive":true
                 }
             }
             */
        }
        public static void AddTxtTextByFileStream(string txtText)
        {
            string path = Application.dataPath + "/Text/MyTxtByFileStream.txt";
            // 文件流创建一个文本文件
            FileStream file = new FileStream(path, FileMode.Create);
            //得到字符串的UTF8 数据流
            byte[] bts = System.Text.Encoding.UTF8.GetBytes(txtText);
            // 文件写入数据流
            file.Write(bts, 0, bts.Length);
            if (file != null)
            {
                //清空缓存
                file.Flush();
                // 关闭流
                file.Close();
                //销毁资源
                file.Dispose();
            }
        }
        // TODO: not implement for every json config.
        // TODO: output all error
        public static void Check(RawData config, GameObject obj)
        {
            List<string> error = new List<string>();
            var root = (GameObject)CheckRoot(config.characterName, ref error);

            foreach (RawComponent sub in config.subs)
            {
                var com = CheckObject(root.transform, sub.Parent, sub.Name, ref error) as GameObject;
                //if (com != null)
                    //CheckObjectParent(com.transform, sub.Parent, ref error);
            }
            
            if (error.Count == 0)
            {
                Debug.Log(string.Concat("Prefab [", config.characterName, "] is all good."));
            }
            else
            {
                foreach (var err in error)
                {
                    Debug.LogError(err);
                }
            }
        }

        static Object CheckRoot(string name, ref List<string> error)
        {
            var root = GameObject.Find(name);
            if (root == null)
            {
                error.Add(string.Concat("No Prefab named ", name));
                return null;
            }
            return root;
        }

        static Object CheckObject(Transform root, string parent, string name, ref List<string> error)
        {
            var p = root.Find(parent);
            if (p == null)
            {
                error.Add(string.Concat("Can't find Object ", parent, "."));
                return null;
            }
            var obj = p.Find(name);
            if (obj == null)
            {
                error.Add(string.Concat("Prefab [", p.name, "] has no component: ", name));
                return null;
            }
            return obj;
        }

        static void CheckObjectParent(Transform transform, string parent, ref List<string> error)
        {
            if (transform.parent.name == parent)
            {
                
            }
        }

        static void CheckComponent<T>(Transform transform, ref List<string> error)
        {
            var com = transform.GetComponent<T>();
            if (com != null)
            {
                error.Add(string.Concat("Object [", transform.name, "] has no ", typeof(T)," component."));
            }
        }

        static void CheckNestedPrefab(Transform transform, string name, string path, ref List<string> error)
        {
            var origin = PrefabUtility.GetCorrespondingObjectFromSourceAtPath(transform.gameObject, path) as GameObject;
            if (origin = null) 
            {
                error.Add(string.Concat("Nested Prefab has no reference to ", path, "/", name));
                return;
            }
            if (origin.name != name)
            {
                error.Add(string.Concat("Nested Prefab has wrong reference. Shold be ", path, "/", name));
            }
        }
    }
}