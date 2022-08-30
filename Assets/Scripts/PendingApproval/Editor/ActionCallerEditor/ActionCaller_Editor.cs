using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActionCaller), true)]
public class ActionCaller_Editor : Editor
{
    ActionCaller myScript;

    public override void OnInspectorGUI()
    {
        myScript = (ActionCaller)target;
        BackButton();
        base.OnInspectorGUI();
        if (myScript.randomTarget != null)
        {
            if (!myScript.IsValidType())
            {
                myScript.randomTarget = null;
                Debug.LogWarning("Not a valid type of script for targetRandom in: " + myScript.gameObject.name);
            }
        }
    }
    protected void BackButton()
    {
        if (myScript.optionsManager != null)
        {
            if (GUILayout.Button("Back to create actions", GUILayout.Height(30f)))
            {
                Selection.activeObject = myScript.optionsManager.gameObject;
            }
        }
    }
}
