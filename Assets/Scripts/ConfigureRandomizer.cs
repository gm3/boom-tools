using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRM;
using UnityEngine.Animations;

public class ConfigureRandomizer : MonoBehaviour
{
    public int totalNFTs;
    public DNAManager dnaManagerReference;
    public Animator animationComponent;
    public RandomizeAll randomizeAllScriptReference;
    public PoseRandomizer randomPoseScriptReferences;
    public Button buttonReference;
    public CameraCapture cameraCaptureReference;
    public ToTextFile toTxtFileRef;
    private Coroutine delayCoroutine;
    public float delaySpeed;
    public VRMRuntimeExporter1 vrmRuntimeExporterRef;
    public GameObject modelToExportToVRM;
    public SetObjectsVisibility exportVRMFromRandomTrait;
    //public gltfExporter gltfExporterRef;
    public string vrmTitle = "";
    public string vrmAuthor = "";
    public string vrmContactInformation = "";
    public string vrmReference = "";

    public LicenseType license = LicenseType.CC0;

     public UssageLicense violentUssage = UssageLicense.Disallow;
    public UssageLicense sexualUssage = UssageLicense.Disallow;
    public UssageLicense commercialUssage = UssageLicense.Allow;
    public string vrmVersion = "0.x";
    private string currentAnimationStateValue;

    public bool exportPNG = false;
    public bool exportJPEG = true;

    public VRMAuthoringManager vrmAuthoringManager;

    public GameObject baseObject;

    public Camera thumbnailCamera; // <-- add this variable to hold the thumbnail camera


    
    //public int currentBatch; //This variable stores the index of the current batch being processed.
    //public int[] batch; //This variable is an array that stores the data for each batch. Each element in the array corresponds to a specific batch.
    //public string batchDate; //This variable stores the date of the current batch being processed.
    //public int totalInBatch; //This variable stores the total number of items in the current batch being processed.
    //public string batchPath; //This variable stores the file path of the current batch being processed.
    //public string imagePath; //This variable stores the file path of the current image being processed.
    //public string glbPath;//: This variable stores the file path of the current glb file being processed.
    //public string vrmPath;//: This variable stores the file path of the current vrm file being processed.

    
    // Start is called before the first frame update
    void Start()
    {
        Button btn = buttonReference.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        // Generate NFts
        if (dnaManagerReference.optionsManager != null)
            dnaManagerReference.optionsManager.AttachDataToDNA(dnaManagerReference);
        delayCoroutine = StartCoroutine(GenerateAllNFTsSlow(totalNFTs));
    }


    IEnumerator GenerateAllNFTsSlow(int totalNFTs)
    {
        HashSet<string> generatedDNAs = new HashSet<string>();

        
        


        VRMMetaObject metaData = ScriptableObject.CreateInstance<VRMMetaObject>();
        metaData.Title = vrmTitle;
        metaData.Version = vrmVersion;
        metaData.Author = vrmAuthor;
        metaData.ContactInformation = vrmContactInformation;
        metaData.Reference = vrmReference;
        metaData.LicenseType = vrmAuthoringManager.license;
        metaData.OtherLicenseUrl = vrmAuthoringManager.additionalLicenseInfoURL;
        metaData.AllowedUser = vrmAuthoringManager.allowedUser;
        metaData.ViolentUssage = violentUssage;
        metaData.SexualUssage = sexualUssage;
        metaData.CommercialUssage = commercialUssage;
        //metaData.Lice = vrmReference;
        VRMMeta metaComponent = modelToExportToVRM.AddComponent<VRMMeta>();
        

        //reset GenID
        dnaManagerReference.genID = 0;

            for (int i = 0; i < totalNFTs; i++) 
            {
               
              
        
                    vrmTitle = dnaManagerReference.name + " #" + (dnaManagerReference.genID+1).ToString();
                    metaComponent.Meta = metaData;

                    if(!dnaManagerReference.DNAList.Contains(dnaManagerReference.DNACode)){

                        randomizeAllScriptReference.RamdomizeAll();
                    }else{
                        randomizeAllScriptReference.RamdomizeAll();
                    }
                    // set animation state to the first random value assigned
                    currentAnimationStateValue = randomPoseScriptReferences.currentEntryValue;

                    if (animationComponent != null) {
                        animationComponent.SetTrigger(currentAnimationStateValue);

                            if (animationComponent.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
                            {
                                       
                            }
                            yield return new WaitForSeconds(delaySpeed+(int)Random.value);
                        cameraCaptureReference.Capture();
                        //cameraCaptureReference.CapturePNG();

                        modelToExportToVRM.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);  
                        ConnectBase();
                        
                        vrmTitle = dnaManagerReference.name + (dnaManagerReference.genID).ToString();
                        
                        if (thumbnailCamera != null) 
                            {
                                // Take a thumbnail from the camera and set it to the metadata
                                var thumbnailTexture = new Texture2D(512, 512, TextureFormat.RGB24, false);

                                // Create a RenderTexture to hold the thumbnail
                                RenderTexture rt = new RenderTexture(512, 512, 24, RenderTextureFormat.ARGB32);

                                rt.Create();

                                // Set the camera's viewport to match the RenderTexture size
                                thumbnailCamera.targetTexture = rt;
                                thumbnailCamera.pixelRect = new Rect(0, 0, rt.width, rt.height);

                                yield return new WaitForEndOfFrame();

                                // Render the camera to the RenderTexture
                                thumbnailCamera.Render();

                                // Read the pixels from the RenderTexture
                                RenderTexture.active = rt;
                                thumbnailTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
                                thumbnailTexture.Apply();
                                    
                                thumbnailCamera.pixelRect = new Rect(0, 0, thumbnailCamera.pixelWidth, thumbnailCamera.pixelHeight);
                                rt.Release();

                                // Convert the thumbnail texture to a byte array
                                byte[] thumbnailBytes = thumbnailTexture.EncodeToJPG();

                                // Create a new texture from the byte array
                                Texture2D thumbnailTextureFromBytes = new Texture2D(2, 2);
                                thumbnailTextureFromBytes.LoadImage(thumbnailBytes);

                                // Set the thumbnail image in the metadata
                                metaData.Thumbnail = thumbnailTextureFromBytes;
                            }
                            else 
                            {
                                Debug.LogError("thumbnailCamera is null!");
                            }
                        
                        metaData.Title = dnaManagerReference.name + " #" + (dnaManagerReference.genID+1).ToString();

                        metaComponent.Meta = metaData;

                        vrmRuntimeExporterRef.Export(modelToExportToVRM, true, dnaManagerReference.genID.ToString(), ".glb");

                        modelToExportToVRM.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self); 
                        DisonnectBase();

                        toTxtFileRef.CreateTextFile();
                        
                            if (exportVRMFromRandomTrait != null)
                            modelToExportToVRM = exportVRMFromRandomTrait.selectedObject as GameObject;

                            
                            

                            animationComponent.SetTrigger("TPose");
                            

                            while (animationComponent.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) {
                                yield return new WaitForSeconds(delaySpeed+(int)Random.value);
                            }

                            vrmRuntimeExporterRef.Export(modelToExportToVRM, true, dnaManagerReference.genID.ToString(), ".vrm");

                        yield return new WaitForSeconds(delaySpeed+(int)Random.value);

                        } else {

                            yield return new WaitForSeconds(delaySpeed+(int)Random.value);
                        }
                        
            }
            
    }

    public void ConnectBase(){
        baseObject.transform.SetParent(modelToExportToVRM.transform);
    }

    public void DisonnectBase(){
        baseObject.transform.SetParent(null);
    }

}
