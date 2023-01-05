using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class RenameChilds : MonoBehaviour
{
    public bool removeLastChars = false;
    public int quantity = 0;
    private void OnEnable()
    {
        if (removeLastChars)
        {
            Transform[] childs = GetComponentsInChildren<Transform>();
            foreach (Transform t in childs)
            {
                if (t.gameObject.name.Contains("."))
                    t.gameObject.name = t.gameObject.name.Substring(0, t.gameObject.name.Length - quantity);
            }
        }
    }
}
