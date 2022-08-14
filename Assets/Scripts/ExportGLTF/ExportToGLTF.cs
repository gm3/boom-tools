using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UniGLTF;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VRMShaders;
using Plattar;

public class ExportToGLTF : MonoBehaviour
{
    //public DNAManager dnaIDReference;
    public Button buttonReference;
    public GameObject gameObjectToExport;
    public gltfExporter gltfExporterReference;
   
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



    void TaskOnClick(){
		//Debug.Log ("You have exported a GLTF"); 
        // export glTF
        //gltfExporterReference.Export(textureSerializer);
                  
	}

    

}
