using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(SuperRules),true)]
public class SuperRules_Editor : Editor
{
    SuperRules myScript;
    List<RandomObject> randomList;

    // for step1
    [SerializeField]
    int curRandomSelected;
    string[] allRandombjects;

    // for step2
    string[] randomObjectOptions;
    int curObject = 0;
    string placeholderSelectObject = "Select Object Option";

    // for step 3
    string[] allActionCaller;
    int curActionSelected = 0;
    string placeholderSelectAction = "Select Action To Disable";
    
    protected virtual void OnEnable()
    {
        myScript = (SuperRules)target;
        ValidateListSize();
        if (myScript.optionsManager != null)
        {
            FetchEnums(myScript.optionsManager);
            if (myScript.randomObject == myScript.optionsManager.mainCharacterOptions)
                curRandomSelected = -2;
            else
                curRandomSelected = GetCurrentSelectedRandom(myScript.randomObject);
            FetchObjectOptions(myScript.randomObject);
        }
    }
    protected virtual void ValidateListSize()
    {
        if (myScript.targetOptions != null)
        {
            if (myScript.optionsTraitName == null)
                myScript.optionsTraitName = new List<string>();

            if (myScript.optionsTraitName.Count != myScript.targetOptions.Count)
            {

                for (int i = myScript.optionsTraitName.Count; i < myScript.targetOptions.Count; i++)
                {
                    myScript.optionsTraitName.Add("");
                }

            }
        }
    }
    public override void OnInspectorGUI()
    {
        if (myScript.optionsManager != null)
        {
            if (GUILayout.Button("Back To Create Rules", GUILayout.Height(30f)))
                Selection.activeGameObject = myScript.optionsManager.gameObject;

            GUILayout.Space(5f);


            // MAKE SURE USER DID NOT SELECT OPTIONS FROM CURRENT RANDOM OBJECTS
            if (myScript.targetOptions?.Count > 0)
                GUI.enabled = false;
            else
                GUILayout.Label("=Instructions= \n\n Select the options this rule be aware of.\n", BoomToolsGUIStyles.CustomLabel(true, false, true));


            if (myScript.randomObject != myScript.optionsManager.mainCharacterOptions)
            {
                if (GUILayout.Button("Choose Main Character", GUILayout.Height(30f)))
                {
                    Undo.RecordObject(myScript, "Change Random Object");
                    Undo.RecordObject(this, "Change Random Object");
                    myScript.randomObject = myScript.optionsManager.mainCharacterOptions;
                    FetchObjectOptions(myScript.randomObject);
                    curRandomSelected = -2;
                }
            }
            else
            {
                if (GUILayout.Button("Choose from Options", GUILayout.Height(30f)))
                {
                    Undo.RecordObject(myScript, "Change Random Object");
                    Undo.RecordObject(this, "Change Random Object");
                    myScript.randomObject = null;
                    FetchObjectOptions(myScript.randomObject);
                    curRandomSelected = -1;
                }
            }
            EditorGUILayout.Space(5f);
            if (myScript.randomObject != myScript.optionsManager.mainCharacterOptions)
            {
                int newSelected = EditorGUILayout.Popup("Target Options", curRandomSelected, allRandombjects);
                if (newSelected != curRandomSelected)
                {
                    RandomObject ro = myScript.optionsManager.randomObjects[newSelected].GetComponent<RandomObject>();
                    Undo.RecordObject(myScript, "Change Random Object");
                    Undo.RecordObject(this, "Change Random Object");
                    myScript.randomObject = ro;
                    FetchObjectOptions(ro);
                    curRandomSelected = newSelected;
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Target Options", GUILayout.Width(120f));
                EditorGUILayout.LabelField("Character", BoomToolsGUIStyles.CustomLabel(false,true,false));
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space(5f);


            GUI.enabled = true;

            if (myScript.targetOptions?.Count > 0)
                GUILayout.Label("(*Remove options below to edit this section again).", BoomToolsGUIStyles.CustomLabel(true, true, true));

            EditorGUILayout.Space(5f);

            // SECOND STEP - Select which options may be chosen from this random object
            if (myScript.randomObject != null)
            {
                EditorGUILayout.LabelField("Now select the options to watch\n", BoomToolsGUIStyles.CustomLabel(true,true,true));



                int newOption = EditorGUILayout.Popup("Add Objects: ", curObject, randomObjectOptions);
                EditorGUILayout.Space(5f);
                if (newOption != curObject)
                {
                    // make sure is (value -1), since we are using the first value as placeholder
                    Object obj = myScript.randomObject.objects[newOption - 1];
                    if (!ExistsInList(obj, ref myScript.targetOptions))
                    {
                        Undo.RecordObject(myScript, "Add Object Option");
                        myScript.AddOption(obj, myScript.randomObject.nameTraits[newOption - 1]);
                    }
                    else
                    {
                        Debug.LogWarning("Object already exists in list, skipping");
                    }
                    curActionSelected = 0;
                }

                if (myScript.targetOptions?.Count>0)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Trait Name",BoomToolsGUIStyles.CustomLabel(false, true, false), GUILayout.Width(120f));
                    EditorGUILayout.LabelField("Chosen Object",BoomToolsGUIStyles.CustomLabel(false, true, false));
                    EditorGUILayout.EndHorizontal();
                    for (int i = 0; i < myScript.targetOptions.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.ObjectField(myScript.optionsTraitName[i], myScript.targetOptions[i], typeof(Object), true);
                        if (GUILayout.Button("X", GUILayout.Width(20f)))
                        {
                            Undo.RecordObject(myScript, "Remove Object Option");
                            myScript.RemoveOption(i);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }


                // THIRD STEP - Indicate what will happen when this options are fullfilled
                EditorGUILayout.Space(15f);

                if (myScript.beChosen)
                {
                    if (GUILayout.Button("Switch to NOT chosen options", GUILayout.Height(30f)))
                        myScript.beChosen = false;
                }
                else
                {
                    if (GUILayout.Button("Switch to ARE chosen options", GUILayout.Height(30f)))
                        myScript.beChosen = true;
                }
                EditorGUILayout.LabelField("If any of the above options " + (myScript.beChosen ? "ARE" : "ARE NOT") + " chosen, the next actions will be DISABLED", BoomToolsGUIStyles.CustomLabel(true, false, true));
                EditorGUILayout.Space(15f);
                int newRandom = EditorGUILayout.Popup("Target Action: ", curActionSelected, allActionCaller);
                if (newRandom != curActionSelected)
                {
                    // make sure is (value -1), since we are using the first value as placeholder
                    ActionCaller ac = myScript.optionsManager.actionCallers[newRandom - 1].GetComponent<ActionCaller>();
                    if (!ExistsInList(ac, ref myScript.disableActions))
                    {
                        Undo.RecordObject(myScript, "Add Action Caller");
                        myScript.AddAction(ref myScript.disableActions, ac);
                    }
                    else
                    {
                        Debug.LogWarning("Action already exsits in list, skipping");
                    }
                    curActionSelected = 0;
                }



                if (myScript.disableActions?.Count > 0)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Action Name", BoomToolsGUIStyles.CustomLabel(false, true, false), GUILayout.Width(120f));
                    EditorGUILayout.LabelField("Chosen Action", BoomToolsGUIStyles.CustomLabel(false, true, false));
                    EditorGUILayout.EndHorizontal();
                    for (int i = 0; i < myScript.disableActions.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        //EditorGUILayout.LabelField(myScript.enableActions[i].gameObject.name);
                        EditorGUILayout.ObjectField(myScript.disableActions[i].gameObject.name, myScript.disableActions[i], typeof(Object), true);
                        if (GUILayout.Button("X", GUILayout.Width(20f)))
                        {
                            Undo.RecordObject(myScript, "Remove Enable Action Caller");
                            myScript.RemoveActionAt(ref myScript.disableActions, i);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }

            }

            //EditorGUILayout.LabelField("=======");
            //base.OnInspectorGUI();
        }

        else
        {

            base.OnInspectorGUI();
        }
    }
    private bool ExistsInList(Object targetAction, ref List<Object> objectList)
    {
        if (objectList == null)
            return false;
        foreach (Object ob in objectList)
        {
            if (targetAction == ob)
                return true;
        }
        return false;
    }
    private bool ExistsInList(ActionCaller targetAction, ref List<ActionCaller> actionList)
    {
        if (actionList == null)
            return false;
        foreach (ActionCaller ac in actionList)
        {
            if (targetAction == ac)
                return true;
        }
        return false;
    }
    private void FetchEnums(OptionsManager optionManager)
    {
        randomList = optionManager.GetRandomObjectOfType(typeof(RandomObject));

        allRandombjects = new string[myScript.optionsManager.randomObjects.Count];
        for (int i = 0; i < myScript.optionsManager.randomObjects.Count; i++)
        {
            allRandombjects[i] = myScript.optionsManager.randomObjects[i] == null ? "null" : myScript.optionsManager.randomObjects[i].name;
        }

        allActionCaller = new string[myScript.optionsManager.actionCallers.Count + 1];
        allActionCaller[0] = placeholderSelectAction;
        for (int i =0; i < myScript.optionsManager.actionCallers.Count; i++)
        {
            allActionCaller[i+1] = myScript.optionsManager.actionCallers[i] == null ? "null": myScript.optionsManager.actionCallers[i].name;
        }
    }
    private void FetchObjectOptions(RandomObject target)
    {
        if (target == null)
        {
            randomObjectOptions = new string[1];
        }
        else
        {
            randomObjectOptions = new string[target.nameTraits.Count + 1];
            randomObjectOptions[0] = placeholderSelectObject;
            for (int i = 0; i < target.nameTraits.Count; i++)
            {
                randomObjectOptions[i + 1] = target.nameTraits[i];
            }
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
