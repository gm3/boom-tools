using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomTexture))]
public class RandomTexture_Editor : RandomObject_Editor
{
    RandomTexture txrScript;
    protected override void OnEnable()
    {
        txrScript = (RandomTexture)target;
        base.OnEnable();
    }
    protected override void ValidateListSize()
    {
        base.ValidateListSize();

        if (txrScript.metallicProperty == null) txrScript.metallicProperty = new List<float>(txrScript.objects.Count);
        if (txrScript.smoothnessProperty == null) txrScript.smoothnessProperty = new List<float>(txrScript.objects.Count);

        if (txrScript.metallicProperty.Count != txrScript.objects.Count)
        {
            for (int i = txrScript.metallicProperty.Count; i < txrScript.objects.Count; i++)
            {
                txrScript.metallicProperty.Add(0f);
            }

        }
        if (txrScript.smoothnessProperty.Count != txrScript.objects.Count)
        {
            for (int i = txrScript.smoothnessProperty.Count; i < txrScript.objects.Count; i++)
            {
                txrScript.smoothnessProperty.Add(0f);
            }
        }
    }
    protected override void Titles()
    {
        EditorGUILayout.BeginHorizontal();
        if (!txrScript.setMetallic)
        {
            if (GUILayout.Button("Enable Metallic",GUILayout.Height(30f)))
                txrScript.setMetallic = true;
        }
        else
        {
            if (GUILayout.Button("Disable Metallic", GUILayout.Height(30f)))
                txrScript.setMetallic = false;
        }

        if (!txrScript.setSmoothness)
        {
            if (GUILayout.Button("Enable Smoothness", GUILayout.Height(30f)))
                txrScript.setSmoothness = true;
        }
        else
        {
            if (GUILayout.Button("Disable Smoothness", GUILayout.Height(30f)))
                txrScript.setSmoothness = false;
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(txrScript.objectName, GUILayout.MinWidth(50f));
        EditorGUILayout.LabelField("Trait Name", GUILayout.Width(140f));
        if (txrScript.setMetallic) EditorGUILayout.LabelField("Metal", GUILayout.Width(40f));
        if (txrScript.setSmoothness) EditorGUILayout.LabelField("Smooth", GUILayout.Width(40f));
        EditorGUILayout.LabelField("Weight", GUILayout.Width(40f));
        if (editing)EditorGUILayout.LabelField("", GUILayout.Width(20f));
        EditorGUILayout.EndHorizontal();
    }
    protected override void Option(int index)
    {
        EditorGUILayout.ObjectField(txrScript.objects[index], typeof(Object), true);

        if (editing)
        {
            string trait = EditorGUILayout.TextField(txrScript.nameTraits[index], GUILayout.Width(140f));
            if (trait != txrScript.nameTraits[index])
            {
                Undo.RecordObject(txrScript, "Set Trait Name Value");
                txrScript.nameTraits[index] = trait;
            }
            if (txrScript.setMetallic)
            {
                float metal = EditorGUILayout.FloatField(txrScript.metallicProperty[index], GUILayout.Width(40f));
                if (metal > 1f) metal = 1f;
                if (metal < 0f) metal = 0f;
                if (metal != txrScript.metallicProperty[index])
                {
                    Undo.RecordObject(txrScript, "Set Metal Value");
                    txrScript.metallicProperty[index] = metal;
                }
            }

            if (txrScript.setSmoothness)
            {
                float smooth = EditorGUILayout.FloatField(txrScript.smoothnessProperty[index], GUILayout.Width(40f));
                if (smooth > 1f) smooth = 1f;
                if (smooth < 0f) smooth = 0f;
                if (smooth != txrScript.smoothnessProperty[index])
                {
                    Undo.RecordObject(txrScript, "Set Smooth Value");
                    txrScript.smoothnessProperty[index] = smooth;
                }
            }

            int weight = EditorGUILayout.IntField(txrScript.weights[index], GUILayout.Width(40f));
            if (weight != txrScript.weights[index])
            {
                Undo.RecordObject(txrScript, "Set Weight Value");
                txrScript.weights[index] = weight;
            }
            if (GUILayout.Button("x", GUILayout.Width(20f)))
            {
                Undo.RecordObject(txrScript, "Remove Object");
                txrScript.RemoveAtIndex(index);
            }
        }
        else
        {
            EditorGUILayout.LabelField(txrScript.nameTraits[index], GUILayout.Width(140f));
            if (txrScript.setMetallic) EditorGUILayout.LabelField(txrScript.metallicProperty[index].ToString(), GUILayout.Width(40f));
            if (txrScript.setSmoothness) EditorGUILayout.LabelField(txrScript.smoothnessProperty[index].ToString(), GUILayout.Width(40f));
            EditorGUILayout.LabelField(txrScript.weights[index].ToString(), GUILayout.Width(40f));
        }
    }
}
