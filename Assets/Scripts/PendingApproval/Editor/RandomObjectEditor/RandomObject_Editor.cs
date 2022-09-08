using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomObject), true)]
public class RandomObject_Editor : Editor
{
    RandomObject myScript;

    protected string filterByObject = "";
    protected string filterByName = "";
    protected int filterByWeight = -1;
    bool enabledFilters = false;
    int operation = 0;
    string[] operationOptions;
    public bool editing = false;
    public GUIStyle style;
    protected virtual void OnEnable()
    {
        myScript = (RandomObject)target;
        myScript.SetObjectName();
        ValidateListSize();
        operationOptions = new string[3];
        operationOptions[0] = "equals";
        operationOptions[1] = "smaller";
        operationOptions[2] = "greater";

    }
    protected virtual void ValidateListSize()
    {
        if (myScript.objects != null)
        {
            if (myScript.weights == null) myScript.weights = new List<int>(myScript.objects.Count);
            if (myScript.nameTraits == null) myScript.nameTraits = new List<string>(myScript.objects.Count);

            if (myScript.weights.Count != myScript.objects.Count)
            {
                for (int i = myScript.weights.Count; i < myScript.objects.Count; i++)
                {
                    myScript.weights.Add(1);
                }

            }
            if (myScript.nameTraits.Count != myScript.objects.Count)
            {
                for (int i = myScript.nameTraits.Count; i < myScript.objects.Count; i++)
                {
                    myScript.nameTraits.Add(myScript.objects[i].name);
                }
            }
        }
    }
    public override void OnInspectorGUI()
    {
        editing = ActiveEditorTracker.sharedTracker.isLocked;
        style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, wordWrap = true };
        GUIStyle styleCenteredYellow = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, wordWrap = true, normal = { textColor = Color.yellow }, hover = { textColor = Color.yellow } };

        if (!editing)
        {
            if (myScript.optionsManager != null)
            {
                if (GUILayout.Button("Back to main options", GUILayout.Height(30f)))
                {
                    Selection.activeObject = myScript.optionsManager.gameObject;
                    myScript.optionsManager.setupStage = 2;
                }
            }
            if (GUILayout.Button("Edit", GUILayout.Height(30f)))
            {
                ActiveEditorTracker.sharedTracker.isLocked = true;
            }
        }
        else
        {
            if (GUILayout.Button("Finish Editing", GUILayout.Height(30f)))
            {
                ActiveEditorTracker.sharedTracker.isLocked = false;
                Selection.activeGameObject = myScript.gameObject;
                EditorUtility.SetDirty(myScript);
            }
            GUILayout.Space(5f);
            GUILayout.Label("EDIT MODE\n\n" + myScript.gameObject.name + "\nSelect Objects of type " + myScript.objectName + " Then click add selected to add them to the list\n", styleCenteredYellow);
            
            if (GUILayout.Button("Add Selected " + myScript.objectName + "s: ", GUILayout.Height(30f)))
            {
                ClearFilters();
                Undo.RecordObject(myScript, "Add " + myScript.objectName + "s: ");
                // If there is no current objects list, crerate a new one
                if (myScript.objects == null)
                    myScript.ResetObjects();

                foreach (Object obj in Selection.objects)
                {
                    if (myScript.IsValidObjectType(obj))
                    {
                        if (!myScript.ObjectExists(obj))
                            myScript.AddObject(obj);
                        else
                            Debug.Log(myScript.objectName + " already added");
                    }
                    else
                    {
                        Debug.Log("not a " + myScript.objectName + ": " + obj.name);
                    }
                }
                EditorUtility.SetDirty(myScript);
            }
            if (GUILayout.Button("Remove All", GUILayout.Height(30f)))
            {
                Undo.RecordObject(myScript, "Remove " + myScript.objectName + "s: ");
                myScript.ResetObjects();
                EditorUtility.SetDirty(myScript);
            }

            // filter section
            if (enabledFilters)
            {
                if (GUILayout.Button("Disable filters", GUILayout.Height(30f)))
                {
                    enabledFilters = false;
                    ClearFilters();
                }
                GUILayout.Label("== filters: ==", style, GUILayout.Height(30f));
                EditorGUILayout.LabelField("Quickly search through your added options with keywords and weights (value below 0 in weight will ignore this field)\n", style);

                if (GUILayout.Button("Clear filters", GUILayout.Height(30f)))
                    ClearFilters();
                filterByObject = EditorGUILayout.TextField("Object: ", filterByObject);
                filterByName = EditorGUILayout.TextField("Trait: ", filterByName);
                EditorGUILayout.BeginHorizontal();
                filterByWeight = EditorGUILayout.IntField("Weight: ", filterByWeight);
                operation = GUILayout.SelectionGrid(operation, operationOptions, 3);
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                if (GUILayout.Button("Enable filters", GUILayout.Height(30f)))
                {
                    enabledFilters = true;
                    GUI.FocusControl(null);
                }
            }
        }
       

        if (myScript.objects?.Count > 0)
        {
            DisplayAllOptions();
        }
        else
        {
            EditorGUILayout.LabelField("*No options. Click Edit button to start adding options", style);
        }
    }

    protected virtual void DisplayAllOptions()
    {
        GUILayout.Space(5f);
        GUILayout.Label("== " + myScript.objectName + "s ==", style);

        if (editing)
        {
            GUILayout.Label("\n*Trait name: final name of option exported in the json trait\n*Weight: How probably is for this option to get chosen from other options\n", style);
        }

        Titles();

        for (int i = 0; i < myScript.objects.Count; i++)
        {
            if (myScript.objects[i] == null)
                myScript.RemoveAtIndex(i);
            EditorGUILayout.BeginHorizontal();

            if (filterByObject == "" && filterByName != "" && filterByWeight < 0)
            {
                Option(i);
            }
            else
            {
                if (FilterValue(i))
                {
                    Option(i);
                }
            }


            EditorGUILayout.EndHorizontal();
        }
    }

    protected virtual void Titles()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(myScript.objectName, GUILayout.MinWidth(50f));
        EditorGUILayout.LabelField("Trait Name", GUILayout.Width(200f));
        EditorGUILayout.LabelField("Weight", GUILayout.Width(60f));
        EditorGUILayout.EndHorizontal();
    }
    protected virtual void Option(int index)
    {
        EditorGUILayout.ObjectField(myScript.objects[index], typeof(Object), true);

        if (editing)
        {
            string trait = EditorGUILayout.TextField(myScript.nameTraits[index], GUILayout.Width(160f));
            if (trait != myScript.nameTraits[index])
            {
                Undo.RecordObject(myScript, "Set Trait Name Value");
                myScript.nameTraits[index] = trait;
            }
            int weight = EditorGUILayout.IntField(myScript.weights[index], GUILayout.Width(40f));
            if (weight != myScript.weights[index])
            {
                Undo.RecordObject(myScript, "Set Weight Value");
                myScript.weights[index] = weight;
            }
            if (GUILayout.Button("x", GUILayout.Width(20f)))
            {
                Undo.RecordObject(myScript, "Remove Object");
                myScript.RemoveAtIndex(index);
            }
        }
        else
        {
            EditorGUILayout.LabelField(myScript.nameTraits[index], GUILayout.Width(200f));
            EditorGUILayout.LabelField(myScript.weights[index].ToString(), GUILayout.Width(40f));
        }
    }

    private void ClearFilters()
    {
        operation = 0;
        filterByWeight = -1;
        filterByName = "";
        filterByObject = "";
        GUI.FocusControl(null);
    }

    protected bool FilterValue(int index)
    {

        if (filterByObject != "")
        {
            if (!myScript.objects[index].name.ToLower().Contains(filterByObject.ToLower()))
                return false;
        }
        if (filterByName != "")
        {
            if (!myScript.nameTraits[index].ToLower().Contains(filterByName.ToLower()))
                return false;
        }
        if (filterByWeight != -1)
        {
            switch (operation)
            {
                case 0: // equals
                    if (filterByWeight != myScript.weights[index])
                        return false;
                    break;
                case 1: // smaller
                    if (myScript.weights[index] >= filterByWeight)
                        return false;
                    break;
                case 2: // greater
                    if (myScript.weights[index] <= filterByWeight)
                        return false;
                    break;
                default:
                    Debug.Log("invalid operation");
                    break;
            }
        }

        return true;
    }


}
