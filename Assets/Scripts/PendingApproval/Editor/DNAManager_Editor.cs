using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DNAManager))]
public class DNAManager_Editor : Editor
{
    DNAManager myScript;
    private void OnEnable()
    {
        myScript = (DNAManager)target;
    }
    public override void OnInspectorGUI()
    {
        if (myScript.optionsManager == null)
        {
            if (GUILayout.Button("Add options manager", GUILayout.Height(30f)))
            {
                myScript.optionsManager = myScript.transform.root.gameObject.AddComponent<OptionsManager>();
                myScript.optionsManager.dnaManager = myScript;
                Selection.activeGameObject = myScript.transform.root.gameObject;
            }
        }
        else
        {
            if (GUILayout.Button("Select options manager", GUILayout.Height(30f)))
            {
                Selection.activeGameObject = myScript.optionsManager.gameObject;
            }
        }
        base.OnInspectorGUI();
    }
}
