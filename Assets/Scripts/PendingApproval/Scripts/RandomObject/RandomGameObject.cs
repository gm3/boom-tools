using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGameObject : RandomObject
{
    public bool setNewParent = false;
    public List<string> parentName;
    public List<bool> isReadable;
    public override void SetObjectName()
    {
        objectName = "GameObject";
    }
    public override bool HasCorrectSetup()
    {
        if (isReadable != null)
        {
            foreach(bool bo in isReadable)
            {
                if (!bo)
                    return false;
            }
        }

        return base.HasCorrectSetup();
    }

    public override bool IsValidObjectType(Object obj)
    {
        return obj.GetType() == typeof(GameObject);
    }
    public override void ResetObjects()
    {
        base.ResetObjects();
        parentName = new List<string>();
        isReadable = new List<bool>();
    }
    public override void RemoveAtIndex(int index)
    {
        base.RemoveAtIndex(index);
        parentName.RemoveAt(index);
        isReadable.RemoveAt(index);
    }
    public override void AddObject(Object value)
    {
        base.AddObject(value);
        parentName.Add("");
        isReadable.Add(true);
    }
}
