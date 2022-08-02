using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* 1-9 keys will swap between cameras
Zero Key (0) goes to Overlay mode
F - Map Cameras To Screen
L - Hand Held Camera Light
G - Greenscreen
M - Mirror Toggle
T - Studio Lights Toggle
P - Reflection Probe Toggle
U - Toggle All Cameras On and Off
K - Photo Screen Backdrop Toggle
O - Camera Preview Toggle
Page Up - Swap item left
Page Down - Swap Item Right
Backslash - Show Hide All Items 
RightBracket - Sunlight 
Mouse2 MiddleMouse - Adjest X Y position of Camera
Divide Numpad - Post Processing On Off*/

public class CameraControls : MonoBehaviour
{				public int cameraNumber = 1;
				public int totalCameras;
				public int totalCameraReferences; // total cam component references
				public GameObject[] cameras;
				public GameObject[] adjustedCameraPositions;
				public Camera[] cameraComponentReferences; // the camera components

				private float mouseXvalue;  
        		private float mouseYvalue;  

				public int slideNumber;
				public int totalSlides;
				public GameObject[] slides; 
				
				public GameObject CamOverlay;
				public Camera FullScreenCam;
			  
			      	public Camera camReference1;
					public Camera camReference2;
					public Camera camReference3;
					public Camera camReference4;
					public Camera camReference5;
					public Camera camReference6;
					public Camera camReference7;
					public Camera camReference8;
					public Camera camReference9;
			 
			 private bool isGreenPressed = false;
			 public Color cameraBgColor1 = Color.black;
    		 public Color cameraBgColor2 = Color.green;
    		 public float cameraBackgroundSwapDuration = 3.0F;
			  
   
			  public GameObject allCamerasReference;
			  private bool showAllCameras = true;
			  
			  public GameObject cemeraPreviewReferences;
			  private bool showCameraPreviews = true;
			  
			  public GameObject realtimeReflectionProbeReference;
			  private bool showRealtimeReflectionProbes =  true;

			  public GameObject bakedReflectionProbeReference;
			 
			  
			  public GameObject camear1LightReference;
			  public GameObject camear2LightReference;
			  private bool camLightOn = false;
			  
			  
			 void Start()
			 {

			// init positions of default adjusted cam positions
				for (int i = 0; i < totalCameras; i++)
						{
						   adjustedCameraPositions[i].transform.position = cameras[i].transform.position;   
						} 

			 }
			  
			  



			void ChangeCamera(int cameraNumber){
				
				 for (int i = 0; i < totalCameras; i++)
						{
						   cameras[i].SetActive(false);   
						}
						
						cameras[cameraNumber-1].SetActive(true);
			}   

			public void AdjustCamera(){
					
				
				if (mouseXvalue != 0) {  
					Debug.Log("Mouse X movement: " + mouseXvalue);  
				}  
				if (mouseYvalue != 0) {  
					Debug.Log("Mouse Y movement: " + mouseYvalue);  
				}  
				
				for (int i = 0; i < totalCameras; i++)
						{
						   cameras[i].transform.position += new Vector3(mouseXvalue/10, mouseYvalue/10, 0);   
						}
		
						
				 
			}   

            void ChangeSlide(int slideNumber){
				
				 for (int i = 0; i < totalSlides; i++)
						{
						   slides[i].SetActive(false);   
						}
						
						slides[slideNumber-1].SetActive(true);
			
			 }

			 void ChangeCameraBGColor(/*int cameraNumber*/){
				if(isGreenPressed)
				{

				 for (int i = 0; i < totalCameraReferences; i++)
						{
						    cameraComponentReferences[i].backgroundColor = Color.Lerp(cameraBgColor1, cameraBgColor2, 1F); 
						}
						
						
				}else{
					for (int i = 0; i < totalCameraReferences; i++)
						{
						    cameraComponentReferences[i].backgroundColor = Color.Lerp(cameraBgColor2, cameraBgColor1, 1F); 
						}
				}

			}   
   

