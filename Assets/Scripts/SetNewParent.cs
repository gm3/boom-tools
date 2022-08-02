using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetNewParent : MonoBehaviour
{
   public GameObject childReference;
   public GameObject newParentReference;
   
   public TMP_Dropdown ParentBoneDropdownReference;
   public TMP_Dropdown ChildObjectDropdownReference;
   public Button buttonReference;

   

   void Start(){
        Button btn = buttonReference.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
        
    }

    //Invoked when a button is pressed.
    public void SetParent(GameObject newParent)
    {
        //Makes the GameObject "newParent" the parent of the GameObject "player".
        childReference.transform.parent = newParent.transform;

        //Display the parent's name in the console.
        Debug.Log("Player's Parent: " + childReference.transform.parent.name);

        // Check if the new parent has a parent GameObject.
        if (newParent.transform.parent != null)
        {
            //Display the name of the grand parent of the player.
            Debug.Log("Player's Grand parent: " + childReference.transform.parent.parent.name);
        }
    }

    public void DetachFromParent()
    {
        // Detaches the transform from its parent.
        transform.parent = null;
    }

    void TaskOnClick(){
		Debug.Log (ChildObjectDropdownReference.value); 
        SetParent(newParentReference);    
	}
}
