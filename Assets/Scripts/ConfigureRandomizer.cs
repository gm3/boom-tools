using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRM;
//using VRM.RuntimeExporterSample;

public class ConfigureRandomizer : MonoBehaviour
{


    public int totalNFTs;
    public DNAManager dnaManagerReference;
    public RandomizeAll randomizeAllScriptReference;
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
    public string vrmVersion = "0.x";

    // Start is called before the first frame update
    void Start()
    {
         Button btn = buttonReference.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void TaskOnClick()
    {
        // Generate NFts SLow
        delayCoroutine = StartCoroutine(GenerateAllNFTsSlow(totalNFTs));

        // Generate NFTs Fast
		//GenerateAllNFTs(totalNFTs);
    }


    void GenerateAllNFTs(int totalNFTs){ 
        
        for (int i = 0; i < totalNFTs; i++) {
               

                    if(!dnaManagerReference.DNAList.Contains(dnaManagerReference.DNACode)){

                        randomizeAllScriptReference.RamdomizeAll();
                    }else{
                        randomizeAllScriptReference.RamdomizeAll();
                    }

                    if (exportVRMFromRandomTrait != null)
                        modelToExportToVRM = exportVRMFromRandomTrait.selectedObject as GameObject;
                    cameraCaptureReference.Capture();
                    toTxtFileRef.CreateTextFile();
                    vrmRuntimeExporterRef.Export(modelToExportToVRM, true, dnaManagerReference.genID.ToString());

                    //StartCoroutine("WaitForAnAmoutOfTime");
        }
        
    }

    IEnumerator GenerateAllNFTsSlow(int totalNFTs)
    {

        VRMMetaObject metaData = ScriptableObject.CreateInstance<VRMMetaObject>();
        metaData.Title = vrmTitle;
        metaData.Version = vrmVersion;
        metaData.Author = vrmAuthor;
        metaData.ContactInformation = vrmContactInformation;
        metaData.Reference = vrmReference;

        VRMMeta metaComponent = modelToExportToVRM.AddComponent<VRMMeta>();
        metaComponent.Meta = metaData;

        //reset GenID
        dnaManagerReference.genID = 0;

            for (int i = 0; i < totalNFTs; i++) {
               

                    if(!dnaManagerReference.DNAList.Contains(dnaManagerReference.DNACode)){

                        randomizeAllScriptReference.RamdomizeAll();
                    }else{
                        randomizeAllScriptReference.RamdomizeAll();
                    }

                    cameraCaptureReference.Capture();
                    toTxtFileRef.CreateTextFile();
                    if (exportVRMFromRandomTrait != null)
                        modelToExportToVRM = exportVRMFromRandomTrait.selectedObject as GameObject;
                    vrmRuntimeExporterRef.Export(modelToExportToVRM, true, dnaManagerReference.genID.ToString());
                    //gltfExporterRef.Export(ITextureSerializer textureSerializer);
                    yield return new WaitForSeconds(delaySpeed);
        }
            
    }
}
