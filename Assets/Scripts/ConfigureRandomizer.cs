using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public VRMRuntimeExporter vrmRuntimeExporterRef;
    public GameObject modelToExportToVRM;
    //public gltfExporter gltfExporterRef;

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

                    cameraCaptureReference.Capture();
                    toTxtFileRef.CreateTextFile();
                    vrmRuntimeExporterRef.Export(modelToExportToVRM, true, dnaManagerReference.genID.ToString());

                    //StartCoroutine("WaitForAnAmoutOfTime");
        }
        
    }

    IEnumerator GenerateAllNFTsSlow(int totalNFTs)
    {
            for (int i = 0; i < totalNFTs; i++) {
               

                    if(!dnaManagerReference.DNAList.Contains(dnaManagerReference.DNACode)){

                        randomizeAllScriptReference.RamdomizeAll();
                    }else{
                        randomizeAllScriptReference.RamdomizeAll();
                    }

                    cameraCaptureReference.Capture();
                    toTxtFileRef.CreateTextFile();
                    vrmRuntimeExporterRef.Export(modelToExportToVRM, true, dnaManagerReference.genID.ToString());
                    //gltfExporterRef.Export(ITextureSerializer textureSerializer);
                    yield return new WaitForSeconds(delaySpeed);
        }
            
    }
}
