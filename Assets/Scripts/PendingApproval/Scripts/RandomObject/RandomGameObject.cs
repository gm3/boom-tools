using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGameObject : RandomObject
{
    public override void SetObjectName()
    {
        objectName = "GameObject";
    }

    public override bool IsValidObjectType(Object obj)
    {
        return obj.GetType() == typeof(GameObject);
    }
}
