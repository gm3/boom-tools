using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleControls : MonoBehaviour
{
    // declare variables
                  
            
                    
            public GameObject studioLightsReference;
            private bool showStudioLights = false;

			public GameObject sunLightReference;
			private bool sunLightRefernceOn = false;

			public GameObject postPossingReference;
			private bool PostProcessingVolumeOn = false;

			public GameObject UIReference;
			private bool isUIOn = true;

			public RandomizeAll randomizeAllref;
            


    void Start()
    {
        
    }

    void Update()
    {
       
     

			// toggle ui
            if (Input.GetKeyDown(KeyCode.Tab))
			{
				isUIOn = !isUIOn;
				
				if (isUIOn) 
             UIReference.SetActive(true);
				else
             UIReference.SetActive(false);
				
			}

			if (Input.GetKeyDown(KeyCode.Space))
			{
				randomizeAllref.RamdomizeAll();
				
			}

            // sunlight Reference Right Bracket
            if (Input.GetKeyDown(KeyCode.RightBracket))
			{
				sunLightRefernceOn = !sunLightRefernceOn;
				
				if (sunLightRefernceOn) 
             sunLightReference.SetActive(true);
				else
             sunLightReference.SetActive(false);
				
			}


            // studio lights toggle "T"
			if (Input.GetKeyDown(KeyCode.T))
			{
				
				showStudioLights = !showStudioLights;
				
				if (showStudioLights) 
             studioLightsReference.SetActive(true);
				else
             studioLightsReference.SetActive(false);
			}

		
			// PostProcessing ShowHide
			if (Input.GetKeyDown(KeyCode.KeypadDivide))
			{
				
				PostProcessingVolumeOn = !PostProcessingVolumeOn;
				
				if (PostProcessingVolumeOn)
				{ 
             postPossingReference.SetActive(true);
				}else{
             postPossingReference.SetActive(false);
				}
			}

			// randomize all



    }
}
