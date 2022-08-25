using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomTexture : RandomObject
{
    public override void SetObjectName()
    {
        objectName = "Texture";
    }
    public override bool IsValidObjectType(Object obj)
    {
        return obj.GetType() == typeof(Texture2D);
    }
}



