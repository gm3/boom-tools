using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RandomizeAll : MonoBehaviour
{
    public GameObject parentRandomTraitCaller;

    public List<WeightedValue> weightedValues;
    public PrintRandomValue[] randomScriptReferences;
    public BGColorRandomizer randomBGScriptReference;
    public BGColorRandomizer randomBodyTextureScriptReferences;
    public Button buttonReference;
    public DNAManager dnaManagerReference;
    
    // Start is called before the first frame update
    void Start()
    {
        Button btn = buttonReference.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
    RamdomizeAll();

    }

    // Update is called once per frame
    void Update()
    {
        


    }

    void TaskOnClick()
    {
		//Debug.Log ("You have randomized All!"); 
      
      
        if(!dnaManagerReference.DNAList.Contains(dnaManagerReference.DNACode)){
          RamdomizeAll();
        }else{
          RamdomizeAll();
        }

        
    }
                  
	public void RamdomizeAll(){
        string newTraits = "";
        if (parentRandomTraitCaller != null)
            newTraits = RandomizeParentGetJsonData();
        randomScriptReferences[0].RandomCheck();
        randomScriptReferences[1].RandomCheck();
        randomScriptReferences[2].RandomCheck();
        randomScriptReferences[3].RandomCheck();
        randomScriptReferences[4].RandomCheck();
        randomScriptReferences[5].RandomCheck();
        randomScriptReferences[6].RandomCheck();
        randomBGScriptReference.RandomCheck();
        randomBodyTextureScriptReferences.RandomCheck();
        dnaManagerReference.ExportJsonToText(newTraits);
    }

    private string RandomizeParentGetJsonData()
    {
        ActionCaller[] traits = parentRandomTraitCaller.GetComponentsInChildren<ActionCaller>(true);
        string result = "";
        foreach (ActionCaller t in traits)
        {
            t.SetRandomTrait();
            result += t.GetJsonedObject(true,1);
        }
        return result.Substring(0, result.Length - 2) + "\n";
    }
}
