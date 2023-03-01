using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseMatchTheBorder : MonoBehaviour
{
    public Renderer baseObject;
    public Renderer borderObject;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        baseObject.material = borderObject.material;
    }
}
