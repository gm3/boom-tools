using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
 
public class ImportMaterialsConfig : MonoBehaviour
{
    public List<WeightedValue> weightedValues;
    public Material[] totalMaterials;
    public string[] layerStringData;
    public TextMeshProUGUI traitLayerText;
    public string traitType;
    public string currentEntryValue; 
    public DNAManager dnaManagerReference;
    public GameObject ObjectToChangeMatReference;
 
    
    void Start(){
 
    }


}