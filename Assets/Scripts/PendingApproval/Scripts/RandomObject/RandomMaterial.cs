using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterial : RandomObject
{
    public override void SetObjectName()
    {
        objectName = "Material";
    }

    public override bool IsValidObjectType(Object obj)
    {
        return obj.GetType() == typeof(Material);
    }
}
