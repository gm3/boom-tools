using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class ExportToWebaverse : MonoBehaviour
{
    public Button buttonReference;

    [TextArea(10,10)]
    public string webaJSONOutputPreview;

    public TextMeshProUGUI WebaJSONBody;
    private int exportNumber;

    // reference to the allObjestdata Array
    

    void Start()
    {       
        // create a folder
        Directory.CreateDirectory(Application.streamingAssetsPath + "/WEBA_Output/");

        // Reference to Button
        Button btn = buttonReference.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);    
    }

    
    void Update()
    {      
    }

    void TaskOnClick()
    {
		Debug.Log ("You have exported a webaverse scene file"); 
       
        //GatherAllLightsAndParameters();
        //GatherAllCamerasAndParamaters();
        //GatherAllObjectsComponentsAndParamaters();

        //  go through all objects in scene and define an array of all the objects and components and their respective parameters

        //typecheck all of the object and build a dictionary of all of these objects

        // assign all of the paramaters to the correct export variable containers

        // do all of the error checking and contructing of the objects strings

        ExportWebaverseJsonToText();

        if(webaJSONOutputPreview == "")
        {
            return;
        }
       
        // create and print the text file
        CreateTextFile();
        exportNumber++;         
	}

    

    

public void ExportWebaverseJsonToText()
    {
       
        webaJSONOutputPreview = "{\n" + 
        // open objects array
        "  \"objects\": [\n" +

        GetAllObjects() +
        
        // close object array
        "  ]\n" +
        "}";

        WebaJSONBody.text = webaJSONOutputPreview;
    }

    // object constructor
     // use \t to tab and \n to line break
    public string GetObject(int objectIndex)
    {
        string value;

        // start string constructor using "value" to temp hold the data
        value = 

        // light object example
        "    {\n" +
        "      \"type\": \"application/light\",\n" +
        "      \"content\": {\n" +
        "        \"lightType\": \"ambient\",\n" +
        "        \"args\": [\n" +
        "           [\n" +
        "             225,\n" +
        "             225,\n" +
        "             225\n" +
        "           ],\n" +
        "           0.1\n" +
        "         ]\n" +
        "       }\n" +
        "     },\n" +

        // 2nd light example

        "     {\n" +
        "      \"type\": \"application/light\",\n" +
        "      \"content\": {\n" +
        "        \"lightType\": \"directional\",\n" +
        "        \"args\": [\n" +
        "           [\n" +
        "             255,\n" +
        "             255,\n" +
        "             255\n" +
        "           ],\n" +
        "           0.5\n" +
        "         ],\n" +
        "         \"position\": [\n" +
        "           22,\n" +
        "           177,\n" +
        "           360\n" +
        "         ],\n" +
        "         \"shadow\": [\n" +
        "            150,\n" +
        "            5120,\n" +
        "            0.1,\n" +
        "            10000,\n" +
        "            -0.0001\n" +
        "         ]\n" +
        "       }\n" +
        "    },\n" +

        // wind example

        "     {\n" +
        "      \"type\": \"application/wind\",\n" +
        "      \"content\": {\n" +
        "        \"windType\": \"directional\",\n" +
        "        \"direction\": [\n" +
        "           22,\n" +
        "           177,\n" +
        "           360\n" +
        "         ],\n" +
        "         \"windForce\": 0.5,\n" +
        "         \"noiseScale\": 1,\n" +
        "         \"windFrequency\": 1\n" +
        "       }\n" +
        "    },\n";


        

        // end string constructor and return the value as as a string
        return value;

    }

    public string GetAllObjects()
    {
        string allObjects;
        allObjects = GetObject(0);
          //GetObject(0) + "," 
        //+ GetObject(1) + ","
        //+ GetObject(2) + ","
        //+ GetObject(4);
        // end trait function
        return allObjects;

    }

    public void CreateTextFile(){

        // name it
        string txtDocumentName = Application.streamingAssetsPath + "/WEBA_Output/" + "/WEBA_Output" + exportNumber + ".txt";

        // check to see if exists
        if (!File.Exists(txtDocumentName))
        {
            // Write to File
            File.WriteAllText(txtDocumentName, webaJSONOutputPreview);
        }
    }
}
