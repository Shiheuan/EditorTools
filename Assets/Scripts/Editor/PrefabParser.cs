using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;

namespace PrefabGen
{
    public enum ComponentType
    {
        Null = 0,
        GameObject = 1,
        Transform = 4,
        BoxCollider = 65,
        PlayableDirector = 320
    };

    class MyAllPostprocessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            // happened when change, also fit certain prefab string.
            foreach (string str in importedAssets)
            {
                Regex prefabSuffix = new Regex(".prefab$"); // '$' match end position
                var data = RawData.Load(AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Text/Poster.json").text);
                Regex nameSuffix = new Regex(data.characterName); 
                string absolutePath = Application.dataPath + str.Replace("Assets", ""); // del repeat "Assets" path
                if (prefabSuffix.IsMatch(str) && nameSuffix.IsMatch(str)){ // check str has ".prefab" or not
                    ResetTxt(absolutePath);
                }
            }
        }
        // parse .prefab txt
        static void ResetTxt(string path){
            StreamReader sr = new StreamReader(path);
            string str = sr.ReadToEnd();
            //Debug.Log(str);
            sr.Close();
            string pattern = @"m_InitialState: \d"; // match any decimal
            Regex prefabSuffix = new Regex(pattern);
            if(prefabSuffix.IsMatch(str)){
                str = prefabSuffix.Replace(str, "m_InitialState: 0");
            }
            StreamWriter sw = new StreamWriter(path, false);
            sw.WriteLine(str);
            sw.Close();
        }
    }

    class PrefabParser 
    {
        static string configPath = "Assets/Text";
        static string prefabPath = "Assets/Prefabs";
        [MenuItem("Tools/PrefabParser")]
        public static void CheckPosterPrefab()
        {
            if (!Directory.Exists(prefabPath))
            {
                Debug.Log("No Prefabs Directory.");
                return;
            }
            // get config.
            TextAsset jsonStr = AssetDatabase.LoadAssetAtPath<TextAsset>(string.Concat(configPath,"/Poster.json"));
            RawData data = RawData.Load(jsonStr.text);

            Regex prefabSuffix = new Regex(string.Concat(data.characterName,".prefab$"));

            foreach (var name in Directory.GetFiles(prefabPath))
            {
                if (prefabSuffix.IsMatch(name))
                {
                    Dictionary<string, string> dict = GetPrefabInfo(string.Concat(prefabPath, "/", data.characterName, ".prefab"));
                }
            }
        }

        private static Dictionary<string, string> GetPrefabInfo(string path)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Regex prefabSuffix = new Regex(@"--- !u!(?=\d+)");
            Regex Component_type = new Regex(@"^\d+\b");
            Regex Guid_txt = new Regex(@"(?<=&)\d+");
            string absolutePath = Application.dataPath + path.Replace("Assets", "");
            string text_prefab = System.IO.File.ReadAllText(absolutePath);
            string[] rawdatas = prefabSuffix.Split(text_prefab);

            foreach (var data in rawdatas)
            {
                if (!Component_type.IsMatch(data) || !Guid_txt.IsMatch(data)){
                    continue;
                }
                var Type = Component_type.Match(data);
                var id = Guid_txt.Match(data);
                // Debug.Log(Type);
                // Debug.Log(id);
                // Debug.Log("---"); 
                switch((ComponentType)int.Parse(Type.Value))
                {
                    case ComponentType.GameObject:
                        dict.Add(id.Value, data);
                        break;
                    case ComponentType.PlayableDirector:
                        dict.Add(id.Value, data);
                        break;
                    case ComponentType.BoxCollider:
                        dict.Add(id.Value, data);
                        break;
                    case ComponentType.Transform:
                        dict.Add(id.Value, data);
                        break;
                }
            }

            foreach(var item in dict){
                Debug.Log(string.Concat(item.Key, ": ",item.Value));
            }

            return dict;
            // TODO: to RawComponent
        }
    }
}