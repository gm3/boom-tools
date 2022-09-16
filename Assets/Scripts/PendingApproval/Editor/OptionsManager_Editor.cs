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

    GUIStyle styleCentered, styleTitle, styleTitleCentered, styleCorrect, styleWarning, styleWrong, textFieldStyle, buttonStyle;

    string newOptionsName = "";
    string identifierName = "";
    string traitName = "";

    bool isEditing = false;
    public override void OnInspectorGUI()
    {
        styleCentered = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, wordWrap = true };
        styleTitle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };
        styleTitleCentered = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
        styleCorrect = new GUIStyle(GUI.skin.label) { normal = { textColor = Color.green } };
        styleWarning = new GUIStyle(GUI.skin.label) { normal = { textColor = Color.yellow } };
        styleWrong = new GUIStyle(GUI.skin.label) { normal = { textColor = Color.red } };


        textFieldStyle = new GUIStyle(GUI.skin.textField) { alignment = TextAnchor.MiddleCenter };
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
            case 4:
                RulesSetup();
                break;
        }


    }

    private void InitialSetup()
    {
        if (GUILayout.Button("Go to DNA manager", GUILayout.Height(30f)))
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
                if (myScript.mainCharacterOptions.objects.Count == 0)
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

        if (myScript.actionCallers == null)
            guiEnabled = false;
        else if (myScript.actionCallers.Count == 0)
            guiEnabled = false;
        GUI.enabled = guiEnabled;
        if (GUILayout.Button("Rules", GUILayout.Height(30f)))
        {
            myScript.setupStage = 4;
        }
    }
    private void CharacterSetup()
    {

        // GoBackButton();
        GUILayout.Space(5f);

        #region Main Edit
        if (ActiveEditorTracker.sharedTracker.isLocked)
        {
            isEditing = true;

            if (GUILayout.Button("Finish Editing", GUILayout.Height(30f)))
            {
                ActiveEditorTracker.sharedTracker.isLocked = false;
                Selection.activeGameObject = myScript.gameObject;
            }

            GUILayout.Label("EDIT MODE\n\n *Select gameObjects and click Add selected (you may choose more than 1 at a time)\n*Use Add blendshape only if any of your characters contain this information\n", BoomToolsGUIStyles.CustomColorLabel(true,false,true,Color.yellow));

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
            GoBackButton();
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

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Humanoid", BoomToolsGUIStyles.CustomLabel(false, true, false), GUILayout.MinWidth(30f));
            EditorGUILayout.LabelField("Trait Name", BoomToolsGUIStyles.CustomLabel(false, true, false), GUILayout.Width(200f));
            EditorGUILayout.LabelField("Weight", BoomToolsGUIStyles.CustomLabel(true, true, false), GUILayout.Width(40f));
            if (isEditing)
                EditorGUILayout.LabelField("X", BoomToolsGUIStyles.CustomLabel(true,true,false), GUILayout.Width(20f));
            EditorGUILayout.EndHorizontal();
        }
        bool fixables = false;
        for (int i = 0; i < myScript.mainCharacterOptions.objects.Count; i++)
        {
            if (myScript.mainCharacterOptions.objects[i] != null)
            {

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.ObjectField(myScript.mainCharacterOptions.objects[i], typeof(Object), true);

                GameObject go = myScript.mainCharacterOptions.objects[i] as GameObject;
                Animator goAnim = go.GetComponent<Animator>();
                Avatar animAvt = goAnim == null ? null : goAnim.avatar;

                string errorField = "";
                if (goAnim == null) errorField = "No animator in gameObject";
                else if (goAnim.avatar == null) errorField = "No Avatar in Animator";
                else if (!goAnim.avatar.isHuman) errorField = "Non Human Avatar setup";
                else if (!goAnim.avatar.isValid) errorField = "Non Valid Animator Avatar setup";

                SkinnedMeshRenderer rend = go.GetComponentInChildren<SkinnedMeshRenderer>();
                bool correctRend = true;
                if (rend == null)
                {
                    errorField = "No skinned mesh render";
                    correctRend = false;
                }
                else
                {
                    if (!rend.sharedMesh.isReadable)
                    {
                        errorField = "Non readable mesh";
                        correctRend = false;
                    }
                }

                if (isEditing)
                {
                    if (errorField == "")
                    {
                        string trait = EditorGUILayout.TextField(myScript.mainCharacterOptions.nameTraits[i], GUILayout.Width(200f));
                        if (trait != myScript.mainCharacterOptions.nameTraits[i])
                        {
                            Undo.RecordObject(myScript.mainCharacterOptions, "Set Trait Name Value");
                            myScript.mainCharacterOptions.nameTraits[i] = trait;
                        }
                        int weight = EditorGUILayout.IntField(myScript.mainCharacterOptions.weights[i], BoomToolsGUIStyles.CustomLabel(true,false,false), GUILayout.Width(40f));
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

                        EditorGUILayout.LabelField(errorField, BoomToolsGUIStyles.CustomColorLabel(false, false, false, Color.red), GUILayout.Width(200f));

                        if (PrefabUtility.IsPartOfModelPrefab(go))
                        {
                            if (goAnim == null || goAnim.avatar == null)
                            {
                                fixables = true;
                                if (GUILayout.Button("Fix", GUILayout.Width(40f)))
                                {
                                    TryFixHumanoidOptions(go);
                                }
                            }
                            else
                            {
                                if (GUILayout.Button("MFix", GUILayout.Width(40f)))
                                {
                                    Object obj = PrefabUtility.GetCorrespondingObjectFromOriginalSource(go);
                                    ActiveEditorTracker.sharedTracker.isLocked = false;
                                    EditorGUIUtility.PingObject(obj);
                                    Selection.activeObject = obj;
                                }
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
                    if (errorField == "")
                    {
                        EditorGUILayout.LabelField(myScript.mainCharacterOptions.nameTraits[i], BoomToolsGUIStyles.CustomColorLabel(false, false, false, Color.green), GUILayout.Width(200f));
                        EditorGUILayout.LabelField(myScript.mainCharacterOptions.weights[i].ToString(), GUILayout.Width(40f));
                    }
                    else
                    {
                        EditorGUILayout.LabelField(errorField, BoomToolsGUIStyles.CustomColorLabel(false, false, false, Color.red), GUILayout.Width(200f));
                        if (PrefabUtility.IsPartOfModelPrefab(go))
                        {
                            if (goAnim == null || goAnim.avatar == null || !correctRend)
                            {
                                fixables = true;
                                if (GUILayout.Button("Fix", GUILayout.Width(40f)))
                                {
                                    TryFixHumanoidOptions(go);
                                }
                            }
                            else
                            {
                                if (GUILayout.Button("MFix", GUILayout.Width(40f)))
                                {
                                    Object obj = PrefabUtility.GetCorrespondingObjectFromOriginalSource(go);
                                    ActiveEditorTracker.sharedTracker.isLocked = false;
                                    EditorGUIUtility.PingObject(obj);
                                    Selection.activeObject = obj;
                                }
                            }
                        }
                        else
                        {
                            EditorGUILayout.LabelField("Fix", GUILayout.Width(40f));
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                myScript.mainCharacterOptions.RemoveAtIndex(i);
            }
        }
        if (fixables)
        {
            EditorGUILayout.LabelField("*Objects are required to be humanoid");
            if (GUILayout.Button("Try quick fix", GUILayout.Height(30f)))
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
            if (debug) Debug.LogWarning("Cant fix, require manual fixing");
        }
        else
        {
            importer.animationType = ModelImporterAnimationType.Human;
            importer.isReadable = true;
            importer.autoGenerateAvatarMappingIfUnspecified = true;

            AssetDatabase.ImportAsset(path);

            Animator anim = go.GetComponent<Animator>();
            if (anim == null)
                anim = go.AddComponent<Animator>();
            if (anim.avatar == null)
                anim.avatar = importer.sourceAvatar;
        }
    }
    private void OptionSetup()
    {
        GoBackButton();
        GUILayout.Space(5f);
        GUILayout.Label("=Instructions= \n\n Create options for traits: Type a name for the options, then use the buttons below to define the type of options they can choose from.\n", styleCentered);

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
            Undo.RegisterCreatedObjectUndo(myScript.AddRandomObjectOption(typeof(RandomGameObject), newOptionsName), "Object Option");
            newOptionsName = "";
            GUI.FocusControl(null);
        }
        if (GUILayout.Button("Textures", GUILayout.Height(30f)))
        {
            Undo.RegisterCreatedObjectUndo(myScript.AddRandomObjectOption(typeof(RandomTexture), newOptionsName), "Texture Option");
            newOptionsName = "";
            GUI.FocusControl(null);
        }
        if (GUILayout.Button("Materials", GUILayout.Height(30f)))
        {
            Undo.RegisterCreatedObjectUndo(myScript.AddRandomObjectOption(typeof(RandomMaterial), newOptionsName), "Material Option");
            newOptionsName = "";
            GUI.FocusControl(null);
        }
        //if (GUILayout.Button("Materials")) Selection.activeTransform = myScript.AddRandomObjectOption(typeof(RandomMaterial));
        EditorGUILayout.EndHorizontal();
        GUI.enabled = true;
        GUILayout.Space(5f);
        if (myScript.randomObjects?.Count > 0)
        {
            EditorGUILayout.LabelField("== Current OPTIONS ==\n\n Click on view to add, remove, edit and set weight on options\n", styleCentered);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Name", styleTitle, GUILayout.MinWidth(30f));
            EditorGUILayout.LabelField("Type", styleTitle, GUILayout.Width(70f));
            EditorGUILayout.LabelField("Options", styleTitle, GUILayout.Width(70f));
            EditorGUILayout.LabelField("Edit", styleTitleCentered, GUILayout.Width(60f));
            EditorGUILayout.LabelField("X", styleTitleCentered, GUILayout.Width(20f));
            EditorGUILayout.EndHorizontal();


            for (int i = 0; i < myScript.randomObjects.Count; i++)
            {
                if (myScript.randomObjects[i] == null)
                {
                    myScript.RemoveElement(ref myScript.randomObjects,i);
                }
                else
                {
                    RandomObject ro = myScript.randomObjects[i].GetComponent<RandomObject>();

                    string labelType;
                    switch (ro.GetType().ToString())
                    {
                        case "RandomGameObject":
                            labelType = "GObject";
                            break;
                        case "RandomTexture":
                            labelType = "Texture";
                            break;
                        case "RandomMaterial":
                            labelType = "Material";
                            break;
                        default:
                            labelType = "Generic";
                            break;
                    }
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(myScript.randomObjects[i].name, BoomToolsGUIStyles.CustomColorLabel(false,false,false, ro.HasCorrectSetup() ? Color.green:Color.red), GUILayout.MinWidth(30f));
                    EditorGUILayout.LabelField(labelType, GUILayout.Width(70f));
                    int val = ro.objects == null ? 0 : ro.objects.Count;
                    EditorGUILayout.LabelField(val.ToString(), val == 0 ? styleWrong : styleCorrect, GUILayout.Width(70f));


                    if (GUILayout.Button("View", GUILayout.Width(60f))) Selection.activeGameObject = myScript.randomObjects[i];
                    if (GUILayout.Button("X", GUILayout.Width(20f)))
                    {
                        Undo.RegisterFullObjectHierarchyUndo(myScript.optionsHolder, "Remove Random Options");
                        Undo.RecordObject(myScript, "Remove Random Options");
                        myScript.RemoveElement(ref myScript.randomObjects, i);
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
        GUILayout.Label("=Instructions= " +
            "\n\nAdd action traits and assign previous options to them.\n" +
            "*Identifier name: a custom name to quickly identify this action\n" +
            "*Trait name: this name will be exported in the final json file\n", styleCentered);

        GUILayout.Space(5f);

        identifierName = EditorGUILayout.TextField("Identifier Name: ", identifierName);
        traitName = EditorGUILayout.TextField("Trait Name: ", traitName);

        GUILayout.Space(5f);

        if (identifierName.Length == 0 || traitName.Length == 0)
            GUI.enabled = false;
        else
            GUI.enabled = true;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Display Chosen Object", GUILayout.Height(30f)))
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
        if (myScript.actionCallers?.Count > 0)
        {
            EditorGUILayout.LabelField("== Current ACTIONS ==", styleCentered, GUILayout.Height(20f));
            EditorGUILayout.LabelField("\nIf text displays red, it means some values are still required for this action to work, a green color means it's ready, and yellow the option type required\n", styleCentered);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Name", styleTitle, GUILayout.MinWidth(30f));
            EditorGUILayout.LabelField("Type", styleTitle, GUILayout.Width(60f));
            EditorGUILayout.LabelField("Option", styleTitle, GUILayout.Width(100f));
            EditorGUILayout.LabelField("Trait", styleTitle, GUILayout.Width(70f));
            EditorGUILayout.LabelField("Edit", styleTitleCentered, GUILayout.Width(50f));
            EditorGUILayout.LabelField("X", styleTitleCentered, GUILayout.Width(20f));
            EditorGUILayout.EndHorizontal();
            for (int i = 0; i < myScript.actionCallers.Count; i++)
            {
                if (myScript.actionCallers[i] == null)
                {
                    myScript.RemoveElement(ref myScript.actionCallers, i);
                }
                else
                {
                    ActionCaller ac = myScript.actionCallers[i].GetComponent<ActionCaller>();

                    string labelType;
                    switch (ac.GetType().ToString())
                    {
                        case "SetObjectsVisibility":
                            labelType = "GObject";
                            break;
                        case "SetTextureToMaterial":
                            labelType = "Texture";
                            break;
                        case "SetMaterialToMesh":
                            labelType = "Material";
                            break;
                        default:
                            labelType = "Generic";
                            break;
                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(myScript.actionCallers[i].name, ac.IsValidTrait() ? styleCorrect : styleWrong, GUILayout.MinWidth(30f));
                    EditorGUILayout.LabelField(labelType, styleTitle, GUILayout.Width(60f));
                    if (ac.randomTarget == null)
                        EditorGUILayout.LabelField("--", GUILayout.Width(100f));
                    else
                        EditorGUILayout.LabelField(ac.randomTarget.gameObject.name, GUILayout.Width(100f));
                    EditorGUILayout.LabelField(ac.traitName, GUILayout.Width(70f));
                    if (GUILayout.Button("Edit", GUILayout.Width(50f))) Selection.activeGameObject = myScript.actionCallers[i];
                    if (GUILayout.Button("X", GUILayout.Width(20f)))
                    {
                        Undo.RegisterFullObjectHierarchyUndo(myScript.actionsHolder, "Remove Random Options");
                        Undo.RecordObject(myScript, "Remove Random Options");
                        myScript.RemoveElement(ref myScript.actionCallers, i);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
    }

    string ruleIdentifier = "";
    private void RulesSetup()
    {
        GoBackButton();
        GUILayout.Space(5f);
        GUILayout.Label("=Instructions= " +
            "\n\nAdd Rules, what options can exist and cannot when other options are chosen.\n", styleCentered);
        GUILayout.Space(5f);

        
        ruleIdentifier = EditorGUILayout.TextField("Rule Name: ", ruleIdentifier);


        if (ruleIdentifier.Length == 0)
            GUI.enabled = false;
        else
            GUI.enabled = true;
        if (GUILayout.Button("Add New Rule", GUILayout.Height(30f)))
        {
            GameObject newObj = myScript.AddRule(ruleIdentifier);
            Undo.RegisterCreatedObjectUndo(newObj, "New Super Rule");
            ruleIdentifier = "";
            GUI.FocusControl(null);
        }
        GUILayout.Space(5f);


        GUI.enabled = true;
        if (myScript.superRules?.Count > 0)
        {
            EditorGUILayout.LabelField("== Current Rules ==", styleCentered, GUILayout.Height(20f));
            EditorGUILayout.LabelField("\nIf text displays red, it means some values are still required for this rules to apply and green color means it's ready\n", styleCentered);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Name", styleTitle, GUILayout.MinWidth(30f));
            EditorGUILayout.LabelField("Edit", styleTitleCentered, GUILayout.Width(50f));
            EditorGUILayout.LabelField("X", styleTitleCentered, GUILayout.Width(20f));
            EditorGUILayout.EndHorizontal();


            for (int i = 0; i < myScript.superRules.Count; i++)
            {
                if (myScript.superRules[i] == null)
                {
                    myScript.RemoveElement(ref myScript.superRules, i);
                }
                else
                {
                    SuperRules rule = myScript.superRules[i].GetComponent<SuperRules>();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(myScript.superRules[i].name, rule.IsActiveRule() ? styleCorrect : styleWrong, GUILayout.MinWidth(30f));
                    if (GUILayout.Button("Edit", GUILayout.Width(50f))) Selection.activeGameObject = myScript.superRules[i];
                    if (GUILayout.Button("X", GUILayout.Width(20f)))
                    {
                        Undo.RegisterFullObjectHierarchyUndo(myScript.rulesHolder, "Remove Rule GameObject");
                        Undo.RecordObject(myScript, "Remove Super Rule");
                        myScript.RemoveElement(ref myScript.superRules, i);
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
