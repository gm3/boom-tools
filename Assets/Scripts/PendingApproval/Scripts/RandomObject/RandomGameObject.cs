using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGameObject : RandomObject
{
    public bool setNewParent = false;
    public List<string> parentName;
    public override void SetObjectName()
    {
        objectName = "GameObject";
    }

    public override bool IsValidObjectType(Object obj)
    {
        return obj.GetType() == typeof(GameObject);
    }
    public override void ResetObjects()
    {
        base.ResetObjects();
        parentName = new List<string>();
    }
    public override void RemoveAtIndex(int index)
    {
        base.RemoveAtIndex(index);
        parentName.RemoveAt(index);
    }
    public override void AddObject(Object value)
    {
        base.AddObject(value);
        parentName.Add("");
    }
}
