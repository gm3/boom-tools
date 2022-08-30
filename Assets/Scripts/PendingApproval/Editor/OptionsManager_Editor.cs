using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OptionsManager))]
public class OptionsManager_Editor : Editor
{
    OptionsManager myScript;
    private void OnEnable()
    {
        myScript = (OptionsManager)target;
    }

   
    //int secondarySetup = 0;
    GUIStyle style;
    GUIStyle textFieldStyle;
    string newOptionsName;
    string identifierName;
    string traitName;
    public override void OnInspectorGUI()
    {
        style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        textFieldStyle = new GUIStyle(GUI.skin.textField) { alignment = TextAnchor.MiddleCenter};
        //base.OnInspectorGUI();
        switch (myScript.setupStage) {
            case 0:
                InitialSetup();
                break;
            case 1:
                OptionSetup();
                break;
            case 2:
                ActionSetup();
                break;
        }
            
    
    }

    private void InitialSetup()
    {
        //if (GUILayout.Button("Edit Main Models"))
        //{

        //}
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Edit Options", GUILayout.Height(30f)))
        {
            myScript.setupStage = 1;
        }
        if (GUILayout.Button("Edit Actions", GUILayout.Height(30f)))
        {
            myScript.setupStage = 2;
        }
        EditorGUILayout.EndHorizontal();
    }

    private void OptionSetup()
    {
        GoBackButton();
        GUILayout.Space(5f);
        newOptionsName = EditorGUILayout.TextField("New options Name:", newOptionsName);
        GUILayout.Space(5f);

        if (newOptionsName == "")
            GUI.enabled = false;
        else
            GUI.enabled = true;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("GameObjects", GUILayout.Height(30f)))
        {
            Undo.RegisterCreatedObjectUndo(myScript.AddRandomObjectOption(typeof(RandomGameObject), "GameOject: "+newOptionsName), "Object Option");
            newOptionsName = "";
            GUI.FocusControl(null);
        }
        if (GUILayout.Button("Textures", GUILayout.Height(30f)))
        {
            Undo.RegisterCreatedObjectUndo(myScript.AddRandomObjectOption(typeof(RandomTexture), "Texture: " + newOptionsName), "Texture Option");
            newOptionsName = "";
            GUI.FocusControl(null);
        }
        if (GUILayout.Button("Materials", GUILayout.Height(30f)))
        {
            Undo.RegisterCreatedObjectUndo(myScript.AddRandomObjectOption(typeof(RandomMaterial), "Material: " + newOptionsName), "Material Option");
            newOptionsName = "";
            GUI.FocusControl(null);
        }
        //if (GUILayout.Button("Materials")) Selection.activeTransform = myScript.AddRandomObjectOption(typeof(RandomMaterial));
        EditorGUILayout.EndHorizontal();
        GUI.enabled = true;
        EditorGUILayout.LabelField("== Current OPTIONS ==", style, GUILayout.Height(20f));
        if (myScript.randomObjects != null)
        {
            for (int i = 0; i < myScript.randomObjects.Count; i++)
            {
                if (myScript.randomObjects[i] == null)
                {
                    myScript.RemoveRandomObjectOption(i);
                }
                else
                {
                    string name = myScript.randomObjects[i].name;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(myScript.randomObjects[i].name);
                    if (GUILayout.Button("Edit", GUILayout.Width(60f))) Selection.activeGameObject = myScript.randomObjects[i];
                    if (GUILayout.Button("X", GUILayout.Width(20f)))
                    {
                        Undo.RegisterFullObjectHierarchyUndo(myScript.optionsHolder, "Remove Random Options");
                        Undo.RecordObject(myScript, "Remove Random Options");
                        myScript.RemoveRandomObjectOption(i);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
    }

    private void ActionSetup()
    {
        GoBackButton();
        GUILayout.Space(5f);

        identifierName = EditorGUILayout.TextField("Identifier Name: ", identifierName);
        traitName = EditorGUILayout.TextField("Trait Name: ", traitName);

        GUILayout.Space(5f);

        if (identifierName == "" || traitName == "")
            GUI.enabled = false;
        else
            GUI.enabled = true;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Display Selected Object", GUILayout.Height(30f)))
        {
            Undo.RegisterCreatedObjectUndo(myScript.AddActionCaller(typeof(SetObjectsVisibility), identifierName, traitName), "New Set Object Visibility Action");
            traitName = "";
            identifierName = "";
            GUI.FocusControl(null);
        }
        if (GUILayout.Button("Set Texture to material", GUILayout.Height(30f)))
        {
            Undo.RegisterCreatedObjectUndo(myScript.AddActionCaller(typeof(SetTextureToMaterial), identifierName, traitName), "New Set Object Visibility Action");
            traitName = "";
            identifierName = "";
            GUI.FocusControl(null);
        }
        EditorGUILayout.EndHorizontal();
        GUI.enabled = true;
        EditorGUILayout.LabelField("== Current ACTIONS ==", style, GUILayout.Height(20f));
        if (myScript.actionCallers != null)
        {
            for (int i = 0; i < myScript.actionCallers.Count; i++)
            {
                if (myScript.actionCallers[i] == null)
                {
                    myScript.RemoveActionCaller(i);
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(myScript.actionCallers[i].name + " : " + myScript.actionCallers[i].GetComponent<ActionCaller>().traitName);
                    if (GUILayout.Button("Edit", GUILayout.Width(60f))) Selection.activeGameObject = myScript.actionCallers[i];
                    if (GUILayout.Button("X", GUILayout.Width(20f)))
                    {
                        Undo.RegisterFullObjectHierarchyUndo(myScript.actionsHolder, "Remove Random Options");
                        Undo.RecordObject(myScript, "Remove Random Options");
                        myScript.RemoveActionCaller(i);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
    }

    private void GoBackButton()
    {
        if (GUILayout.Button("Go Back", GUILayout.Height(30f)))
        {
            myScript.setupStage = 0;
            GUI.FocusControl(null);
        }
    }

}
