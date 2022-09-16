using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperRules : MonoBehaviour
{
    [HideInInspector]
    public OptionsManager optionsManager;
    //this is the set of objects that will check for coincidence
    public RandomObject randomObject;
    // this are all the options from the previous set
    public List<Object> targetOptions;
    public List<string> optionsTraitName;
    // should we consider all options from before?, or only at least 1
    //public bool anyOption = true;
    public bool beChosen = true;

    // this action will enable/disable ONLY if rules are fullfilled
    public List<ActionCaller> disableActions;
    public void ApplyRule()
    {
        if (IsActiveRule())
        {
            Rule();
        }
    }
    protected virtual void Rule()
    {
        
        if (beChosen)
        {
            foreach (Object ob in targetOptions)
                if (ob == randomObject.GetSelectedObject())
                    DisableActions();
        }
        else
        {
            bool notChosen = true;
            foreach (Object ob in targetOptions)
                if (ob == randomObject.GetSelectedObject())
                    notChosen = false;
            if (notChosen)
                DisableActions();
        }
        /* override */
    }
    private void DisableActions()
    {
        foreach (ActionCaller ac in disableActions)
        {
            ac.DisableByRule();
        }
    }

    public void AddAction(ref List<ActionCaller> list, ActionCaller ac)
    {
        if (list == null) list = new List<ActionCaller>();
        list.Add(ac);
    }
    public void RemoveActionAt(ref List<ActionCaller> list, int index)
    {
        if (list != null) list.RemoveAt(index);
    }



    public void ResetOptions()
    {
        targetOptions = new List<Object>();
        optionsTraitName = new List<string>();
    }
    public void AddOption(Object obj, string traitName)
    {
        if (targetOptions == null) ResetOptions();
        targetOptions.Add(obj);
        optionsTraitName.Add(traitName);
    }
    public void RemoveOption(int index)
    {
        if (targetOptions != null)
        {
            targetOptions.RemoveAt(index);
            optionsTraitName.RemoveAt(index);
        }
    }

    public bool IsActiveRule()
    {
        if (randomObject == null)
            return false;

        if (targetOptions== null)
            return false;
        if (targetOptions.Count == 0)
            return false;

        if (disableActions == null)
            return false;
        if (disableActions.Count == 0)
            return false;
        return true;
        
    }
}
