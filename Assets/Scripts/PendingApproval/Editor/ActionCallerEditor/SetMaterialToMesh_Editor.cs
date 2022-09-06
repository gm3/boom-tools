using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SetMaterialToMesh), true)]
public class SetMaterialToMesh_Editor : ActionCaller_Editor
{
    SetMaterialToMesh myScript;
    protected override void OnEnable()
    {
        base.OnEnable();
        myScript = (SetMaterialToMesh)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (myScript.optionsManager != null)
        {
            bool hasRend = myScript.HasRenderer();
            if (hasRend)
            {

                for (int i = 0; i < myScript.targetRenderers.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.ObjectField("Renderer: " + (i + 1).ToString(), myScript.targetRenderers[i], typeof(Renderer), true);
                    if (GUILayout.Button("X", GUILayout.Width(20f)))
                    {
                        Undo.RecordObject(myScript, "Remove Renderer");
                        myScript.RemoveRendererAt(i);
                    }
                    EditorGUILayout.EndHorizontal();
                }

            }
            string targetString = (hasRend ? "Additional Renderer: " : "Target Renderer: ");
            Renderer selRend = null;
            Renderer rend = (Renderer)EditorGUILayout.ObjectField(targetString, selRend, typeof(Renderer), true);
            if (rend != selRend)
            {
                Undo.RecordObject(myScript, "Add Renderer To Array");
                myScript.AddRenderer(rend);
            }
        }
    }
}
