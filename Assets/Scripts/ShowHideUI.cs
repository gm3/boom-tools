using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideUI : MonoBehaviour
{
    public GameObject UIReference;
	private bool isUIOn = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // toggle UI
			if (Input.GetKeyDown(KeyCode.Tilde))
			{
				
				isUIOn = !isUIOn;
				
				if (isUIOn)
				{ 
             UIReference.SetActive(true);
			
				}else{
             UIReference.SetActive(false);
			
				}
			}
    }

   
}
