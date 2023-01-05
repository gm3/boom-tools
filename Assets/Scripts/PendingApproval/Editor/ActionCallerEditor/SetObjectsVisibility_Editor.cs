using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VRM;

[CustomEditor(typeof(SetObjectsVisibility), true)]
public class SetObjectsVisibility_Editor : ActionCaller_Editor
{
    SetObjectsVisibility myScript;
    protected override void OnEnable()
    {
        base.OnEnable();
        myScript = (SetObjectsVisibility)target;
        if (myScript.optionsManager != null)
        {
            if (myScript.rootParentOnChosen == null)
                myScript.rootParentOnChosen = myScript.optionsManager.mainCharacterOptions;
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        // works only for manager
        if (myScript.optionsManager != null)
        {
            string newParentName = EditorGUILayout.TextField("Parent Name (optional): ", myScript.newParentName);
            if (newParentName != myScript.newParentName)
            {
                Undo.RecordObject(myScript, "Set New Parent Name");
                myScript.newParentName = newParentName;
            }
            //EditorGUILayout.ObjectField("Root On Parent", myScript.rootParentOnChosen,typeof(RandomGameObject),true);
        }
        // works for all
        if (!myScript.setbonesSkinToVRM)
        {
            if (GUILayout.Button("Enable reparent skin bones to vrm", GUILayout.Height(30f)))
                myScript.setbonesSkinToVRM = true;
        }
        else
        {
            if (GUILayout.Button("Disable reparent skin bones to vrm", GUILayout.Height(30f)))
                myScript.setbonesSkinToVRM = false;
        }

        if (GUILayout.Button("Add Blendshape Identifier", GUILayout.Height(30f))) myScript._AddBlendShape();
        if (myScript.blendShapes != null)
        {
            if (myScript.blendShapes.Count > 0)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
                EditorGUILayout.LabelField("= Non case sensitive =", style, GUILayout.Height(30f));
                EditorGUILayout.LabelField("*Leave empty text field to use shape name", style, GUILayout.Height(30f));
                EditorGUILayout.Space();

                for (int i = 0; i < myScript.blendShapes.Count; i++)
                {
                    BlendShapePreset shape = myScript.blendShapes[i];
                    string name = myScript.blendShapeNames[i];
                    EditorGUILayout.BeginHorizontal();
                    shape = (BlendShapePreset)EditorGUILayout.EnumPopup(shape);
                    name = EditorGUILayout.TextField(name);

                    if (name != myScript.blendShapeNames[i] || shape != myScript.blendShapes[i])
                    {
                        Undo.RecordObject(myScript, "Set blendshapes value");
                        myScript.blendShapeNames[i] = name;
                        myScript.blendShapes[i] = shape;
                    }

                    if (GUILayout.Button("X"))
                    {
                        Undo.RecordObject(myScript, "Remove Blendshape");
                        myScript._RemoveBlendshape(i);
                    }
                    EditorGUILayout.EndVertical();
                }
            }
        }
    }
}
