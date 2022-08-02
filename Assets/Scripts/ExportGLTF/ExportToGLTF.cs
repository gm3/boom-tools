using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
//using GLTFast.Export;


public class ExportToGLTF : MonoBehaviour
{
    //public DNAManager dnaIDReference;
    public Button buttonReference;
    public GameObject gameObjectToExport;

    [SerializeField]
    string path;
    
    // Start is called before the first frame update
    void Start()
    {
        // create a folder
           Directory.CreateDirectory(Application.streamingAssetsPath + "/GLTF_Output/");

            // Reference to Button
            Button btn = buttonReference.GetComponent<Button>();
		    btn.onClick.AddListener(TaskOnClick);
        
    }

    // Update is called once per frame
    void Update()
    {
   
    }

    /* public void CreateTextFile(){

        // name it
        string txtDocumentName = Application.streamingAssetsPath + "/JSON_Output" + dnaIDReference.genID + ".txt";

        // check to see if exists
        if (!File.Exists(txtDocumentName))
        {
            // Write to File
            File.WriteAllText(txtDocumentName, dnaIDReference.jsonOutputPreview);
        }
    } */

    void TaskOnClick(){
		//Debug.Log ("You have exported a GLTF"); 
       
        /* if(dnaIDReference.jsonOutputPreview == ""){
            return;
        } */
       
        // export glTF
        //SimpleExport();
                  
	}

    

    async void SimpleExport() {

        // name it
        //string gltfDocumentName = Application.streamingAssetsPath + "/GLTF_Output" + dnaIDReference.genID + ".txt";

        // Example of gathering GameObjects to be exported (recursively)
        //var rootLevelNodes = GameObject.FindGameObjectsWithTag("ExportMe");
        
        // GameObjectExport lets you create glTFs from GameObject hierarchies
        //var export = new GameObjectExport();

        // Add a scene
       // export.AddScene(rootLevelNodes);

        // Async glTF export
        //bool success = await export.SaveToFileAndDispose(path);

       /*  bool success = await export.SaveToFileAndDispose(Application.streamingAssetsPath + "/GLTF_Output/" + gameObjectToExport.name + ".gltf"); */

        //if(!success) {
            //Debug.LogError("Something went wrong exporting a glTF");
        //}
    }
}
