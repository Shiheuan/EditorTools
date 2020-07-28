using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PrefabGen
{
    public class PrefabCheck
    {
        [MenuItem("Tools/Check Prefab")]
        public static void CheckPrefab()
        {
            var prefab = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/SM_Chr_Fairy_01_Root.prefab")) as GameObject;
            var config = RawData.Load(AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Text/Poster.json").text);
            Check(config, prefab);
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