    // Update is called once per frame
    void Update()
    {

			
		mouseXvalue = Input.GetAxis ("Mouse X");  
        mouseYvalue = Input.GetAxis ("Mouse Y"); 

        if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				ChangeCamera(1);
			}
		if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				ChangeCamera(2);
			}
		if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				ChangeCamera(3);
			}
		if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				ChangeCamera(4);
			}
		if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				ChangeCamera(5);
			}
		if (Input.GetKeyDown(KeyCode.Alpha6))
			{
				ChangeCamera(6);
			}
		if (Input.GetKeyDown(KeyCode.Alpha7))
			{
				ChangeCamera(7);
			}
		if (Input.GetKeyDown(KeyCode.Alpha8))
			{
				ChangeCamera(8);
			}
		if (Input.GetKeyDown(KeyCode.Alpha9))
			{
				ChangeCamera(9);
			}	
		if (Input.GetKeyDown(KeyCode.Alpha0))
			{
				ChangeCamera(10);
			}
			
			// load full screen camera which overlays screen
			if (Input.GetKeyDown(KeyCode.F))
			{
				FullScreenCam.enabled = !FullScreenCam.enabled;
			}
			
			
				// Middle Mouse Button To Adjust Camera x Y
			if (Input.GetKey(KeyCode.Mouse2))
			{
				// ADjust Camera Function
				AdjustCamera();	
				Debug.Log("Mouse2 Clicked"); 
				
			}
			
			
			

			
			// green screen toggle "G"
			if (Input.GetKeyDown(KeyCode.G))
			{
			
			ChangeCameraBGColor();
			isGreenPressed = !isGreenPressed;
			
			}

			
			
			
			// cameera 1 light "L"
			if (Input.GetKeyDown(KeyCode.L))
			{
				
				camLightOn = !camLightOn;
				
				if (camLightOn)
				{ 
             camear1LightReference.SetActive(true);
			 camear2LightReference.SetActive(true);
				}else{
             camear1LightReference.SetActive(false);
			 camear2LightReference.SetActive(false);
				}
			}

			

			
			// realtime reflection probe toggle "P"
			if (Input.GetKeyDown(KeyCode.P))
			{
				
				showRealtimeReflectionProbes = !showRealtimeReflectionProbes;
				
				if (showRealtimeReflectionProbes)
				{ 
             realtimeReflectionProbeReference.SetActive(true);
			 bakedReflectionProbeReference.SetActive(false);
				}else{
             realtimeReflectionProbeReference.SetActive(false);
			 bakedReflectionProbeReference.SetActive(true);
				}
			}
			
			// camera previews toggle "O"
			if (Input.GetKeyDown(KeyCode.O))
			{
		
				showCameraPreviews = !showCameraPreviews;
				
				if (showCameraPreviews) 
             cemeraPreviewReferences.SetActive(true);
				else
             cemeraPreviewReferences.SetActive(false);
			}
			
			// all cameras toggle "U"
			if (Input.GetKeyDown(KeyCode.U))
			{
		
				showAllCameras = !showAllCameras;
				
				if (showAllCameras) 
             allCamerasReference.SetActive(true);
				else
             allCamerasReference.SetActive(false);
			}
			
			
			
			

		
			
			// Overlay Slide Changer
			if (Input.GetKeyDown("[1]")){
				ChangeSlide(1);
			}
			
			if (Input.GetKeyDown("[2]")){
				ChangeSlide(2);
			}
			
			if (Input.GetKeyDown("[3]")){
				ChangeSlide(3);
			}
			
			if (Input.GetKeyDown("[4]")){
				ChangeSlide(4);
			}
			
			if (Input.GetKeyDown("[5]")){
				ChangeSlide(5);
			}
			
			if (Input.GetKeyDown("[6]")){
				ChangeSlide(6);
			}
			
			if (Input.GetKeyDown("[7]")){
				ChangeSlide(7);;
			}
			
			if (Input.GetKeyDown("[8]")){
				ChangeSlide(8);
			}
			
			if (Input.GetKeyDown("[9]")){
				ChangeSlide(9);
			}
			
			if (Input.GetKeyDown("[0]")){
				ChangeSlide(10);
			}
			
			if (Input.GetKeyDown("[+]")){
				ChangeSlide(11);
			}
			
			if (Input.GetKeyDown("[.]")){ 
				ChangeSlide(12);
			}
			
	
		
			
			
			
			
				
			
			
			
			
			
    }

	private void FixedUpdate(){
		// zoom


        if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
        camReference1.fieldOfView--;
		camReference2.fieldOfView--;
		camReference3.fieldOfView--;
		camReference4.fieldOfView--;
		camReference5.fieldOfView--;
		camReference6.fieldOfView--;
		camReference7.fieldOfView--;
		camReference8.fieldOfView--;
		camReference9.fieldOfView--;
        }

        if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
        camReference1.fieldOfView++;
		camReference2.fieldOfView++;
		camReference3.fieldOfView++;
		camReference4.fieldOfView++;
		camReference5.fieldOfView++;
		camReference6.fieldOfView++;
		camReference7.fieldOfView++;
		camReference8.fieldOfView++;
		camReference9.fieldOfView++;
        }

		 
	}
}
