using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ActionCaller : MonoBehaviour
{
    public RandomObject randomTarget;
    public string traitName = "";

    public Object selectedObject;
    public string selectedTrait;


    public void SetRandomTrait()
    {
        if (randomTarget != null)
        {
            selectedObject = randomTarget.GetRandomObject();
            selectedTrait = randomTarget.GetObjectTraitName();
            Action();
        }
        else
        {
            Debug.LogWarning("No random target set in script SetObjectsVisibility in: " + gameObject.name);
        }
    }
    protected virtual void Action()
    {
        //override//
        Debug.Log(selectedObject.name);
    }
    //set tu public to be able to se it in custom editor
    public virtual bool IsValidType()
    {
        return true;
    }
    protected virtual bool IsValidTrait()
    {
        return true;
    }
    public string GetJsonedObject(bool addEndComma, int tabulation = 0)
    {
        string tab = "";
        for (int i = 0; i < tabulation; i++)
            tab += "\t";

        if (IsValidTrait())
        {
            return
                tab + "{\n" +
                tab + "\t\"trait_type\":" + "\"" + traitName + "\",\n" +
                tab + "\t\"value\":" + "\"" + selectedTrait + "\"\n" +
                tab + "}" + (addEndComma ? ",\n":"");
        }
        return "";

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ActionCaller), true)]
public class ActionCaller_Editor : Editor
{
    ActionCaller myScript;

    private void OnEnable()
    {
        myScript = (ActionCaller)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (myScript.randomTarget != null)
            if (!myScript.IsValidType())
            {
                myScript.randomTarget = null;
                Debug.LogWarning("Not a valid type of script for targetRandom in: " + myScript.gameObject.name);
            }
    }
}

#endif
