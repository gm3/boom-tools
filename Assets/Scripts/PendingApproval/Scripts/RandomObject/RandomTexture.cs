using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomTexture : RandomObject
{
    public List<float> smoothnessProperty;
    public List<float> metallicProperty;

    public bool setSmoothness = false;
    public bool setMetallic = false;

    public override void SetObjectName()
    {
        objectName = "Texture";
    }
    
    public override bool IsValidObjectType(Object obj)
    {
        return obj.GetType() == typeof(Texture2D);
    }
    public override void ResetObjects()
    {
        base.ResetObjects();
        smoothnessProperty = new List<float>();
        metallicProperty = new List<float>();
    }
    public override void AddObject(Object value)
    {
        base.AddObject(value);
        smoothnessProperty.Add(0);
        metallicProperty.Add(0);
    }

    public override void RemoveAtIndex(int index)
    {
        base.RemoveAtIndex(index);
        smoothnessProperty.RemoveAt(index);
        metallicProperty.RemoveAt(index);
    }

    
}



