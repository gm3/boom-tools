using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using System;
using TMPro;
using UnityEngine.Networking;
using VRM;
using System.Linq;

public class ImportJSON : MonoBehaviour
{
    
    string[] jsonFileNames = { "1" };
    public static List<TraitsToLoad> TraitsToLoadList = new List<TraitsToLoad>();
    public static TextAsset jsonFile;
    public static TraitsToLoad obj;
    public string filename;
    public Button buttonReference;
    public Button browseButtonReference;
    public Button batchImportButtonReference;
    public DNAManager dnaManagerReference;
    public TMP_InputField inputField;
    public TMP_InputField totalJsonFilesInputField;
    private string myString; 
    string path;    
    public GameObject Object1Ref; // Add a public field for the first additional game object
    public GameObject Object2Ref; // Add a public field for the second additional game object
    //public GameObject Object3Ref; // Add a public field for the third additional game object

    public Material[] totalMaterialsObject1;
    public string[] layerStringData1;
    public Material[] totalMaterialsObject2;
    public string[] layerStringData2;
    /* public Material[] totalMaterialsObject3;
    public string[] layerStringData3;   */

    private int index1 = -1;
    private int index2 = -1;
    //public int index3 = -1;

    private GameObject objectToChangeMaterialOf;

    public int totalNFTs = 1;
    int loadCount = 0;

    public int rangeStart;
    public int rangeEnd;
    public int currentOutoutEdition;

    // VRM Metadata feilds
    public string vrmTitle = "";
    public string vrmAuthor = "";
    public string vrmContactInformation = "";
    public string vrmReference = "";
    public string vrmVersion = "0.x";

    public LicenseType license = LicenseType.CC0;

    public UssageLicense violentUssage = UssageLicense.Disallow;
    public UssageLicense sexualUssage = UssageLicense.Disallow;
    public UssageLicense commercialUssage = UssageLicense.Allow;

    public VRMAuthoringManager vrmAuthoringManager;

    private string currentAnimationStateValue;

    public bool exportPNG = false;
    public bool exportJPEG = true;

    // CameraReference
    public CameraCapture cameraCaptureReference;

    // Export specific feilds
    public ToTextFile toTxtFileRef;
    private Coroutine delayCoroutine;
    public float delaySpeed;
    public VRMRuntimeExporter1 vrmRuntimeExporterRef;
    public GameObject modelToExportToVRM;
    public SetObjectsVisibility exportVRMFromRandomTrait;
    //public gltfExporter gltfExporterRef;

    public bool isExportingToFile = false; 
    public bool isExportingScreenshots = false; 

    public string materialTraitNameToLoad1;
    public string materialTraitNameToLoad2;
    //public string materialTraitNameToLoad3;
    
    public Animator animationComponent;

    public string[] possiblePoses;

    public GameObject baseObject;

    void Start()
        {
            Button btn = buttonReference.GetComponent<Button>();
            btn.onClick.AddListener(TaskOnImportClick); 
            Button brzbtn = browseButtonReference.GetComponent<Button>();
            brzbtn.onClick.AddListener(TaskOnBrowseButtonClick); 

            // set filename default
            filename = inputField.text;

            Button batchbtn = batchImportButtonReference.GetComponent<Button>();
            batchbtn.onClick.AddListener(TaskOnBatchImportClick); 

            inputField.onValueChanged.AddListener(OnPathValueChanged);
            totalJsonFilesInputField.onValueChanged.AddListener(OnTotalJsonFilesValueChanged);
        }

    void TaskOnImportClick()
        {
            //Debug.Log ("You have imported a json text file"); 
            LoadJSON(filename);
        }

    void TaskOnBatchImportClick()
        {
            
            //dnaManagerReference.genID = 1;
            StartCoroutine(BatchLoadJSON());
            //Debug.Log ("You have imported a batch"); 
            
        }

    void TaskOnBrowseButtonClick()
        {
            OpenFileExplorer();
            //Debug.Log ("You have clicked browse"); 
            GetJSONFile();
        }

    void OnPathValueChanged(string newValue)
        {   
            filename = inputField.text;
        }

    void OnTotalJsonFilesValueChanged(string newValue)
        {   
            
            totalNFTs = int.Parse(totalJsonFilesInputField.text);
        }
    
    public void OpenFileExplorer()
        {
            path = EditorUtility.OpenFilePanel("Show all json", "/Resources", "json");
            string fileName = Path.GetFileNameWithoutExtension(path);
            string finalString = fileName;
            inputField.text = finalString;
        }
    
