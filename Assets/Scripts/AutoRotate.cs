using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{

  
  public float ySpeed = 0.3f;
  public float xSpeed = 0f;
  public float zSpeed = 0f;

  public GameObject thingToRotate; 


    void Update()
   {
       thingToRotate.transform.Rotate(xSpeed * Time.deltaTime, ySpeed * Time.deltaTime, zSpeed * Time.deltaTime);
   }

}
