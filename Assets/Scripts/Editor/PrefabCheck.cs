using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using System.IO;
 
class MyAllPostprocessor : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        // happened when change, also fit certain prefab string.
        foreach (string str in importedAssets)
        {
            Regex prefabSuffix = new Regex(".prefab$"); // '$' match end position
            var data = CharacterGen.Load(AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Text/Poster.json").text);
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
        Debug.Log(str);
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
