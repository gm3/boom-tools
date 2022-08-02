using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
   public GameObject[] itemsOnDisplay;
    private int totalItems;
    private int currentItemIndex = 0;

    void Start()
    {
        // check the total length of the array
        totalItems = itemsOnDisplay.Length;
    }

    void Update()
    {

    if (Input.GetKeyDown(KeyCode.PageUp))
            
            {
            // You Pushed Page Up
            //check to see if the items index in the array is still within range
            if(currentItemIndex < totalItems-1){
                currentItemIndex += 1;
            }
            

            for (int i = 0; i < totalItems; i++)
						{
						   itemsOnDisplay[i].SetActive(false);   
						}
						
			itemsOnDisplay[currentItemIndex].SetActive(true);
                        
                        
                        if(currentItemIndex > itemsOnDisplay.Length){
                            currentItemIndex = totalItems;
                        }

                        
                }

    if (Input.GetKeyDown(KeyCode.PageDown))
			
            {
			// You Pushed Page Down
            //check to see if the items index in the array is still within range
            if(currentItemIndex > 0){
                currentItemIndex -= 1;
            }
           	  

            for (int i = 0; i < totalItems; i++)
						{
						   itemsOnDisplay[i].SetActive(false);   
						}
						
						
            itemsOnDisplay[currentItemIndex].SetActive(true);            
                        
                        if(currentItemIndex < 0){
                            currentItemIndex = 0;
                        }


			}


    }
}