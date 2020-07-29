using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LitJson;

public class CompareJson
{
    [MenuItem("Tools/Test Lua to Json")]
    public static void TestLua2Json()
    {
        string path = Application.dataPath + "/Text/Config.lua";
        using (FileStream file = new FileStream(path, FileMode.Open))
        using (StreamReader reader = new StreamReader(file))
        {
            var context = reader.ReadToEnd();
            Debug.Log(Json2TableTools.GetJsonFromLua(context));
        }
    }
}