   public void LoadJSON(string filename)
{   
    //Debug.Log(filename);
    jsonFile = Resources.Load(filename) as TextAsset;
    obj = JsonUtility.FromJson<TraitsToLoad>(jsonFile.text);
    GameObject[] objects = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));

    GameObject[] objectsWithTraits = GameObject.FindGameObjectsWithTag("Traits");

    foreach (GameObject objs in objectsWithTraits)
                {
                    //Debug.Log(objs.name);
                    objs.SetActive(false); 
                }
    
    foreach (AttributeClass attr in obj.attributes)
    {
    
        string traitType = attr.trait_type;
        string value = attr.value;

        if (traitType == materialTraitNameToLoad1)
        {
            LoadMaterial(traitType, value, layerStringData1, totalMaterialsObject1, Object1Ref);
        }
        else if (traitType == materialTraitNameToLoad2)
        {
            LoadMaterial(traitType, value, layerStringData2, totalMaterialsObject2, Object2Ref);
        }
        /* else if (traitType == materialTraitNameToLoad3)
        {
            LoadMaterial(traitType, value, layerStringData3, totalMaterialsObject3, Object3Ref);
        } */
        else
        {
            
            
                // Find the object that matches the trait value and set it active
            foreach (GameObject objs in objects)
            {
                if (objs.name == value && objs.transform.parent.name == traitType)
            {
                objs.SetActive(true);
            }
            }

            
        }
    }
}

    private void LoadMaterial(string traitType, string value, string[] layerStringData, Material[] materials, GameObject gameObject)
        {
            for (int i = 0; i < layerStringData.Length; i++)
            {
                if (layerStringData[i] == value)
                {
                    int index = i;
                    if (index >= 0)
                    {
                        
                            gameObject.GetComponent<Renderer>().material = materials[index];
                        
                        //Debug.Log(traitType + " Match " + layerStringData[i] + " " + value);
                    }
                }
            }
        }

  

   IEnumerator GetJSONFile()
        {
        UnityWebRequest www = UnityWebRequest.Get("file:///" + path);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        

        if (www.isHttpError || www.isNetworkError)
        {
                Debug.Log(www.error); 
        }
        else
        {
                string text = www.downloadHandler.text;
                filename = text;
        }
        }


    public bool ExportVRM(string ext)
        {
             if(isExportingToFile)
             {
             
                    // export vrm            
                    if (exportVRMFromRandomTrait != null)
                    
                    modelToExportToVRM = exportVRMFromRandomTrait.selectedObject as GameObject;
                    dnaManagerReference.genID++;
                    vrmRuntimeExporterRef.Export(modelToExportToVRM, true, currentOutoutEdition.ToString(), ext);

                    dnaManagerReference.jsonOutputPreview = jsonFile.text;

                    loadCount++;
                    dnaManagerReference.genID = loadCount;

                    // eport json
                    toTxtFileRef.CreateBatchImportTextFile();

                    // take screenshot
                    
                    // export to file if condition is true
                    
             }
                
            // ...
            //Debug.Log("You exported VRM# " + loadCount);
            // Return true if export was successful, false otherwise
            return true;
        }


    IEnumerator BatchLoadJSON()
        {
            string path = "Assets/Resources/"; // Use a valid path
            string[] info = Directory.GetFiles(path, "*.json");

            int totalFiles = info.Length;

            string[] fileEntries = info;
            Array.Sort(fileEntries);
        
            for (int i = rangeStart; i < rangeEnd; i++) 
                {

                currentOutoutEdition = i;
                LoadJSON(i.ToString());
                
                //randomize pose before export
                string randomPose = possiblePoses[UnityEngine.Random.Range (0, possiblePoses.Length)];
                animationComponent.SetTrigger(randomPose);
                Debug.Log("You exported a random pose");
                yield return new WaitForSeconds(1f);

                // Add metadata
                AdddMetaData();
                ConnectBase();

                if(isExportingScreenshots)
                    {
                    cameraCaptureReference.ExportCapture();  

                    }

                modelToExportToVRM.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);  

                //vrmRuntimeExporterRef.ExportSimple(modelToExportToVRM);
                ExportVRM(".glb");
                DisonnectBase();
                modelToExportToVRM.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self); 

                //go back to TPose pose before export
                animationComponent.SetTrigger("TPose");
                yield return new WaitForSeconds(1f);
                Debug.Log(randomPose);

                // Add metadata
                AdddMetaData();

                ExportVRM(".vrm");
                //Debug.Log("You exported a TPose pose");
                yield return new WaitForSeconds(1f);
                }  
        }

    public void AdddMetaData()
        {
            VRMMetaObject metaData = ScriptableObject.CreateInstance<VRMMetaObject>();
           metaData.Title = vrmTitle + " #" + currentOutoutEdition.ToString();
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

            VRMMeta metaComponent = modelToExportToVRM.AddComponent<VRMMeta>();
            metaComponent.Meta = metaData;   
            //yield return new WaitForSeconds(.01f);    
        }  

    public void ConnectBase(){
        baseObject.transform.SetParent(modelToExportToVRM.transform);
        }

    public void DisonnectBase(){
            baseObject.transform.SetParent(null);
        }

}









