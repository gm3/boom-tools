using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
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

    void Start(){
        Button btn = buttonReference.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnImportClick); 
        Button brzbtn = browseButtonReference.GetComponent<Button>();
        brzbtn.onClick.AddListener(TaskOnBrowseButtonClick); 
        filename = inputField.text;
        inputField.onValueChanged.AddListener(OnPathValueChanged);
    }

    void Update()
    {
        
        //LoadMultiFileJSONIntoList();
        //Debug.Log(jsonFile);
    }

    void LoadJSON()
    {   
        jsonFile = Resources.Load(filename) as TextAsset;
        obj = JsonUtility.FromJson<TraitsToLoad>(jsonFile.text);
        GameObject[] objects = (GameObject[])Object.FindObjectsOfTypeAll(typeof(GameObject));
       
        foreach (AttributeClass attr in obj.attributes)
        {
            string traitType = attr.trait_type;
            string value = attr.value;
            //Debug.Log(traitType + ":" + value);

            // create variables for names of objects to be found
            string name = attr.value;
            //Debug.Log("the value is " + value);
            

            // set objects active that match the name
            foreach (GameObject objs in objects)
            {
                if (objs.name == name)
                {
                    objs.SetActive(true);
                    Debug.Log(name + "this object was set to" + objs.activeInHierarchy);
                    break;
                }
            }


        }

              
    }

    /* void LoadMultiFileJSONIntoList()
    {
        List<TraitsToLoad> TraitsToLoadListMultiFile = new List<TraitsToLoad>();
     
        foreach (string jsonFileName in jsonFileNames)
        {
            jsonFile = Resources.Load("JSON_Output1") as TextAsset;
            obj = JsonUtility.FromJson<TraitsToLoad>(jsonFile.text);
            TraitsToLoadListMultiFile.Add(obj);
            
            foreach (AttributeClass attr in obj.attributes)
                {
                string traitType = attr.trait_type;
                string value = attr.value;
              
                Debug.Log(traitType + ":" + value);

                }
            // Do something with the object of traits
        }  
    } */

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
