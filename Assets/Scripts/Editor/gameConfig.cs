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
            //Debug.Log(context);
            if (!string.IsNullOrEmpty(context))
            {
                configJson = Json2TableTools.GetJsonFromLua(context);
            }
            //Debug.Log(configJson);
            
            Data = JsonMapper.ToObject<Dictionary<string, CharacterConfig>>(configJson);

            //Debug.Log(string.Concat("test: ", Data.ToString()));
            //Debug.Log(GetModelPath(1, "PosterModel.body"));
            return Data;
        }

    }

    [System.Serializable]
    public class CharacterConfig
    {
        public int ID;
        public string Name;
        //public Dictionary<string, Dictionary<string, string>> Model;
        public bool Poster;
        public List<Dictionary<string, string>> PosterModel;
        public bool Drama;
        public List<Dictionary<string, string>> DramaModel;
        public bool Battle;
        public List<Dictionary<string, string>> BattleModel;
        public bool Enhnace;
        public List<Dictionary<string, string>> EnhanceModel;
        public bool Gacha;
        public List<Dictionary<string, string>> GachaModel;
        public bool BossRush;
        public List<Dictionary<string, string>> BossRushModel;
        public bool Album;
        public List<Dictionary<string, string>> AlbumModel;
        private Dictionary<string, Dictionary<string,string>> AvaliableModels;

        public void init()
        {
            //TODO: for visit easily
            AvaliableModels = new Dictionary<string, Dictionary<string, string>>();
        }
        public string GetModelPath(string config)
        {
            var key = config.Split('.');
            var res = "";
            //TODO: Reflection
            switch(key[0]){
                case "Poster":
                    res = PosterModel[0][key[1]];
                    break;
                case "Battle":
                    res = BattleModel[0][key[1]];
                    break;
                case "Drama":
                    res = DramaModel[0][key[1]];
                    break;
                case "Enhance":
                    res = EnhanceModel[0][key[1]];
                    break;
                case "Gacha":
                    res = GachaModel[0][key[1]];
                    break;
                case "BossRush":
                    res = BossRushModel[0][key[1]];
                    break;
                case "Album":
                    res = AlbumModel[0][key[1]];
                    break;
            }
            // return Model[key[0]][key[1]];
            return res;
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