using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
 
public class BoomboxColorRandomizer : MonoBehaviour
{
    public List<WeightedValue> weightedValues;
    public Button buttonReference;
    public Material[] totalMaterials;
    public string[] layerStringData;
    public TextMeshProUGUI traitLayerText;
    public string traitType;
    public string currentEntryValue; 
    public DNAManager dnaManagerReference;
    public GameObject ObjectToChangeMatReference;
    private string randomValue;
    


    void Start(){
        Button btn = buttonReference.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
        randomValue = GetRandomValue(weightedValues);
        for(int i = 0; i < totalMaterials.Length; i++)
        {
           if(randomValue == layerStringData[i]){
                ObjectToChangeMatReference.GetComponent<Renderer>().material = totalMaterials[i];
            }
        }
    }

    void Update()
    {
    traitLayerText.text =  currentEntryValue;
    }
 


    string GetRandomValue(List<WeightedValue> weightedValueList)
    {
        string output = null;
 
        //Getting a random weight value
        var totalWeight = 0;
        foreach (var entry in weightedValueList)
        {
            totalWeight += entry.weight;
        }
        var rndWeightValue = Random.Range(1, totalWeight + 1);
 
        //Checking where random weight value falls
        var processedWeight = 0;
        foreach (var entry in weightedValueList)
        {
            processedWeight += entry.weight;
            if(rndWeightValue <= processedWeight)
            {
                output = entry.value;
                currentEntryValue = entry.value;
                break;
            }
        }
        traitLayerText.text =  currentEntryValue;
 
        return output;
    }

    void TaskOnClick(){
        Debug.Log ("You have clicked the button!"); 
        string randomValue = GetRandomValue(weightedValues);
        Debug.Log(randomValue ?? "No entries found");
        

        for(int i = 0; i < totalMaterials.Length; i++)
        {
           if(randomValue == layerStringData[i]){
                ObjectToChangeMatReference.GetComponent<Renderer>().material = totalMaterials[i];
            }
        }

        
        dnaManagerReference.ExportJsonToText();

            
	}

    public void RandomCheck(){
        string randomValue = GetRandomValue(weightedValues);
        Debug.Log(randomValue ?? "No entries found");

            for(int i = 0; i < totalMaterials.Length; i++)
            {
           if(randomValue == layerStringData[i]){
                ObjectToChangeMatReference.GetComponent<Renderer>().material = totalMaterials[i];  
            }
            }

	}

    /* public string PrintDNA(string dna){
  
        //Debug.Log(currentEntryValue);      
        return dna;    
	} */
}