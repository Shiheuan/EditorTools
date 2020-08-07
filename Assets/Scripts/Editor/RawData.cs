using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PrefabGen
{
    [Serializable]
    public class RawData
    {
        public string prefabName = "";
        public bool Readable = false;
        public List<RawComponent> subs = new List<RawComponent>();
        //private RawData instance = null;

        public static RawData Load(string json){
            return JsonUtility.FromJson<RawData>(json);
        }

        public bool Save(){
            return false;
        }
    }
    //[SerializeField]
    [Serializable]
    public class RawComponent
    {
        public string localId = "";
        public ComponentType Type = ComponentType.Null;
        public string Parent = "";
        public string Name = "";
        public string SourceName = "";
        public AttrConfig Params = new AttrConfig();
    }

    [Serializable]
    public class AttrConfig
    {
        // timeline params
        public bool playOnAwake = false;
        public int directorWrapMode = (int)UnityEngine.Playables.DirectorWrapMode.None;

        // common params
        public Vector3 position = Vector3.zero;
    }
}