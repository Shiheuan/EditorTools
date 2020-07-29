using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// test PreviewRenderUtility
// no documents at unity.com
public class PreviewPrefabEditor : EditorWindow
{
    PreviewRenderUtility m_PreviewRenderUtility;
    Material m_PreviewMaterial;
    Mesh m_PreviewMesh;

    //[MenuItem("Tools/Preview Prefab Window")]
    static void Setup()
    {
        GetWindow<PreviewPrefabEditor>();
    }

    private void OnGUI() {
        if (m_PreviewRenderUtility == null)
        {
            m_PreviewRenderUtility = new PreviewRenderUtility();
            m_PreviewRenderUtility.camera.farClipPlane = 500;
            m_PreviewRenderUtility.camera.clearFlags = CameraClearFlags.SolidColor;
            m_PreviewRenderUtility.camera.transform.position = new Vector3(0, 0, -10);

            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var meshFilter = go.GetComponentInChildren<MeshFilter>();
            m_PreviewMesh = meshFilter.sharedMesh;
            m_PreviewMaterial = go.GetComponent<MeshRenderer>().sharedMaterial;

            DestroyImmediate(go);
        }    
        var drawRect = new Rect(0, 0, 500, 500);
        m_PreviewRenderUtility.BeginPreview(drawRect, GUIStyle.none);
        m_PreviewRenderUtility.DrawMesh(m_PreviewMesh, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(30, 45, 0), Vector3.one), m_PreviewMaterial, 0);
        m_PreviewRenderUtility.camera.Render();
        var texture = m_PreviewRenderUtility.EndPreview();
        GUI.Box(drawRect, texture);
    }

    private void OnDisable() {
        m_PreviewRenderUtility.Cleanup();
    }
}

