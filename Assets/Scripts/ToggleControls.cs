using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

			public GameObject consoleUIReference;
			private bool isConsoleUIOn = true;

			public RandomizeAll randomizeAllref;
            
			public GameObject TraitListUIReference;
			private bool isTraitlistUIOn = true;
			public Button traitsButtonReference;

    void Start()
    {
        	Button traitsbtn = traitsButtonReference.GetComponent<Button>();
            traitsbtn.onClick.AddListener(TaskOnShowTraitsClick); 
    }

	void TaskOnShowTraitsClick()
        {
            
            TraitListUIReference.SetActive(isTraitlistUIOn);
			isTraitlistUIOn = !isTraitlistUIOn;
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

			// toggle ui
            if (Input.GetKeyDown(KeyCode.BackQuote))
			{
				isConsoleUIOn = !isConsoleUIOn;
				
				if (isConsoleUIOn) 
             consoleUIReference.SetActive(true);
				else
             consoleUIReference.SetActive(false);
				
			}

			if (Input.GetKeyDown(KeyCode.Space))
			{
				randomizeAllref.RamdomizeAllSpaceBar();
				
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
