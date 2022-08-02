using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
#endif

[ExecuteInEditMode]
public class WearableOffsetManager : MonoBehaviour
{
    
    public Slider[] PSR;
    public TMP_Dropdown[] PSRMode;
    public GameObject objectToOffset;
    private Vector3[] PSRDefaults = new Vector3[3];
    //public Vector3[] PSRVectors;
    public Button buttonReference;

    public GameObject[] allGameOjbectsToControl;
    public GameObject[] allPositionContainerReferences;
  

    
    
    // Start is called before the first frame update
    void Start()
    {

        // Declare ResetPSR Button
        Button btn = buttonReference.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);

           
        // set Default values for the PSR
        /* for (int i = 0; i < PSR.Length; ++i) 
        {
            PSRDefaults[i].position = new Vector3(0,0,0);
        } */

        
            // error check to make sure there is items in the array
            if (PSR.Length != 0)
            {

            // declare and event listener for each slider in the array
             for (int i = 0; i < PSR.Length; i++) 
                {
                PSR[i].onValueChanged.AddListener (delegate {ValueChangeCheck ();});      
                }
            }

            
    }

    // Update is called once per frame
    void Update()
    {
            
            //objectToOffset.transform.localRotation = allPositionContainerReferences[0].transform.localRotation;
            // This code goes through all of the game object references in the array assignes the position of the Position Controllers to the positions of each asset container object
             for (int i = 0; i < allGameOjbectsToControl.Length; i++) 
                {
                   
                        allGameOjbectsToControl[i].transform.position = allPositionContainerReferences[i].transform.position;
                        allGameOjbectsToControl[i].transform.localScale = allPositionContainerReferences[i].transform.localScale;
                        allGameOjbectsToControl[i].transform.rotation = allPositionContainerReferences[i].transform.rotation;
                }        

    }

    public void ResetPSR()
    {

        for (int i = 0; i < PSR.Length; i++)
        {
            objectToOffset.transform.localPosition = PSRDefaults[i];       
        }
        
    }

    public void ValueChangeCheck()
    {   
                        //Debug.Log(PSR[0].value);    
                        objectToOffset.transform.localPosition = new Vector3(PSR[0].value,PSR[1].value,PSR[2].value);
	}

   public void TaskOnClick()
    {
		Debug.Log ("You have reset the PSR"); 
        ResetPSR();
       
                  
	}

   
}
