using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
* data class: 
*/
[Serializable]
public class CharacterGen
{
    public string characterName = "";
    public List<baseSub> subs = new List<baseSub>();
    private CharacterGen instance = null;

    public static CharacterGen Load(string json){
        return JsonUtility.FromJson<CharacterGen>(json);
    }

    public bool Save(){
        return false;
    }
}
//[SerializeField]
[Serializable]
public class baseSub
{
    public string Type = "";
    public string Parent = "";
    public string Name = "";
    public string SourceName = "";
}
