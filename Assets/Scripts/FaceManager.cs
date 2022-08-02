using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
#endif

[ExecuteInEditMode]
public class FaceManager : MonoBehaviour
{
    

   
   public Texture[] allTextureReferences;
   public GameObject[] allGameOjbectReferences;
   public GameObject[] allGameOjbectBackReferences;
   public GameObject[] allGameOjbectPositionReferences;
   public GameObject[] allPositionControllerReferences;
   public Transform[] allTransformReferences;
    //public Renderer[] allRenderReferences;
   //public Material[] allMaterialReferences;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            
            for (int i = 0; i < allGameOjbectReferences.Length; i++) 
               {
                        // This code goes through all of the game objects in the array and changes the Texture on both the Main and Emissive Channel
                        allGameOjbectReferences[i].GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", allTextureReferences[i]);
                        allGameOjbectReferences[i].GetComponent<Renderer>().sharedMaterial.SetTexture ("_EmissionMap", allTextureReferences[i]);
                        allGameOjbectBackReferences[i].GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", allTextureReferences[i]);
                        allGameOjbectBackReferences[i].GetComponent<Renderer>().sharedMaterial.SetTexture ("_EmissionMap", allTextureReferences[i]);                                 
               } 
            
            // This code goes through all of the game object references in the array assignes the position of the Position Controllers to the positions of each asset container object
            for (int i = 0; i < allGameOjbectPositionReferences.Length; i++) 
                {
                   if(i < allGameOjbectPositionReferences.Length){ 
                        allGameOjbectPositionReferences[i].transform.position = allPositionControllerReferences[i].transform.position;
                        allGameOjbectPositionReferences[i].transform.localScale = allPositionControllerReferences[i].transform.localScale;
                        allGameOjbectPositionReferences[i].transform.localRotation = allPositionControllerReferences[i].transform.localRotation;
                        
                   }
                }       

    }

         
}
