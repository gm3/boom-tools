using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class RandomRoll : MonoBehaviour
{
    public RandomizeAll ramdomizeAllRef;
    public int totalSpins;
    private int lengthOfSpin = 3;
    public float delaySpeed = 0.01f;
    public Button buttonReference;
    private Coroutine rollCoroutine;
     public DNAManager dnaManagerReference;
     public AudioSource audioSource;
     public int startingPitch = 1;
     public int timeToDecrease = 5;
     public AudioClip progressSound;
    


    // Start is called before the first frame update
    void Start()
    {
        Button btn = buttonReference.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick); 
        audioSource.pitch = startingPitch;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TaskOnClick()
    {
        // Generate NFts SLow
        rollCoroutine = StartCoroutine(Roll(totalSpins));

        // Generate NFTs Fast
		//GenerateAllNFTs(totalNFTs);
    }

    IEnumerator Roll(int totalSpins)
    {
        dnaManagerReference.genID = 0;
        delaySpeed = 0.01f;
        audioSource.pitch = startingPitch;

            for (int i = 0; i < totalSpins; i++) {
               
                if(delaySpeed != 0){
                        audioSource.pitch += .02F;
                
                    if(!dnaManagerReference.DNAList.Contains(dnaManagerReference.DNACode)){

                        ramdomizeAllRef.RamdomizeAll();
                        audioSource.PlayOneShot(progressSound, .07f);
                    }else{
                        ramdomizeAllRef.RamdomizeAll();
                        audioSource.PlayOneShot(progressSound, .07f);
                    }

                    delaySpeed += .0009F;// / totalSpins;
                }
                    //cameraCaptureReference.Capture();
                    //toTxtFileRef.CreateTextFile();
                    //vrmRuntimeExporterRef.Export(modelToExportToVRM, true, dnaManagerReference.genID.ToString());
                    //gltfExporterRef.Export(ITextureSerializer textureSerializer);
                    yield return new WaitForSeconds(delaySpeed);
        }
            
    }
}
