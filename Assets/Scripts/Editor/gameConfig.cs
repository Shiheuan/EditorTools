using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LitJson;

namespace PrefabGen
{
    public class gameConfig
    {
        static string configPath = "Assets/Text/Config.lua";
        static string configItem = "Assets/Text/ConfigItem.lua";

        static Dictionary<string, CharacterConfig> Data = null;

        [MenuItem("Tools/Load Config")]
        public static Dictionary<string,CharacterConfig> LoadConfig()
        {
            var context = "";
            var configJson = "";

            using (FileStream file = new FileStream(configPath, FileMode.Open))
            using (StreamReader reader = new StreamReader(file))
            {
                context = reader.ReadToEnd();
            }
            Debug.Log(context);
            if (!string.IsNullOrEmpty(context))
            {
                configJson = Json2TableTools.GetJsonFromLua(context);
            }
            Debug.Log(configJson);
            
            Data = JsonMapper.ToObject<Dictionary<string, CharacterConfig>>(configJson);

            Debug.Log(string.Concat("test: ", Data.ToString()));
            //Debug.Log(GetModelPath(1, "PosterModel.body"));
            return Data;
        }

    }

    [System.Serializable]
    public class CharacterConfig
    {
        public int ID;
        public string Name;
        public Dictionary<string, Dictionary<string, string>> Model;

        public string GetModelPath(string config)
        {
            var key = config.Split('.');
            return Model[key[0]][key[1]];
        }
    }

    [System.Serializable]
    public class ModelList
    {
        public string body = "";
        public string weapon = "";
        public string parts = "";
    }
}