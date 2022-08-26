using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class RandomObject : MonoBehaviour
{
    public List<Object> objects;
    public List<int> weights;
    public List<string> nameTraits;

    [HideInInspector]
    public int currentSelected = 0;

    public string objectName = "Object";

    public Object GetRandomObject()
    {
        currentSelected = GetRandomValue();
        return objects[currentSelected];
    }
    public int GetObjectWeight()
    {
        return weights[currentSelected];
    }
    public string GetObjectTraitName()
    {
        return nameTraits[currentSelected];
    }
    public virtual bool IsValidObjectType(Object obj)
    {
        return obj.GetType() == typeof(Object);
    }
    public virtual void SetObjectName()
    {
        objectName = "Object";
    }

    public void ResetObjects()
    {
        objects = new List<Object>();
        weights = new List<int>();
        nameTraits = new List<string>();
    }
    public void AddObject(Object value)
    {
        objects.Add(value);
        weights.Add(1);
        nameTraits.Add(value.name);
    }
    public void RemoveAtIndex(int index)
    {
        objects.RemoveAt(index);
        weights.RemoveAt(index);
        nameTraits.RemoveAt(index);
    }
    protected int GetRandomValue()
    {
        return Random.Range(0, objects.Count);
    }

    public bool ObjectExists(Object obj)
    {
        foreach (Object o in objects)
        {
            if (o == obj)
            {
                return true;
            }
        }
        return false;
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(RandomObject),true)]
public class RandomObject_Editor : Editor
{
    RandomObject myScript;

    string filterByObject = "";
    string filterByName = "";
    int filterByWeight = -1;
    int operation = 0;
    string[] operationOptions;
    private void OnEnable()
    {
        myScript = (RandomObject)target;
        myScript.SetObjectName();
        ValidateListSize();
        operationOptions = new string[3];
        operationOptions[0] = "equals";
        operationOptions[1] = "smaller";
        operationOptions[2] = "greater";

    }
    private void ValidateListSize()
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
        bool editing = ActiveEditorTracker.sharedTracker.isLocked;
        GUIStyle style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        if (!editing)
        {
            if (GUILayout.Button("Edit " + myScript.objectName + "s: ", GUILayout.Height(30f)))
            {
                ActiveEditorTracker.sharedTracker.isLocked = true;
            }
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("EDITING: " + myScript.gameObject.name, style, GUILayout.Height(30f));
            if (GUILayout.Button("Finish Editing", GUILayout.Height(30f)))
            {
                ActiveEditorTracker.sharedTracker.isLocked = false;
                Selection.activeGameObject = myScript.gameObject;
                EditorUtility.SetDirty(myScript);
            }
            EditorGUILayout.EndHorizontal();
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
                        Debug.Log("not a "+ myScript.objectName +": " + obj.name);
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
            
            if (GUILayout.Button("Random Example", GUILayout.Height(30f)))
            {
                Object obj = myScript.GetRandomObject();
                Debug.Log(myScript.currentSelected);
                Debug.Log("Type: " + obj.GetType().ToString() + ", Object name: " + obj.name + ", " + "Trait name: " + myScript.GetObjectTraitName() + ", Weight: " + myScript.GetObjectWeight().ToString());
            }

            // filter section
            GUILayout.Label("== filters: ==", style, GUILayout.Height(30f));
            if (GUILayout.Button("Clear filters", GUILayout.Height(30f)))
                ClearFilters();
            filterByObject = EditorGUILayout.TextField("Object: ", filterByObject);
            filterByName = EditorGUILayout.TextField("Trait: ", filterByName);
            EditorGUILayout.BeginHorizontal();
            filterByWeight = EditorGUILayout.IntField("Weight: ", filterByWeight);
            operation = GUILayout.SelectionGrid(operation, operationOptions, 3);
            EditorGUILayout.EndHorizontal();
        }
        GUILayout.Label("== " + myScript.objectName + " ==", style, GUILayout.Height(30f));
        
        if (myScript.objects != null)
        {
            
            EditorGUILayout.LabelField("object / trait name / weight");

            for (int i = 0; i < myScript.objects.Count; i++)
            {
                

                EditorGUILayout.BeginHorizontal();
                int weight = myScript.weights[i];
                string trait = myScript.nameTraits[i];

                if (filterByObject == "" && filterByName != "" && filterByWeight < 0)
                {
                    EditorGUILayout.ObjectField(myScript.objects[i], typeof(Object), true);
                   
                    if (editing)
                    {
                        trait = EditorGUILayout.TextField(myScript.nameTraits[i], GUILayout.Width(200f));
                        weight = EditorGUILayout.IntField(myScript.weights[i], GUILayout.Width(40f));
                        if (GUILayout.Button("x", GUILayout.Width(20f)))
                        {
                            Undo.RecordObject(myScript, "Remove Object");
                            myScript.RemoveAtIndex(i);
                        }
                    }
                    else
                    {
                        EditorGUILayout.LabelField(myScript.nameTraits[i], GUILayout.Width(200f));
                        EditorGUILayout.LabelField(myScript.weights[i].ToString(), GUILayout.Width(40f));
                    }

                    // delete button

                }
                else
                {
                    if (FilterValue(i))
                    {
                        EditorGUILayout.ObjectField(myScript.objects[i], typeof(Object), true);

                        if (editing)
                        {
                            trait = EditorGUILayout.TextField(myScript.nameTraits[i], GUILayout.Width(200f));
                            weight = EditorGUILayout.IntField(myScript.weights[i], GUILayout.Width(40f));
                            if (GUILayout.Button("x", GUILayout.Width(20f)))
                            {
                                Undo.RecordObject(myScript, "Remove Object");
                                myScript.RemoveAtIndex(i);
                            }
                        }
                        else
                        {
                            EditorGUILayout.LabelField(myScript.nameTraits[i], GUILayout.Width(200f));
                            EditorGUILayout.LabelField(myScript.weights[i].ToString(), GUILayout.Width(40f));
                        }

                        // delete button
                    }
                }
                if (weight != myScript.weights[i])
                {
                    Undo.RecordObject(myScript, "Set Weight Value");
                    myScript.weights[i] = weight;
                }
                if (trait  != myScript.nameTraits[i])
                {
                    Undo.RecordObject(myScript, "Set Trait Name Value");
                    myScript.nameTraits[i] = trait;
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
    private void ClearFilters()
    {
        operation = 0;
        filterByWeight = -1;
        filterByName = "";
        filterByObject = "";
    }
    private bool FilterValue(int index)
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

#endif
