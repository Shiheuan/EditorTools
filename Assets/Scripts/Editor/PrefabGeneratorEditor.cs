using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PrefabGen;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace PrefabGen
{
    public class PrefabGeneratorEditor : OdinMenuEditorWindow 
    {
        static PrefabGeneratorEditor instance = null;
        [MenuItem("Tools/Prefab Generator Tool")]
        private static void ShowWindow() {
            instance = GetWindow<PrefabGeneratorEditor>();
            instance.titleContent = new GUIContent("Prefab Generator Tool");
            instance.Show();
        }

        //string configPath = "Assets/Text";
        //string testPrefabPath = "Assets/Prefabs";   

        //Vector2 scrollPosition;

        // private void OnGUI() {
        //     // check if prefab already exist
        //     // TextAsset jsonStr = AssetDatabase.LoadAssetAtPath<TextAsset>(string.Concat(configPath,"/Poster.json"));
        //     // RawData data = RawData.Load(jsonStr.text);
        //     GUILayout.BeginHorizontal();
            
        //     // if (File.Exists(string.Concat(testPrefabPath, "/", data.prefabName, ".prefab")))
        //     // {
        //     //     if (GUILayout.Button("Check"))
        //     //     {
        //     //         PrefabCheck.CheckPrefab();
        //     //     }
        //     // }
        //     // else
        //     // {
        //         if (GUILayout.Button("Generate"))
        //         {
        //             var config = gameConfig.LoadConfig();
        //             GeneratePrefab.PrefabGenerator("Poster", config["1"]);
        //         }
        //     // }

        //     //GUILayout.EndArea();
        //     GUILayout.EndHorizontal();
        // }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Config.DrawSearchToolbar = true;
            tree.Config.SearchToolbarHeight = 30;
            var data = gameConfig.LoadConfig();
            foreach (var item in data) 
            {
                var c = new CharacterData(item.Value);
                c.PosterPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Character1.prefab");
                tree.Add(item.Value.Name, c);
            }
            return tree;
        }

        public class CharacterData
        {
            #region Debug
            [HideInInspector]
            private bool develop = true;
            [HideInInspector]
            private bool auto = true;
            #endregion

            #region ToggleList
            [ToggleLeft]
            public bool Poster;
            [ToggleLeft, DisableIf("develop")]
            public bool Battle;
            [ToggleLeft]
            public bool BossRush;
            [ToggleLeft, DisableIf("develop")]
            public bool Drama;
            [ToggleLeft, DisableIf("develop")]
            public bool Enhance;
            [ToggleLeft, DisableIf("develop")]
            public bool Gacha;
            [ToggleLeft, DisableIf("develop")]
            public bool Album;
            #endregion

            #region Poster
            // Poster
            [ShowIfGroup("Poster")]
            [FoldoutGroup("Poster/Poster", expanded: true)]
            [AssetsOnly, DisableIf("auto")]
            public GameObject PosterPrefab;

            [BoxGroup("Poster/Poster/Models")]
            [AssetsOnly]
            public GameObject poster_body;
            [BoxGroup("Poster/Poster/Models")]
            [AssetsOnly]
            public GameObject poster_weapon;
            [BoxGroup("Poster/Poster/Models")]
            [AssetsOnly]
            public GameObject poster_parts;
            // End Poster
            #endregion

            #region Battle
            // Battle
            [ShowIfGroup("Battle")]
            [FoldoutGroup("Battle/Battle", expanded: true)]
            [AssetsOnly, DisableIf("auto")]
            public GameObject BattlePrefab;
            [BoxGroup("Battle/Battle/Models")]
            [AssetsOnly]
            public GameObject battle_body;
            [BoxGroup("Battle/Battle/Models")]
            [AssetsOnly]
            public GameObject battle_weapon;
            [BoxGroup("Battle/Battle/Models")]
            [AssetsOnly]
            public GameObject battle_parts;
            // End Battle
            #endregion
            
            #region BossRush
            // BossRush
            [ShowIfGroup("BossRush")]
            [FoldoutGroup("BossRush/BossRush", expanded: true)]
            [AssetsOnly, DisableIf("auto")]
            public GameObject BossRushPrefab;
            [BoxGroup("BossRush/BossRush/Models")]
            [AssetsOnly]
            public GameObject bossrush_body;
            [BoxGroup("BossRush/BossRush/Models")]
            [AssetsOnly]
            public GameObject bossrush_weapon;
            [BoxGroup("BossRush/BossRush/Models")]
            [AssetsOnly]
            public GameObject bossrush_parts;
            // End BossRush
            #endregion

            #region Drama
            // Drama
            [ShowIfGroup("Drama")]
            [FoldoutGroup("Drama/Drama", expanded: true)]
            [AssetsOnly, DisableIf("auto")]
            public GameObject DramaPrefab;
            [BoxGroup("Drama/Drama/Models")]
            [AssetsOnly]
            public GameObject drama_body;
            [BoxGroup("Drama/Drama/Models")]
            [AssetsOnly]
            public GameObject drama_weapon;
            [BoxGroup("Drama/Drama/Models")]
            [AssetsOnly]
            public GameObject drama_parts;
            // End Drama
            #endregion

            #region Enhance
            // Enhance
            [ShowIfGroup("Enhance")]
            [FoldoutGroup("Enhance/Enhance", expanded: true)]
            [AssetsOnly, DisableIf("auto")]
            public GameObject EnhancePrefab;
            [BoxGroup("Enhance/Enhance/Models")]
            [AssetsOnly]
            public GameObject enhance_body;
            [BoxGroup("Enhance/Enhance/Models")]
            [AssetsOnly]
            public GameObject enhance_weapon;
            [BoxGroup("Enhance/Enhance/Models")]
            [AssetsOnly]
            public GameObject enhance_parts;
            // End Enhance
            #endregion

            #region Gacha
            // Gacha
            [ShowIfGroup("Gacha")]
            [FoldoutGroup("Gacha/Gacha", expanded: true)]
            [AssetsOnly, DisableIf("auto")]
            public GameObject GachaPrefab;
            [BoxGroup("Gacha/Gacha/Models")]
            [AssetsOnly]
            public GameObject gacha_body;
            [BoxGroup("Gacha/Gacha/Models")]
            [AssetsOnly]
            public GameObject gacha_weapon;
            [BoxGroup("Gacha/Gacha/Models")]
            [AssetsOnly]
            public GameObject gacha_parts;
            // End Gacha
            #endregion
            
            #region Album
            // Album
            [ShowIfGroup("Album")]
            [FoldoutGroup("Album/Album", expanded: true)]
            [AssetsOnly, DisableIf("auto")]
            public GameObject AlbumPrefab;
            [BoxGroup("Album/Album/Models")]
            [AssetsOnly]
            public GameObject album_body;
            [BoxGroup("Album/Album/Models")]
            [AssetsOnly]
            public GameObject album_weapon;
            [BoxGroup("Album/Album/Models")]
            [AssetsOnly]
            public GameObject album_parts;
            // End Album
            #endregion

            [HorizontalGroup]
            [Button(ButtonSizes.Medium)]
            public void Generate(){}
            [HorizontalGroup]
            [Button(ButtonSizes.Medium)]
            public void Check(){}

            public CharacterData(CharacterConfig config)
            {
                // pre check toggles
                Poster = config.Poster;
                Drama = config.Drama;
                BossRush = config.BossRush;
                Battle = config.Battle;
                Enhance = config.Enhnace;
                Gacha = config.Gacha;
                Album = config.Album;

                // get prefab path, check already exist

                // get models
            }
        }
        public enum PrefabType
        {
            None = -1,
            Poster = 0,
            Battle = 1,
            BossRush = 2,
            Drama = 3,
            Enhance = 4,
            Gacha = 5,
            Album = 6
        }
    }

}

