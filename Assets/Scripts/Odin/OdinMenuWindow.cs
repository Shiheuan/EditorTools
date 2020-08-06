using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

public class OdinMenuWindow : OdinMenuEditorWindow
{
    [MenuItem("Tools/Test Odin Menu Window")]
    private static void OpenWindow()
    {
        GetWindow<OdinMenuWindow>().Show();
    }

    [PropertyOrder(-10)]
    [HorizontalGroup]
    [Button(ButtonSizes.Large)]
    public void SomeButton1(){}

    [HorizontalGroup]
    [Button(ButtonSizes.Large)]
    public void SomeButton2(){}

    [HorizontalGroup]
    [Button(ButtonSizes.Large)]
    public void SomeButton3(){}

    [HorizontalGroup]
    [Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
    public void SomeButton4(){}

    [HorizontalGroup]
    [Button(ButtonSizes.Large), GUIColor(1, 0.5f, 0)]
    public void SomeButton5(){}

    [TableList]
    public List<SomeType> SomeTypeData;

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.Selection.SupportsMultiSelect = false;
        //tree.Add("Create New Player", new PlayerInfo());
        //tree.Add("Settings", GeneralDrawerConfig.Instance);
        //tree.Add("Utilities", new TextureUtilityEditor());
        //tree.Add("Player", new PlayerInfo());
        //tree.AddAllAssetsAtPath("Odin Settings", "Assets/Plugins/Sirenix", typeof(ScriptableObject), true, true);
        tree.AddAllAssetsAtPath("Player Data", "Assets/Data", typeof(PlayerInfo));
        var data = ScriptableObject.CreateInstance<CharacterPrefabData>();
        var obj = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/SM_Chr_Fairy_01_Root.prefab");
        data.OnlyModelPrefab = new List<GameObject>();
        data.OnlyModelPrefab.Add(obj);
        tree.Add("Generator", data);
        return tree;
    }
}

public class TextureUtilityEditor
{
    [BoxGroup("Tool"), HideLabel, EnumToggleButtons]
    public Tool Tool;

    public List<Texture> Textures;

    [Button(ButtonSizes.Large), HideIf("Tool", Tool.Rotate)]
    public void SomeAction() { }

    [Button(ButtonSizes.Large), ShowIf("Tool", Tool.Rotate)]
    public void SomeOtherAction() { }
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class PlayerInfo : ScriptableObject
{
    [BoxGroup("Info")]
    public string Name = "Player1";
    [BoxGroup("Info")]
    public int hp;
    [BoxGroup("Info")]
    public int mp;
}

public class CharacterPrefabData : ScriptableObject
{

    [AssetsOnly]
    public List<GameObject> OnlyModelPrefab;
    //[HorizontalGroup("Poster")]
    [ToggleLeft]
    public bool Poster;

    [ShowIfGroup("Poster")]
    [BoxGroup("Poster")]
    [Button(ButtonSizes.Medium)]
    public void Generate(){}

    [ShowIfGroup("Poster")]
    [BoxGroup("Poster")]
    [Button(ButtonSizes.Medium)]
    public void Check(){}

    [BoxGroup("Battle")]
    [ToggleLeft]
    public bool Battle;

    [BoxGroup("BossRush")]
    [ToggleLeft]
    public bool BossRush;

    [BoxGroup("Drama")]    
    [ToggleLeft]
    public bool Drama;

    [BoxGroup("Enhance")]
    [ToggleLeft]
    public bool Enhance;

    [BoxGroup("Gacha")]
    [ToggleLeft]
    public bool Gacha;

    [BoxGroup("Album")]
    [ToggleLeft]
    public bool Album;

}