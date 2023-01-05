using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    public List<GameObject> randomObjects;
    public List<GameObject> actionCallers;
    public List<GameObject> superRules;

    public RandomGameObject mainCharacterOptions;
    public SetObjectsVisibility mainCharacterAction;

    public RandomGameObject mainVRMStructure;

    public GameObject optionsHolder;
    public GameObject actionsHolder;
    public GameObject characterHolder;
    public GameObject rulesHolder;

    public int setupStage = 0;

    public DNAManager dnaManager;
    // random objects
    public GameObject AddRandomObjectOption(System.Type type, string name = "")
    {
        if (type.IsSubclassOf(typeof(RandomObject)))
        {
            if (optionsHolder == null) CreateHolder(ref optionsHolder, "options");   
            if (randomObjects == null) randomObjects = new List<GameObject>();
            
            GameObject newObj = new GameObject(name);
            newObj.transform.parent = optionsHolder.transform;
            newObj.AddComponent(type).GetComponent<RandomObject>().optionsManager = this;
            randomObjects.Add(newObj);

            return newObj;
        }
        else
        {
            Debug.LogWarning("Not a valid random object type: " + type);
            return null;
        }
    }






    // action callers
    public GameObject AddActionCaller(System.Type type,string name = "", string traitName = "")
    {
        if (type.IsSubclassOf(typeof(ActionCaller)))
        {
            if (actionsHolder == null) CreateHolder(ref actionsHolder, "actions");
            if (actionCallers == null) actionCallers = new List<GameObject>();

            GameObject newObj = new GameObject(name);
            newObj.transform.parent = actionsHolder.transform;
            newObj.AddComponent(type);
            ActionCaller newAction = newObj.GetComponent<ActionCaller>();
            newAction.optionsManager = this;
            newAction.traitName = traitName;

            actionCallers.Add(newObj);
            return newObj;
        }
        else
        {
            Debug.LogWarning("Not a valid action caller type: " + type);
            return null;
        }
    }

    public GameObject AddRule(string name)
    {
        if (rulesHolder == null) CreateHolder(ref rulesHolder, "rules");
        if (superRules == null) superRules = new List<GameObject>();

        GameObject newObj = new GameObject(name);
        newObj.transform.parent = rulesHolder.transform;
        newObj.AddComponent(typeof(SuperRules));
        SuperRules newRule = newObj.GetComponent<SuperRules>();
        newRule.optionsManager = this;

        superRules.Add(newObj);
        return newObj;
    }

    private void CreateHolder (ref GameObject storeGo, string name)
    {
        storeGo = new GameObject(name);
        storeGo.transform.SetParent(transform);
    }

    public void RemoveElement(ref List<GameObject> list, int index)
    {
        if (list != null)
        {
            if (list[index] != null)
            {
                DestroyImmediate(list[index]);
            }
            list.RemoveAt(index);
        }
    }

    public void RemoveActionCaller(int index)
    {
        if (actionCallers[index] != null)
            DestroyImmediate(actionCallers[index]);
        actionCallers.RemoveAt(index);
    }


    public List<ActionCaller> GetActionCallersOfType(System.Type type)
    {
        List<ActionCaller> result = new List<ActionCaller>();
        foreach (GameObject go in actionCallers)
        {
            if (go != null)
            {
                ActionCaller caller = go.GetComponent(type) as ActionCaller;
                if (caller != null)
                    result.Add(caller);

            }
        }
        return result;
    }

    public List<RandomObject> GetRandomObjectOfType(System.Type type)
    {
        List<RandomObject> result = new List<RandomObject>();
        foreach (GameObject go in randomObjects)
        {
            if (go != null)
            {
                RandomObject rand = go.GetComponent(type) as RandomObject;
                if (rand != null)
                    result.Add(rand);

            }
        }
        return result;
    }

    //
    public void SetMainVRM()
    {
        Debug.Log("set main action caller vrm");
    }

    public void CreateBasicCharacterSetup()
    {

        if (characterHolder == null)
        {
            characterHolder = new GameObject("character");
            characterHolder.transform.parent = transform;
        }

        if (mainCharacterOptions == null) 
        {
            mainCharacterOptions = characterHolder.AddComponent<RandomGameObject>();
            mainCharacterOptions.ResetObjects();
        }

        if (mainCharacterAction == null)
        {
            mainCharacterAction = characterHolder.AddComponent<SetObjectsVisibility>();
            mainCharacterAction.randomTarget = mainCharacterOptions;
            mainCharacterAction.traitName = "body";
        }
  
    }


    public void AttachDataToDNA(DNAManager dna)
    {
        ConfigureRandomizer randomizer = dna.transform.parent.GetComponentInChildren<ConfigureRandomizer>();
        randomizer.exportVRMFromRandomTrait = mainCharacterAction;

        RandomizeAll randomizeAll = dna.transform.parent.GetComponentInChildren<RandomizeAll>();
        randomizeAll.exportVRMFromRandomTrait = mainCharacterAction;
        randomizeAll.parentRandomTraitCaller = gameObject;

    }
}
