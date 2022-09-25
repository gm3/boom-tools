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
        string info = !myScript.selectionMode ? "Back to create actions" : "Back to select actions";
        if (myScript.optionsManager != null)
        {
            if (GUILayout.Button(info, GUILayout.Height(30f)))
            {
                Selection.activeObject = myScript.optionsManager.gameObject;
            }
        }


        if (!myScript.selectionMode)
        {
            MainUI();
        }
        else
        {
            SelectionModeUI();
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
    protected virtual void MainUI()
    {

        if (myScript.optionsManager != null)
        {

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
    }
    protected virtual void SelectionModeUI()
    {
        EditorGUILayout.LabelField("*Selection Mode: Click the button to trigger the action for that specific option", BoomToolsGUIStyles.CustomLabel(true,true,true));
        GUILayout.Space(5f);
        if (GUILayout.Button("none"))
        {
            myScript.SetTargetTrait(-1);
            myScript.SetAction();
        }
        if (myScript.randomTarget != null)
        {
            for (int i =0;i< myScript.randomTarget.objects.Count; i++)
            {
                //EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(myScript.randomTarget.nameTraits[i])){
                    myScript.SetTargetTrait(i);
                    myScript.SetAction();
                }
                //EditorGUILayout.EndHorizontal();
            }
        }
        
    }
    private void OnDisable()
    {
        myScript.selectionMode = false;
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
