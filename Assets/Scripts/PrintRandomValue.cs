using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
 
public class PrintRandomValue : MonoBehaviour
{
    public List<WeightedValue> weightedValues;
    //public static List weightedValues = new List();
    public Button buttonReference;
    public GameObject[] totalObjectsInLayer;
    public string[] layerStringData;
    public TextMeshProUGUI traitLayerText;
    public string traitType;
    public string currentEntryValue; 
    public DNAManager dnaManagerReference;
    


    void Start(){
        Button btn = buttonReference.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
        
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
 
        return output;
    }

    void TaskOnClick(){
		Debug.Log ("You have clicked the button!"); 
        string randomValue = GetRandomValue(weightedValues);
        Debug.Log(randomValue ?? "No entries found");
        

        for(int i = 0; i < totalObjectsInLayer.Length; i++)
        {
           if(randomValue == layerStringData[i]){
                totalObjectsInLayer[i].SetActive(true);  
            }else{
                totalObjectsInLayer[i].SetActive(false);  
            }
        }

        dnaManagerReference.ExportJsonToText();

            
	}

    public void RandomCheck(){
        string randomValue = GetRandomValue(weightedValues);
        Debug.Log(randomValue ?? "No entries found");
        

            for(int i = 0; i < totalObjectsInLayer.Length; i++)
            {
           if(randomValue == layerStringData[i]){
                totalObjectsInLayer[i].SetActive(true);  
            }else{
                totalObjectsInLayer[i].SetActive(false);  
            }
            }

              
	}

    public string PrintDNA(string dna){
  
        Debug.Log(currentEntryValue);      
        return dna;
              
	}
}