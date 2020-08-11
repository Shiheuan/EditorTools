using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using PrefabGen;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace PrefabGen
{
    public class PrefabGeneratorEditor : OdinMenuEditorWindow 
    {
        static PrefabGeneratorEditor instance = null;
        static string lastScene = "";
        [MenuItem("Tools/Prefab Generator Tool")]
        private static void ShowWindow() {
            // new scene
            lastScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            instance = GetWindow<PrefabGeneratorEditor>();
            instance.titleContent = new GUIContent("Prefab Generator Tool");
            instance.Show();
        }
        protected override void OnDestroy()
        {
            if (!string.IsNullOrEmpty(lastScene))
                {
                    EditorSceneManager.OpenScene(lastScene, OpenSceneMode.Single);
                }
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
                var c = ScriptableObject.CreateInstance<CharacterData>();
                c.init(item.Value);
                //var c = new CharacterData(item.Value);
                tree.Add(item.Value.Name, c);
            }
            return tree;
        }

        public class CharacterData: SerializedScriptableObject
        {
            #region Debug
            [HideInInspector]
            private bool disable = true;
            [HideInInspector]
            private bool auto = true;
            #endregion
            
            public GameObject Model;

            #region ToggleList
            [BoxGroup("Toggle")]
            [ToggleLeft]
            public bool Poster;
            [BoxGroup("Toggle")]
            [ToggleLeft, DisableIf("disable")]
            public bool Battle;
            [BoxGroup("Toggle")]
            [ToggleLeft]
            public bool BossRush;
            [BoxGroup("Toggle", false)]
            [ToggleLeft, DisableIf("disable")]
            public bool Drama;
            [BoxGroup("Toggle")]
            [ToggleLeft, DisableIf("disable")]
            public bool Enhance;
            [BoxGroup("Toggle")]
            [ToggleLeft, DisableIf("disable")]
            public bool Gacha;
            [BoxGroup("Toggle")]
            [ToggleLeft, DisableIf("disable")]
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
            public Dictionary<string, GameObject> poster_models = new Dictionary<string, GameObject>();
            #endregion

            #region Battle
            // Battle
            [ShowIfGroup("Battle")]
            [FoldoutGroup("Battle/Battle", expanded: true)]
            [AssetsOnly, DisableIf("auto")]
            public GameObject BattlePrefab;
            [BoxGroup("Battle/Battle/Models")]
            [AssetsOnly]
            public Dictionary<string, GameObject> battle_models = new Dictionary<string, GameObject>();
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
            public Dictionary<string, GameObject> bossrush_models = new Dictionary<string, GameObject>();
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
            public Dictionary<string, GameObject> drama_models = new Dictionary<string, GameObject>();
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
            public Dictionary<string, GameObject> enhance_models = new Dictionary<string, GameObject>();
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
            public Dictionary<string, GameObject> gacha_models = new Dictionary<string, GameObject>();
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
            public Dictionary<string, GameObject> album_models = new Dictionary<string, GameObject>();
            // End Album
            #endregion

            private CharacterConfig config;
            [Button(ButtonSizes.Medium)]
            public void Generate()
            {
                if (Poster)
                {
                    GeneratePrefab.PrefabGenerator(this.config, "Poster");
                }
            }
            [Button(ButtonSizes.Medium)]
            public void Check(){}

            public void init(CharacterConfig config)
            {
                this.config = config;
                // pre check toggles
                Poster = config.Poster;
                Drama = config.Drama;
                BossRush = config.BossRush;
                Battle = config.Battle;
                Enhance = config.Enhnace;
                Gacha = config.Gacha;
                Album = config.Album;

                // get prefab path, check already exist
                this.CheckPrefabExist();

                // get models
                this.GetModels();
            }
            
            public void CheckPrefabExist()
            {
                if (this.Poster)
                {
                    var prefab = this.config.GetPrefabName(this.config.Name, "Poster");
                    var path = string.Concat(GeneratePrefab.prefabDirectory, "/", prefab, ".prefab");
                    if(File.Exists(path))
                    {
                        this.PosterPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    }
                }
            }

            public void GetModels()
            {
                this.Model = AssetDatabase.LoadAssetAtPath<GameObject>(string.Concat(GeneratePrefab.modelPath, "/", this.config.Body));
                if (this.Poster)
                {
                    foreach (var pair in this.config.PosterModel[0])
                    {
                        var asset = AssetDatabase.LoadAssetAtPath<GameObject>(string.Concat(GeneratePrefab.modelPath, "/", pair.Value));
                        poster_models.Add(pair.Key, asset);
                    }
                }
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

