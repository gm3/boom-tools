using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomGameObject))]
public class RandomGameObject_Editor : RandomObject_Editor
{
    RandomGameObject goScript;
    protected override void OnEnable()
    {
        goScript = (RandomGameObject)target;
        base.OnEnable();
    }
    protected override void ValidateListSize()
    {
        base.ValidateListSize();
        if (goScript.objects != null)
        {
            if (goScript.parentName == null) goScript.parentName = new List<string>(goScript.objects.Count);

            if (goScript.parentName.Count != goScript.objects.Count)
            {
                for (int i = goScript.parentName.Count; i < goScript.objects.Count; i++)
                {
                    goScript.parentName.Add("");
                }

            }
        }
    }
    protected override void Titles()
    {
        if (!goScript.setNewParent) {
            if (GUILayout.Button("Enable reparenting", GUILayout.Height(30f)))
                goScript.setNewParent = true;
        }
        else
        {
            if (GUILayout.Button("Disable reparenting", GUILayout.Height(30f)))
                goScript.setNewParent = false;
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(goScript.objectName, GUILayout.MinWidth(50f));
        EditorGUILayout.LabelField("Trait Name", GUILayout.Width(100f));
        if (goScript.setNewParent) EditorGUILayout.LabelField("Parent Name", GUILayout.Width(100f));
        EditorGUILayout.LabelField("Weight", GUILayout.Width(60f));
        EditorGUILayout.EndHorizontal();
    }
    protected override void Option(int index)
    {
        EditorGUILayout.ObjectField(goScript.objects[index], typeof(Object), true);

        if (editing)
        {
            string trait = EditorGUILayout.TextField(goScript.nameTraits[index], GUILayout.Width(100f));
            if (trait != goScript.nameTraits[index])
            {
                Undo.RecordObject(goScript, "Set Trait Name Value");
                goScript.nameTraits[index] = trait;
            }
            if (goScript.setNewParent)
            {
                string parent = EditorGUILayout.TextField(goScript.parentName[index], GUILayout.Width(100f));
                if (parent != goScript.parentName[index])
                {
                    Undo.RecordObject(goScript, "Set Parent Name");
                    goScript.parentName[index] = parent;
                }
            }
            int weight = EditorGUILayout.IntField(goScript.weights[index], GUILayout.Width(40f));
            if (weight != goScript.weights[index])
            {
                Undo.RecordObject(goScript, "Set Weight Value");
                goScript.weights[index] = weight;
            }
            if (GUILayout.Button("x", GUILayout.Width(20f)))
            {
                Undo.RecordObject(goScript, "Remove Object");
                goScript.RemoveAtIndex(index);
            }
        }
        else
        {
            EditorGUILayout.LabelField(goScript.nameTraits[index], GUILayout.Width(100f));
            if (goScript.setNewParent)EditorGUILayout.LabelField(goScript.parentName[index] == "" ? "  -  " : goScript.parentName[index], GUILayout.Width(100f));
            EditorGUILayout.LabelField(goScript.weights[index].ToString(), GUILayout.Width(40f));
            EditorGUILayout.LabelField("", GUILayout.Width(20f));

        }
    }
}
