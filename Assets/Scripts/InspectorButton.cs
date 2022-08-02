using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
 public class InspectorButton : MonoBehaviour
 {
     public GameObject[] itemsToToggle;
     
     public bool toggleAllOn; //"run" or "generate" for example
     public bool toggleAllOff; //supports multiple buttons
 
     void Update()
     {
         if (toggleAllOn)
             ButtonFunction1 ();
         else if (toggleAllOff)
             ButtonFunction2 ();
         toggleAllOn = false;
         toggleAllOff = false;
     }
 
     void ButtonFunction1 ()
     {
         for (int i = 0; i < itemsToToggle.Length; i++) 
               {
                      itemsToToggle[i].SetActive(true);                              
               } 
         
     }
 
     void ButtonFunction2 ()
     {
         for (int i = 0; i < itemsToToggle.Length; i++) 
               {
                      itemsToToggle[i].SetActive(false);                              
               } 
     }
 }
