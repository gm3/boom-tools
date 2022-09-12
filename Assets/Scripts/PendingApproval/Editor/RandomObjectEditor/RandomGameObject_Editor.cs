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
            if (goScript.isReadable == null) goScript.isReadable = new List<bool>(goScript.objects.Count);

            if (goScript.parentName.Count != goScript.objects.Count)
            {
                for (int i = goScript.parentName.Count; i < goScript.objects.Count; i++)
                {
                    goScript.parentName.Add("");
                }

            }
            if (goScript.isReadable.Count != goScript.objects.Count)
            {
                for (int i = goScript.isReadable.Count; i < goScript.objects.Count; i++)
                {
                    goScript.isReadable.Add(true);
                }

            }
        }
        ValidateReadWriteEnabled();
    }
    protected override void PostOptions()
    {
        if (!AllReadable())
        {
            EditorGUILayout.LabelField("*Imported Objects must have read write enable in order to export them for vrm.\n", BoomToolsGUIStyles.CustomLabel(true, true, true));
            if (GUILayout.Button("Make All Read Write Enabled", GUILayout.Height(30f)))
            {
                FixAllRenderers ();
            }
        }
    }
    private bool AllReadable()
    {
        foreach(bool bo in goScript.isReadable)
        {
            if (!bo)
                return false;
        }
        return true;
    }
    private void ValidateReadWriteEnabled()
    {
        if (goScript.objects != null)
        {
            for (int i = 0; i < goScript.objects.Count; i++)
            {
                goScript.isReadable[i] = isOptionReadable(goScript.objects[i] as GameObject);
            }
        }
    }
    private bool isOptionReadable(GameObject go)
    {
        
        SkinnedMeshRenderer[] skinRends = go.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        foreach (SkinnedMeshRenderer sr in skinRends)
        {
            if (sr != null)
            {
                if (!sr.sharedMesh.isReadable)
                {
                    return false;
                }
            }
        }
        MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>(true);
        foreach (MeshFilter mf in meshFilters)
        {
            if (mf != null)
            {
                if (!mf.sharedMesh.isReadable)
                {
                    return false;
                }
            }
        }
        return true;
    }
    protected override void Titles()
    {
        EditorGUILayout.LabelField("*Reparenting will set a new parent to the selected option, use it when an option needs to be parented to a specific bone (hand, feet, head, etc...)", BoomToolsGUIStyles.CustomLabel(true,false,true));
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
        EditorGUILayout.LabelField("Read", GUILayout.Width(40f));
        EditorGUILayout.LabelField("Trait Name", GUILayout.Width(100f));
        if (goScript.setNewParent) EditorGUILayout.LabelField("Parent Name", GUILayout.Width(100f));
        EditorGUILayout.LabelField("Weight", GUILayout.Width(60f));
        EditorGUILayout.EndHorizontal();
    }

    protected override void OnAddObjectClick()
    {
        ValidateReadWriteEnabled ();
    }
    private void FixAllRenderers()
    {
        for (int i =0; i < goScript.objects.Count; i++)
        {
            if (!goScript.isReadable[i])
            {
                FixReadableRenderer(i);
            }
        }
    }
    private void FixReadableRenderer(int index)
    {
        GameObject go = goScript.objects[index] as GameObject;
        Renderer[] rends = go.GetComponentsInChildren<Renderer>(true);

        foreach (Renderer rend in rends)
        {
            string path = AssetDatabase.GetAssetPath(PrefabUtility.GetCorrespondingObjectFromOriginalSource(rend));

            ModelImporter importer = ModelImporter.GetAtPath(path) as ModelImporter;

            if (importer != null)
            {
                Debug.Log("fixing");
                importer.isReadable = true;
                AssetDatabase.ImportAsset(path);
            }
        }

        goScript.isReadable[index] = isOptionReadable(go);
    }
    protected override void Option(int index)
    {
        EditorGUILayout.ObjectField(goScript.objects[index], typeof(Object), true);

        if (editing)
        {
            if (goScript.isReadable[index])
            {
                EditorGUILayout.LabelField("Yes", BoomToolsGUIStyles.CustomColorLabel(true, false, false, Color.green), GUILayout.Width(40f));
            }
            else
            {
                if(GUILayout.Button("Fix", GUILayout.Width(40f))) FixReadableRenderer(index);
            }
            
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
            EditorGUILayout.LabelField(goScript.isReadable[index] ? "Yes":"No", goScript.isReadable[index] ? 
                BoomToolsGUIStyles.CustomColorLabel(true, false, false, Color.green):
                BoomToolsGUIStyles.CustomColorLabel(true, false, false, Color.red), GUILayout.Width(40f));
            EditorGUILayout.LabelField(goScript.nameTraits[index], GUILayout.Width(100f));
            if (goScript.setNewParent)EditorGUILayout.LabelField(goScript.parentName[index] == "" ? "  -  " : goScript.parentName[index], GUILayout.Width(100f));
            EditorGUILayout.LabelField(goScript.weights[index].ToString(), GUILayout.Width(40f));
            EditorGUILayout.LabelField("", GUILayout.Width(20f));

        }
    }
}
