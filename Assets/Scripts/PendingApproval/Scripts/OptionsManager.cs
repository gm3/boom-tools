using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    public List<GameObject> randomObjects;
    public List<GameObject> actionCallers;

    public RandomGameObject mainVRMStructure;

    public GameObject optionsHolder;
    public GameObject actionsHolder;

    public int setupStage = 0;
    // random objects
    public GameObject AddRandomObjectOption(System.Type type, string name = "")
    {
        if (type.IsSubclassOf(typeof(RandomObject)))
        {
            if (optionsHolder == null) CreateOptionsHolder();   
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
    private void CreateOptionsHolder()
    {
        optionsHolder = new GameObject("options");
        optionsHolder.transform.SetParent(transform);
    }

    public void RemoveRandomObjectOption(int index)
    {
        if (randomObjects[index] != null)
            DestroyImmediate(randomObjects[index]);    
        randomObjects.RemoveAt(index);
    }



    // action callers
    public GameObject AddActionCaller(System.Type type,string name = "", string traitName = "")
    {
        if (type.IsSubclassOf(typeof(ActionCaller)))
        {
            if (actionsHolder == null) CreateActionsHolder();
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
    private void CreateActionsHolder()
    {
        actionsHolder = new GameObject("actions");
        actionsHolder.transform.SetParent(transform);
    }
    public void RemoveActionCaller(int index)
    {
        if (actionCallers[index] != null)
            DestroyImmediate(actionCallers[index]);
        actionCallers.RemoveAt(index);
    }





    //
    public void SetMainVRM()
    {
        Debug.Log("set main action caller vrm");
    }
}
