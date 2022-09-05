using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObject : MonoBehaviour
{
    public OptionsManager optionsManager;

    public List<Object> objects;
    public List<int> weights;
    public List<string> nameTraits;

    [HideInInspector]
    public int currentSelected = 0;

    public string objectName = "Object";

    public Object GetRandomObject()
    {
        currentSelected = GetRandomValue();
        return objects[currentSelected];
    }
    public int GetObjectWeight()
    {
        return weights[currentSelected];
    }
    public string GetObjectTraitName()
    {
        return nameTraits[currentSelected];
    }
    public Object GetSelectedObject()
    {
        return objects[currentSelected];
    }
    public Object GetObjectAt(int index)
    {
        return objects[index];
    }
    public virtual bool IsValidObjectType(Object obj)
    {
        return obj.GetType() == typeof(Object);
    }
    public virtual void SetObjectName()
    {
        objectName = "Object";
    }

    public void ResetObjects()
    {
        objects = new List<Object>();
        weights = new List<int>();
        nameTraits = new List<string>();
    }
    public void AddObject(Object value)
    {
        objects.Add(value);
        weights.Add(1);
        nameTraits.Add(value.name);
    }
    
    protected int GetRandomValue()
    {
        return Random.Range(0, objects.Count);
    }
    public void RemoveAtIndex(int index)
    {
        weights.RemoveAt(index);
        nameTraits.RemoveAt(index);
        objects.RemoveAt(index);
        
    }
    public bool ObjectExists(Object obj)
    {
        foreach (Object o in objects)
        {
            if (o == obj)
            {
                return true;
            }
        }
        return false;
    }
}

