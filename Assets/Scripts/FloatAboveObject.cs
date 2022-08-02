using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatAboveObject : MonoBehaviour
{
    public GameObject objectToFloatAbove;
    public GameObject objectThatIsFloating;
    public int Yoffset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        objectThatIsFloating.transform.position = objectToFloatAbove.transform.position + new Vector3(0,Yoffset,0);
    }
}
