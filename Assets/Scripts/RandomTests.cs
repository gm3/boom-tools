using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTests : MonoBehaviour
{
    int randomNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        randomNumber = UnityEngine.Random.Range(1, 100);  //Random number between 1 and 99
        Debug.Log("Hello: " + randomNumber);
    }
}
