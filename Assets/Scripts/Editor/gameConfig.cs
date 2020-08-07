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
        public static void LoadConfig()
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
            
            //Data = JsonUtility.FromJson<CharacterConfig[]>(configJson);// can't do it
            Data = JsonMapper.ToObject<Dictionary<string, CharacterConfig>>(configJson);

            //Debug.Log(string.Concat("test: ", Data.ToString()));
            Debug.Log(string.Concat("test: ", Data.ToString()));
        }
        [MenuItem("Tools/Load ConfigItem")]
        public static void LoadConfigItem()
        {
            var context = "";
            var configJson = "";
            using (FileStream file = new FileStream(configItem, FileMode.Open))
            using (StreamReader reader = new StreamReader(file))
            {
                context = reader.ReadToEnd();
            }

            if (!string.IsNullOrEmpty(context))
            {
                configJson = Json2TableTools.GetJsonFromLua(context);
            }
            Debug.Log(configJson);
            //var Data_item = JsonUtility.FromJson<CharacterConfig>(configJson);
            var Data_Jit = JsonMapper.ToObject<CharacterConfig>(configJson);

            //Debug.Log(string.Concat("test: ", Data_item.ToString()));
            Debug.Log(string.Concat("test: ", Data_Jit.ToString()));
        }

        public static void GetValue(string config)
        {
            // var key = config.Split('.');
            // for
        }
    }

    [System.Serializable]
    public class CharacterConfig
    {
        public int ID;
        public string Name;
        public Dictionary<string, Dictionary<string, string>> Model;
        // public ModelList PosterModel;
        // public ModelList DramaModel;
        // public ModelList BattleModel;
        // public ModelList EnhanceModel;
        // public ModelList GachaModel;
        // public ModelList BossRushModel;
        // public ModelList AlbumModel;
    }

    [System.Serializable]
    public class ModelList
    {
        public string body = "";
        public string weapon = "";
        public string parts = "";
    }
}