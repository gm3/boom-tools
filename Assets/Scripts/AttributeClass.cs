using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttributeClass
{
    public string trait_type;
    public string value;
}

[System.Serializable]
public class TraitsToLoad
{
    public string description;
    public string external_url;
    public string image;
    public string name;
    public AttributeClass[] attributes;
}