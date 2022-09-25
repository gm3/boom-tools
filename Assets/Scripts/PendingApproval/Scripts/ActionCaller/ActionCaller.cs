using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCaller : MonoBehaviour
{
    [HideInInspector]
    public OptionsManager optionsManager;

    public RandomObject randomTarget;
    public string traitName = "";

    [HideInInspector]
    public Object selectedObject;
    [HideInInspector]
    public string selectedTrait;
    [HideInInspector]
    public bool selectionMode = false;

    public bool enableAction = true;

    public void SetPreSetup()
    {
        if (randomTarget != null)
            PreAction();
    }
    public void SetPostSetup()
    {
        if (randomTarget != null)
            PostAction();
    }
    public void SetAction()
    {
        if (randomTarget != null)
            Action();
    }
    public void DisableByRule()
    {
        enableAction = false;
    }
    public void SetRandomTrait()
    {
        enableAction = true;
        if (randomTarget != null)
        {
            selectedObject = randomTarget.GetRandomObject();
            selectedTrait = randomTarget.GetObjectTraitName();
        }
        else
        {
            Debug.LogWarning("No random target set in script SetObjectsVisibility in: " + gameObject.name);
        }
    }
    public void SetTargetTrait(int value)
    {
        enableAction = true;
        if (randomTarget != null)
        {
            randomTarget.SetCurrentSelected(value);
            selectedObject = randomTarget.GetSelectedObject();
            selectedTrait = randomTarget.GetObjectTraitName();
        }
        else
        {
            Debug.LogWarning("No random target set in script SetObjectsVisibility in: " + gameObject.name);
        }
    }
    protected virtual void PreAction()
    {
        //override//
    }
    protected virtual void PostAction()
    {
        //override//
    }
    protected virtual void Action()
    {
        //override//
    }

    /// <summary>
    /// Returns the Type of option that can be assigned to this class.
    /// </summary>
    /// <returns></returns>
    public virtual System.Type GetRandomObjectValidType()
    {
        return typeof(RandomObject); //generic
    }
    public virtual bool IsValidType()
    {
        return true;
    }
    /// <summary>
    /// Does this trait is something that is displayed in the scene?
    /// </summary>
    /// <returns></returns>
    protected virtual bool IsActiveTrait()
    {
        return true;
    }
    /// <summary>
    /// Does this trait has the basic setup to work?
    /// </summary>
    /// <returns></returns>
    public virtual bool IsValidTrait()
    {
        if (randomTarget == null)
            return false;
        return true;
    }
    public List<Object> GetExtraData()
    {
        if (IsActiveTrait())
            return FetchExtraData();
        else
            return new List<Object>();
    }
    protected virtual List<Object> FetchExtraData()
    {
        return new List<Object>();
    }
    public string GetJsonedObject(bool addEndComma, int tabulation = 0)
    {
        string tab = "";
        for (int i = 0; i < tabulation; i++)
            tab += "\t";

        if (IsActiveTrait())
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

