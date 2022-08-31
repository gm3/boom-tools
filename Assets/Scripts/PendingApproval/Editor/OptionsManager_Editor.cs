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
    GUIStyle buttonStyle;
    string newOptionsName = "";
    string identifierName = "";
    string traitName = "";

    bool isEditing = false;
    public override void OnInspectorGUI()
    {
        style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        textFieldStyle = new GUIStyle(GUI.skin.textField) { alignment = TextAnchor.MiddleCenter};
        buttonStyle = new GUIStyle(GUI.skin.button);
        //buttonStyle.normal.
        //base.OnInspectorGUI();
        switch (myScript.setupStage) {
            case 0:
                InitialSetup();
                break;
            case 1:
                CharacterSetup();
                break;
            case 2:
                OptionSetup();
                break;
            case 3:
                ActionSetup();
                break;
        }
            
    
    }

    private void InitialSetup()
    {
        if (GUILayout.Button("Edit Main Models", GUILayout.Height(30f)))
        {
            myScript.CreateBasicCharacterSetup();
            myScript.setupStage = 1;
        }
        //if (myScript.)
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Edit Options", GUILayout.Height(30f)))
        {
            myScript.setupStage = 2;
        }
        if (GUILayout.Button("Edit Actions", GUILayout.Height(30f)))
        {
            myScript.setupStage = 3;
        }
        EditorGUILayout.EndHorizontal();
    }
    private void CharacterSetup()
    {
        GoBackButton();
        GUILayout.Space(5f);

        // start editing
        if (ActiveEditorTracker.sharedTracker.isLocked)
        {
            isEditing = true;
            if (GUILayout.Button("Finish Editing", GUILayout.Height(30f)))
            {
                ActiveEditorTracker.sharedTracker.isLocked = false;
                Selection.activeGameObject = myScript.gameObject;
            }

            // add new options
            if (GUILayout.Button("Add Selected GameObject(s)", GUILayout.Height(30f)))
            {
                Undo.RecordObject(myScript.mainCharacterOptions, "Add Humanoid Model");

                foreach (Object obj in Selection.objects)
                {
                    if (myScript.mainCharacterOptions.IsValidObjectType(obj))
                    {
                        if (!myScript.mainCharacterOptions.ObjectExists(obj))
                            myScript.mainCharacterOptions.AddObject(obj);
                        else
                            Debug.Log(myScript.mainCharacterOptions.objectName + " already added");
                    }
                    else
                    {
                        Debug.Log("need to add a GameObject from the scene");
                    }
                }
                EditorUtility.SetDirty(myScript);
            }
        }
        else
        {
            isEditing = false;
            if (GUILayout.Button("Edit", GUILayout.Height(30f)))
                ActiveEditorTracker.sharedTracker.isLocked = true;

        }

        if (myScript.mainCharacterOptions.objects.Count > 1)
        {
            GUILayout.Space(10f);
            string characterTraitName = EditorGUILayout.TextField("Trait name: ", myScript.mainCharacterAction.traitName);
            if (characterTraitName != myScript.mainCharacterAction.traitName)
            {
                Undo.RecordObject(myScript.mainCharacterAction, "Change Character Trait Name");
                myScript.mainCharacterAction.traitName = characterTraitName;
            }
        }

        //display selected options
        GUILayout.Space(5f);
        EditorGUILayout.LabelField("== Humanoid Options ==", style);
        GUILayout.Space(5f);

        for (int i = 0; i < myScript.mainCharacterOptions.objects.Count; i++)
        {

            EditorGUILayout.BeginHorizontal();
            


            EditorGUILayout.ObjectField(myScript.mainCharacterOptions.objects[i], typeof(Object), true);

            if (isEditing)
            {
                string trait = EditorGUILayout.TextField(myScript.mainCharacterOptions.nameTraits[i], GUILayout.Width(200f));
                if (trait != myScript.mainCharacterOptions.nameTraits[i])
                {
                    Undo.RecordObject(myScript.mainCharacterOptions, "Set Trait Name Value");
                    myScript.mainCharacterOptions.nameTraits[i] = trait;
                }
                int weight = EditorGUILayout.IntField(myScript.mainCharacterOptions.weights[i], GUILayout.Width(40f));
                if (weight != myScript.mainCharacterOptions.weights[i])
                {
                    Undo.RecordObject(myScript.mainCharacterOptions, "Set Weight Value");
                    myScript.mainCharacterOptions.weights[i] = weight;
                }
                if (GUILayout.Button("x", GUILayout.Width(20f)))
                {
                    Undo.RecordObject(myScript.mainCharacterOptions, "Remove Object");
                    myScript.mainCharacterOptions.RemoveAtIndex(i);
                }
                
                
            }
            else
            {
                EditorGUILayout.LabelField(myScript.mainCharacterOptions.nameTraits[i], GUILayout.Width(200f));
                EditorGUILayout.LabelField(myScript.mainCharacterOptions.weights[i].ToString(), GUILayout.Width(40f));
            }
            EditorGUILayout.EndHorizontal();
        }
    }
    private void OptionSetup()
    {
        GoBackButton();
        GUILayout.Space(5f);
        newOptionsName = EditorGUILayout.TextField("New options Name:", newOptionsName);
        GUILayout.Space(5f);

        if (newOptionsName.Length == 0)
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
                    if (GUILayout.Button("View", GUILayout.Width(60f))) Selection.activeGameObject = myScript.randomObjects[i];
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

        if (identifierName.Length == 0 || traitName.Length == 0)
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
                    if (GUILayout.Button("View", GUILayout.Width(60f))) Selection.activeGameObject = myScript.actionCallers[i];
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
        
        if (GUILayout.Button("Back To Manager", GUILayout.Height(30f)))
        {
            Selection.activeGameObject = myScript.gameObject;
            myScript.setupStage = 0;
            GUI.FocusControl(null);
            ActiveEditorTracker.sharedTracker.isLocked = false;
        }
    }

}
