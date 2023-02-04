using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PoseRandomizer : MonoBehaviour
{
    public List<AnimationClip> animationClips;
    public List<WeightedValue> weightedValues;
    public Button buttonReference;
    public Animator animator;
    public DNAManager dnaManagerReference;
    public string traitType;
    public string[] layerStringData;
    public TextMeshProUGUI traitLayerText;
    public string currentEntryValue; 
    public string currentPose;

    void Start(){
        Button btn = buttonReference.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void Update(){
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
        int randomIndex = Random.Range(0, animationClips.Count);
        AnimationClip randomClip = animationClips[randomIndex];
        animator.Play(randomClip.name);
        currentPose = randomClip.name;
        dnaManagerReference.ExportJsonToText();
    }
    
   public void RandomCheck(){
    string randomValue = GetRandomValue(weightedValues);
    AnimationClip randomClip = animationClips.Find(clip => clip.name == randomValue);
    animator.SetTrigger(randomValue);
    currentPose = randomClip.name;
}

    public string PrintDNA(string dna){
  
        Debug.Log(currentEntryValue);      
        return dna;
              
	}
}