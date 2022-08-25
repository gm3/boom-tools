using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectsVisibility : ActionCaller
{
    public GameObject[] SetOnNewParentIfActive;
    private Transform lastObject = null;
    private Transform lastObjectParent = null;

    protected override void Action()
    {
        if (selectedObject != null)
            DisplayObject(selectedObject as GameObject);

    }
    private void DisplayObject(GameObject obj)
    {
        ResetParentPosition();
        foreach (GameObject go in randomTarget.objects)
        {
            go.SetActive(false);
        }
        obj.SetActive(true);
        if (SetOnNewParentIfActive.Length != 0)
            SetNewParent(obj);

        
    }
    private void ResetParentPosition()
    {
        if (lastObject != null)
        {
            lastObject.SetParent(lastObjectParent);
            lastObject = null;
            lastObjectParent = null;
        }
    }
    private void SaveParentPosition(Transform target)
    {
        lastObject = target;
        lastObjectParent = target.parent;
    }
    private void SetNewParent(GameObject obj)
    {
        
        for (int i =0; i < SetOnNewParentIfActive.Length; i++)
        {
            if (SetOnNewParentIfActive[i] != null)
            {
                if (SetOnNewParentIfActive[i].activeInHierarchy)
                {
                    SaveParentPosition(obj.transform);
                    obj.transform.SetParent(SetOnNewParentIfActive[i].transform);
                    break;
                }
            }

        }
    }
    public override bool IsValidType()
    {
        if (randomTarget.GetType() != typeof(RandomGameObject))
            return false;
        return base.IsValidType();
    }
    protected override bool IsValidTrait()
    {
        GameObject go = (GameObject)selectedObject;
        if (go.activeInHierarchy)
            return true;
        return false;
    }
}
