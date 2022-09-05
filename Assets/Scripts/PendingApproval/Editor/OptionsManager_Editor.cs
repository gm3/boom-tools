using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VRM;
[CustomEditor(typeof(OptionsManager))]
public class OptionsManager_Editor : Editor
{
    OptionsManager myScript;
    private void OnEnable()
    {
        myScript = (OptionsManager)target;
    }

   
    //int secondarySetup = 0;
    GUIStyle styleCentered;
    GUIStyle styleCenteredYellow;
    GUIStyle styleCorrect;
    GUIStyle styleWrong;
    GUIStyle textFieldStyle;
    GUIStyle buttonStyle;
    string newOptionsName = "";
    string identifierName = "";
    string traitName = "";

    bool isEditing = false;
    public override void OnInspectorGUI()
    {
        styleCentered = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter , wordWrap = true};
        styleCenteredYellow = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter , wordWrap = true, normal = { textColor = Color.yellow }, hover = { textColor = Color.yellow } };
        styleCorrect = new GUIStyle(GUI.skin.label) { normal = { textColor = Color.green } };
        styleWrong = new GUIStyle(GUI.skin.label) { normal = { textColor = Color.red} };


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
        if (GUILayout.Button("Back to DNA manager", GUILayout.Height(30f)))
        {
            if (myScript.dnaManager == null)
            {
                myScript.dnaManager = GameObject.FindObjectOfType<DNAManager>();
            }
            if (myScript.dnaManager != null)
            {
                Selection.activeGameObject = myScript.dnaManager.gameObject;
            }
        }
        GUILayout.Space(5f);
        if (GUILayout.Button("Main Models", GUILayout.Height(30f)))
        {
            myScript.CreateBasicCharacterSetup();
            myScript.setupStage = 1;
        }
        bool guiEnabled = true;
        if (myScript.mainCharacterOptions != null)
        {
            if (myScript.mainCharacterOptions.objects == null)
                guiEnabled = false;
            else
                if(myScript.mainCharacterOptions.objects.Count == 0)
                    guiEnabled = false;
        }
        else
            guiEnabled = false;

        GUI.enabled = guiEnabled;
        if (GUILayout.Button("Options", GUILayout.Height(30f)))
        {
            myScript.setupStage = 2;
        }

        //check if there is at least 1 options in random objects
        if (myScript.randomObjects != null)
        {
            if (myScript.randomObjects.Count == 0)
                guiEnabled = false;
        }
        else
            guiEnabled = false;

        GUI.enabled = guiEnabled;

        if (GUILayout.Button("Actions", GUILayout.Height(30f)))
        {
            myScript.setupStage = 3;
        }
    }
    private void CharacterSetup()
    {
        
        GoBackButton();
        GUILayout.Space(5f);

        #region Main Edit
        if (ActiveEditorTracker.sharedTracker.isLocked)
        {
            isEditing = true;
            GUILayout.Label("EDIT MODE\n\n *Select gameObjects and click Add selected (you may choose more than 1 at a time)\n*Use Add blendshape only if any of your characters contain this information\n", styleCenteredYellow);
            
            if (GUILayout.Button("Finish Editing", GUILayout.Height(30f)))
            {
                ActiveEditorTracker.sharedTracker.isLocked = false;
                Selection.activeGameObject = myScript.gameObject;
            }

            // add new options
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Selected", GUILayout.Height(30f)))
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

            if (GUILayout.Button("Add Blendshape", GUILayout.Height(30f)))
            {
                Undo.RecordObject(myScript.mainCharacterAction, "Add Blendshape");
                myScript.mainCharacterAction._AddBlendShape();
            }
            EditorGUILayout.EndHorizontal();

        }
        else
        {
            GUILayout.Label("=Instructions= \n\n Choose gameObjects in your scene hierarchy with Animator component. Animator must have human avatar definition.\n", styleCentered);
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
        #endregion

        #region Selected Humanoids
        if (myScript.mainCharacterOptions.objects?.Count > 0)
        {
            GUILayout.Space(5f);
            EditorGUILayout.LabelField("== Humanoid Options ==", styleCentered);
            GUILayout.Space(5f);
        }
        bool fixables = false;
        for (int i = 0; i < myScript.mainCharacterOptions.objects.Count; i++)
        {

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.ObjectField(myScript.mainCharacterOptions.objects[i], typeof(Object), true);

            GameObject go = myScript.mainCharacterOptions.objects[i] as GameObject;
            Animator goAnim = go.GetComponent<Animator>();
            Avatar animAvt = goAnim == null ? null : goAnim.avatar;
           
            if (isEditing)
            {
                if (animAvt != null)
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
                    
                    string errorField = "Deleted gameObject";
                    if (goAnim == null) errorField = "No animator in gameObject";
                    if (goAnim == null) errorField = "No Avatar in Animator";
                    EditorGUILayout.LabelField(errorField, GUILayout.Width(200f));

                    if (PrefabUtility.IsPartOfModelPrefab(go))
                    {
                        fixables = true;
                        if (GUILayout.Button("Fix", GUILayout.Width(40f)))
                        {
                            //Undo.RecordObject(go, "Remove Object");
                            TryFixHumanoidOptions(go);
                        }
                    }
                    else
                    {
                        EditorGUILayout.LabelField("Fix", GUILayout.Width(40f));
                    }
                    if (GUILayout.Button("x", GUILayout.Width(20f)))
                    {
                        Undo.RecordObject(myScript.mainCharacterOptions, "Remove Object");
                        myScript.mainCharacterOptions.RemoveAtIndex(i);
                    }
                    
                }
                 
            }
            else
            {
                if (animAvt != null) { 
                    EditorGUILayout.LabelField(myScript.mainCharacterOptions.nameTraits[i], GUILayout.Width(200f));
                    EditorGUILayout.LabelField(myScript.mainCharacterOptions.weights[i].ToString(), GUILayout.Width(40f));
                }
                else
                {
                    EditorGUILayout.LabelField("Non humanoid gameObject", GUILayout.Width(200f));
                    if (PrefabUtility.IsPartOfModelPrefab(go))
                    {
                        fixables = true;
                        if (GUILayout.Button("Fix", GUILayout.Width(40f)))
                        {
                            TryFixHumanoidOptions(go);
                        }
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        if (fixables)
        {
            EditorGUILayout.LabelField("*Objects are required to be humanoid");
            if (GUILayout.Button("Try quick fix"))
            {
                FixAllHumanoids(myScript.mainCharacterOptions.objects);
            }
        }
        #endregion

        #region Blendshapes
        GUILayout.Space(5f);
        if (isEditing)
        {
            
            if (myScript.mainCharacterAction.blendShapes?.Count > 0)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };

                GUILayout.Space(5f);
                EditorGUILayout.LabelField("== BlendShapes ==", style);
                GUILayout.Space(5f);

                EditorGUILayout.LabelField("*Non case sensitive", GUILayout.Height(20f));
                EditorGUILayout.LabelField("*Leave empty text field to use shape name", GUILayout.Height(20f));
                EditorGUILayout.Space();

                for (int i = 0; i < myScript.mainCharacterAction.blendShapes.Count; i++)
                {
                    BlendShapePreset shape = myScript.mainCharacterAction.blendShapes[i];
                    string name = myScript.mainCharacterAction.blendShapeNames[i];
                    EditorGUILayout.BeginHorizontal();
                    shape = (BlendShapePreset)EditorGUILayout.EnumPopup(shape);
                    name = EditorGUILayout.TextField(name);

                    if (name != myScript.mainCharacterAction.blendShapeNames[i] || shape != myScript.mainCharacterAction.blendShapes[i])
                    {
                        Undo.RecordObject(myScript.mainCharacterAction, "Set blendshapes value");
                        myScript.mainCharacterAction.blendShapeNames[i] = name;
                        myScript.mainCharacterAction.blendShapes[i] = shape;
                    }

                    if (GUILayout.Button("X"))
                    {
                        Undo.RecordObject(myScript.mainCharacterAction, "Remove Blendshape");
                        myScript.mainCharacterAction._RemoveBlendshape(i);
                    }
                    EditorGUILayout.EndVertical();
                }
            }
        }
        #endregion 
    }
    private void FixAllHumanoids(List<Object> humanoids)
    {
        foreach (Object obj in humanoids)
        {
            TryFixHumanoidOptions(obj as GameObject, false);
        }
    }
    private void TryFixHumanoidOptions(GameObject go, bool debug = true)
    {
        string path = AssetDatabase.GetAssetPath(PrefabUtility.GetCorrespondingObjectFromOriginalSource(go));

        ModelImporter importer = ModelImporter.GetAtPath(path) as ModelImporter;

        if (importer == null)
        {
            if (debug)Debug.LogWarning("Cant fix, require manual fixing");
        }
        else
        {
            Debug.Log("fixing");
            importer.animationType = ModelImporterAnimationType.Human;
            importer.autoGenerateAvatarMappingIfUnspecified = true;
            AssetDatabase.ImportAsset(path);
        }
    }
    private void OptionSetup()
    {
        GoBackButton();
        GUILayout.Space(5f);
        GUILayout.Label("=Instructions= \n\n Create options for traits: Use the buttons below to define the type of options they can choose from.\n", styleCentered);

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
        GUILayout.Space(5f);
        if (myScript.randomObjects?.Count>0)
            EditorGUILayout.LabelField("== Current OPTIONS ==\n\n Click on view to add, remove, edit and set weight on options\n", styleCentered);
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
        GUILayout.Label("=Instructions= \n\nAdd action traits and assign previous options to them.\n", styleCentered);

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
            GameObject newObj = myScript.AddActionCaller(typeof(SetObjectsVisibility), identifierName, traitName);
            Undo.RegisterCreatedObjectUndo(newObj, "New Set Object Visibility Action");
            traitName = "";
            identifierName = "";
            GUI.FocusControl(null);
        }
        if (GUILayout.Button("Set Texture to material", GUILayout.Height(30f)))
        {
            GameObject newObj = myScript.AddActionCaller(typeof(SetTextureToMaterial), identifierName, traitName);
            Undo.RegisterCreatedObjectUndo(newObj, "New Set Texture To Material Action");
            traitName = "";
            identifierName = "";
            GUI.FocusControl(null);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Set Material To Model", GUILayout.Height(30f)))
        {
            GameObject newObj = myScript.AddActionCaller(typeof(SetMaterialToMesh), identifierName, traitName);
            Undo.RegisterCreatedObjectUndo(newObj, "New Set Material To Mesh Action");
            traitName = "";
            identifierName = "";
            GUI.FocusControl(null);
        }
        EditorGUILayout.EndHorizontal();
        GUI.enabled = true;
        EditorGUILayout.LabelField("== Current ACTIONS ==", styleCentered, GUILayout.Height(20f));
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
                    ActionCaller ac = myScript.actionCallers[i].GetComponent<ActionCaller>();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(myScript.actionCallers[i].name + " : " + ac.traitName, ac.IsValidTrait() ? styleCorrect : styleWrong);
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
