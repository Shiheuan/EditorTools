using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
public class TestInspector : MonoBehaviour
{   
    [AssetsOnly]
    public GameObject ProjectObj;
    [SceneObjectsOnly]
    public GameObject SceneObj;
    [SerializeField]
    GameObject prefab;

    [CustomValueDrawer("HaveLabelNameFunction")]
    public string HaveLabelName;

    public string HaveLabelNameFunction(string tempName, GUIContent label)
    {
        return EditorGUILayout.TextField(label, tempName);
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
