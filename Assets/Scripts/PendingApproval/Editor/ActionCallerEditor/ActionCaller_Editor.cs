using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActionCaller), true)]
public class ActionCaller_Editor : Editor
{
    ActionCaller myScript;
    string[] options;
    List<RandomObject> randomList;
    [SerializeField]
    int curRandom;
    protected virtual void OnEnable()
    {
        myScript = (ActionCaller)target;
        if (myScript.optionsManager != null)
        {
            FetchRandomOptions(myScript.optionsManager, myScript.GetRandomObjectValidType());
            curRandom = GetCurrentSelectedRandom(myScript.randomTarget);
        }
    }
    public override void OnInspectorGUI()
    {
        //temporal
        //base.OnInspectorGUI();

        if (myScript.optionsManager != null)
        {
            if (GUILayout.Button("Back to create actions", GUILayout.Height(30f)))
            {
                Selection.activeObject = myScript.optionsManager.gameObject;
            }

            int newRandom = EditorGUILayout.Popup("Target Options: ", curRandom, options);
            if (newRandom != curRandom)
            {
                Undo.RecordObject(myScript, "Change Random Target");
                Undo.RecordObject(this, "Change Random Target");
                myScript.randomTarget = randomList[newRandom];
                curRandom = newRandom;
            }
            string traitName = EditorGUILayout.TextField("Trait Name: ", myScript.traitName);
            if (traitName != myScript.traitName)
            {
                Undo.RecordObject(myScript, "Change Trait Name");
                myScript.traitName = traitName;
            }
        }
        else
        {
            base.OnInspectorGUI();
        }



        if (myScript.randomTarget != null)
        {
            if (!myScript.IsValidType())
            {
                myScript.randomTarget = null;
                Debug.LogWarning("Not a valid type of script for targetRandom in: " + myScript.gameObject.name);
            }
        }
    }


    private void FetchRandomOptions(OptionsManager optionManager, System.Type type)
    {
        randomList = optionManager.GetRandomObjectOfType(type);
        options = new string[randomList.Count];
        for (int i =0; i < randomList.Count; i++)
        {
            options[i] = randomList[i].gameObject.name;
        }
    }
    private int GetCurrentSelectedRandom(RandomObject curRandom)
    {
        if (curRandom == null)
            return -1;
        foreach (RandomObject rand in randomList)
        {
            if (curRandom == rand)
                return randomList.IndexOf(rand);
        }
        return -1;
    }
}
