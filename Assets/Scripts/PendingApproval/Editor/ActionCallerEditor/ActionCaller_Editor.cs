using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActionCaller), true)]
public class ActionCaller_Editor : Editor
{
    ActionCaller myScript;

    private void OnEnable()
    {
        myScript = (ActionCaller)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (myScript.randomTarget != null)
            if (!myScript.IsValidType())
            {
                myScript.randomTarget = null;
                Debug.LogWarning("Not a valid type of script for targetRandom in: " + myScript.gameObject.name);
            }
    }
}
