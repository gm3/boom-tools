using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VRMAuthoringManager))]
public class VRMAuthoringManager_Editor : Editor
{
    VRMAuthoringManager myScript;
    private void OnEnable()
    {
        myScript = (VRMAuthoringManager)target;
    }

    public override void OnInspectorGUI()
    {
        if (myScript.dnaManager != null)
        {
            if (GUILayout.Button("Back To DNA Manager", GUILayout.Height(30f))) Selection.activeGameObject = myScript.dnaManager.gameObject;
        }
        else
        {
            myScript.dnaManager = (DNAManager)EditorGUILayout.ObjectField("DNA Manager", myScript.dnaManager, typeof(DNAManager),true);
        }
        base.OnInspectorGUI();
    }
}
