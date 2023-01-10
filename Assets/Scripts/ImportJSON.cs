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

public class ImportJSON : MonoBehaviour
{
    
    string[] jsonFileNames = { "JSON_Output1" };
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
    public GameObject Object3Ref; // Add a public field for the third additional game object

    public Material[] totalMaterialsObject1;
    public string[] layerStringData1;
    public Material[] totalMaterialsObject2;
    public string[] layerStringData2;
    public Material[] totalMaterialsObject3;
    public string[] layerStringData3;  

    public int index1 = -1;
    public int index2 = -1;
    public int index3 = -1;

    private GameObject objectToChangeMaterialOf;

    public int totalNFTs = 1;
    int loadCount = 0;

    // VRM Metadata feilds
    public string vrmTitle = "";
    public string vrmAuthor = "";
    public string vrmContactInformation = "";
    public string vrmReference = "";
    public string vrmVersion = "0.x";

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
            Debug.Log ("You have imported a batch"); 
            
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
            path = EditorUtility.OpenFilePanel("Show all json", "/Resources", "txt");
            string fileName = Path.GetFileNameWithoutExtension(path);
            string finalString = fileName;
            inputField.text = finalString;
        }
    
   public void LoadJSON(string filename)
{   
    Debug.Log(filename);
    jsonFile = Resources.Load(filename) as TextAsset;
    obj = JsonUtility.FromJson<TraitsToLoad>(jsonFile.text);
    GameObject[] objects = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));

    GameObject[] objectsWithTraits = GameObject.FindGameObjectsWithTag("Traits");

    foreach (GameObject objs in objectsWithTraits)
                {
                    Debug.Log(objs.name);
                    objs.SetActive(false); 
                }
    
    foreach (AttributeClass attr in obj.attributes)
    {
    
        string traitType = attr.trait_type;
        string value = attr.value;

        if (traitType == "BGColor")
        {
            LoadMaterial(traitType, value, layerStringData1, totalMaterialsObject1, Object1Ref);
        }
        else if (traitType == "BodyTexture")
        {
            LoadMaterial(traitType, value, layerStringData2, totalMaterialsObject2, Object2Ref);
        }
        else if (traitType == "BBTexture")
        {
            LoadMaterial(traitType, value, layerStringData3, totalMaterialsObject3, Object3Ref);
        }
        else
        {
            
            
                // Find the object that matches the trait value and set it active
            foreach (GameObject objs in objects)
            {
                if (objs.name == value)
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
                        if (gameObject.GetComponent<SkinnedMeshRenderer>() != null)
                        {
                            gameObject.GetComponent<SkinnedMeshRenderer>().sharedMaterials = new Material[] { materials[index] };
                        }
                        else
                        {
                            gameObject.GetComponent<Renderer>().material = materials[index];
                        }
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


    public bool ExportVRM()
        {
             if(isExportingToFile)
             {
             
                    // export vrm            
                    if (exportVRMFromRandomTrait != null)

                    
                        modelToExportToVRM = exportVRMFromRandomTrait.selectedObject as GameObject;
                        dnaManagerReference.genID++;
                    vrmRuntimeExporterRef.Export(modelToExportToVRM, true, dnaManagerReference.genID.ToString());

                    dnaManagerReference.jsonOutputPreview = jsonFile.text;

                    loadCount++;
                    dnaManagerReference.genID = loadCount;

                    // eport json
                    toTxtFileRef.CreateTextFile();

                    // take screenshot
                    if(isExportingScreenshots)
                    {
                    cameraCaptureReference.Capture();    
                    }
                    

                    // export to file if condition is true
                    
             }
                
            // ...
            Debug.Log("You exported VRM# " + loadCount);
            // Return true if export was successful, false otherwise
            return true;
        }


    IEnumerator BatchLoadJSON()
        {
            string path = "Assets/Resources/"; // Use a valid path
            string[] info = Directory.GetFiles(path, "*.txt");

           // string fileName = Path.GetFileNameWithoutExtension(path);

            string[] fileEntries = info;
        
            foreach (string fileName in fileEntries)
            {
                string file = Path.GetFileNameWithoutExtension(fileName);
                //string file = fileName.Substring(0, fileName.Length - 4);
                //Debug.Log(file);
                
                // Load first JSON file
                LoadJSON(file);

                //Add VRMMetadata to the VRM
                AdddMetaData();

                ExportVRM();

                //update the genID
                

                //update the jsonOutputPrevie required for text output
                

                // Export VRM with null check
                /* if ( == false)
                {
                    break;
                } */
                yield return new WaitForSeconds(.2f);
            }
        }

    public void AdddMetaData()
        {
            VRMMetaObject metaData = ScriptableObject.CreateInstance<VRMMetaObject>();
            metaData.Title = vrmTitle;
            metaData.Version = vrmVersion;
            metaData.Author = vrmAuthor;
            metaData.ContactInformation = vrmContactInformation;
            metaData.Reference = vrmReference;

            VRMMeta metaComponent = modelToExportToVRM.AddComponent<VRMMeta>();
            metaComponent.Meta = metaData;   
            //yield return new WaitForSeconds(.01f);    
        }  

}








