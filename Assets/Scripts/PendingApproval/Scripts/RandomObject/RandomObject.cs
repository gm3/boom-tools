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

    public virtual bool HasCorrectSetup()
    {
        if (objects == null)
            return false;
        if (objects.Count == 0)
            return false;
        return true;
    }
    public Object GetRandomObject()
    {
        if (objects.Count == 0)
            return null;

        currentSelected = GetRandomValue();
        if (objects[currentSelected] == null)
        {
            objects.RemoveAt(currentSelected);
            return GetRandomObject();
        }
        return objects[currentSelected];
    }
    public void SetCurrentSelected(int value)
    {
        currentSelected = value;
    }
    public int GetObjectWeight()
    {
        if (currentSelected < 0) return -1;
        return weights[currentSelected];
    }
    public string GetObjectTraitName()
    {
        if (currentSelected < 0) return "";
        return nameTraits[currentSelected];
    }
    public Object GetSelectedObject()
    {
        if (currentSelected < 0) return null;
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

    public virtual void ResetObjects()
    {
        objects = new List<Object>();
        weights = new List<int>();
        nameTraits = new List<string>();
    }
    public virtual void AddObject(Object value)
    {
        objects.Add(value);
        weights.Add(1);
        nameTraits.Add(value.name);
    }
    
    protected int GetRandomValue()
    {
        return Random.Range(0, objects.Count);
    }
    public virtual void RemoveAtIndex(int index)
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

