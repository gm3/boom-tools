using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using System;
using TMPro;
using UnityEngine.Networking;

public class ImportJSON : MonoBehaviour
{
    string[] jsonFileNames = { "JSON_Output1" };
    public static List<TraitsToLoad> TraitsToLoadList = new List<TraitsToLoad>();
    public static TextAsset jsonFile;
    public static TraitsToLoad obj;
    public string filename;
    public Button buttonReference;
    public Button browseButtonReference;
    public int totalNFTs;
    public DNAManager dnaManagerReference;
    public TMP_InputField inputField;
    private string myString; 
    string path;    
    public ImportMaterialsConfig ImportMaterialsConfigRef; // Add a reference to the other script as a public field
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

    void Start()
        {
            Button btn = buttonReference.GetComponent<Button>();
            btn.onClick.AddListener(TaskOnImportClick); 
            Button brzbtn = browseButtonReference.GetComponent<Button>();
            brzbtn.onClick.AddListener(TaskOnBrowseButtonClick); 
            filename = inputField.text;
            inputField.onValueChanged.AddListener(OnPathValueChanged);
        }

    void TaskOnImportClick()
        {
            Debug.Log ("You have imported a json text file"); 
            LoadJSON();
        }

    void TaskOnBrowseButtonClick()
        {
            OpenFileExplorer();
            Debug.Log ("You have clicked browse"); 
            GetJSONFile();
        }

    void OnPathValueChanged(string newValue)
        {   
            filename = inputField.text;
        }
    
    public void OpenFileExplorer()
        {
            path = EditorUtility.OpenFilePanel("Show all json", "/Resources", "txt");
            string fileName = Path.GetFileNameWithoutExtension(path);
            string finalString = fileName;
            inputField.text = finalString;
        }
    
   public void LoadJSON()
        {   
            jsonFile = Resources.Load(filename) as TextAsset;
            obj = JsonUtility.FromJson<TraitsToLoad>(jsonFile.text);
            GameObject[] objects = (GameObject[])UnityEngine.Object.FindObjectsOfTypeAll(typeof(GameObject));
        
            foreach (AttributeClass attr in obj.attributes)
            {
                string traitType = attr.trait_type;
                string value = attr.value;

                

                foreach (GameObject objs in objects)
                {
                    if (objs.name == value)
                    {
                        objs.SetActive(true);
                    }

                    if (traitType == "BGColor")
                    {
                        for (int i = 0; i < layerStringData1.Length; i++)
                        {
                            
                            if (layerStringData1[i] == value)
                            {
                                index1 = i;
                                 if (index1 >= 0)
                                    {
                                        Object1Ref.GetComponent<Renderer>().material =  totalMaterialsObject1[index1];
                                        Debug.Log("Object1 Match " + traitType + " " + layerStringData1[i] + " " + value);
                                    }
                                
                            }
                        }
                    }
                    
                    
                    if (traitType == "BodyTexture")
                    {

                        for (int i = 0; i < layerStringData2.Length; i++)
                        {
                            if (layerStringData2[i] == value)
                            {
                                
                                index2 = i;
                                if (index2 >= 0)
                                    {
                                       Object2Ref.GetComponent<SkinnedMeshRenderer>().sharedMaterials = new Material[] { totalMaterialsObject2[index2] };
                                    Debug.Log("Object2 Match " + traitType + " " + layerStringData2[i] + " " + value);
                                    }
                                
                            }
                        }
                        
                    }

                    if (traitType == "BBTexture")
                    {
                        for (int i = 0; i < layerStringData3.Length; i++)
                        {
                            if (layerStringData3[i] == value)
                            {
                                index3 = i;
                                if (index3 >= 0)
                                {
                                    Object3Ref.GetComponent<SkinnedMeshRenderer>().sharedMaterials = new Material[] { totalMaterialsObject3[index3] };
                                    Debug.Log("BBTexture Match " + traitType + " " + layerStringData3[i] + " " + value);
                                }
                            }
                        }
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



}
