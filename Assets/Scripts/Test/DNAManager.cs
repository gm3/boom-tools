using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class DNAManager : MonoBehaviour
{
    public PrintRandomValue[] randomScriptReferences;
    public BGColorRandomizer randomBGScriptReferences;
    public BGColorRandomizer randomBodyTextureScriptReferences;
    public SuperRule superRuleReference;
   
    public string[] allLayerTraits;
    //public string BGColorTrait;
    public string description;
    public string name;
    public string externalUrl;
    public string ipfsUrl;

    [TextArea(10,10)]
    public string jsonOutputPreview;
    public int genID;
    public TextMeshProUGUI genIDLabel;

    public TextMeshProUGUI JSONBody;
    public TextMeshProUGUI[] traitLabel;
    public TextMeshProUGUI BGTraitLabel;
    public TextMeshProUGUI BGTraitValue;
    public TextMeshProUGUI URLTraitValue;
    public string DNACode;
    public List<string> DNAList = new List<string>();

    

  
    void Start()
    {
        // define label text
         for(int i = 0; i < allLayerTraits.Length; i++)
        {
           
           allLayerTraits[i] = randomScriptReferences[i].currentEntryValue;

            traitLabel[i].text = randomScriptReferences[i].traitType;
            
        }

        //List<DNA> DNAs = new List<DNA>();
        //BGTraitLabel.text = randomBGScriptReferences.traitType;
    }


      void Update()
    {
         for(int i = 0; i < allLayerTraits.Length; i++)
        {
           
           allLayerTraits[i] = randomScriptReferences[i].currentEntryValue;
           
        }  

        BGTraitLabel.text = randomBGScriptReferences.traitType;
        BGTraitValue.text = randomBGScriptReferences.currentEntryValue;
        genIDLabel.text = genID.ToString();
        URLTraitValue.text = externalUrl.ToString();
        
    }

    public void ExportJsonToText(string attributes = "")
    {
       
        jsonOutputPreview = "{ \n" + 
        //"\"editionId\": " + "\"" + (genID+1).ToString() + "\",\n" +
        "\"description\": " + "\"" + description + "\",\n" +
        "\"external_url\": " + "\"" + externalUrl + "\",\n" +
        "\"image\": " + "\"" + ipfsUrl + "/imagefile" + (genID+1).ToString() + ".png" + "\",\n" +
        "\"name\": " + "\"" + name + " #" + (genID+1).ToString() + " \",\n" +
        
        
        // open attributes array
        "\"attributes\": [\n"



        // get all traits
        + (attributes != "" ? attributes : GetAllTraits()) + 

        // close aatribute array
        "\t]\n" +
        "}";


        genID++;
        JSONBody.text = jsonOutputPreview;

    }

    // Trait object constructor
    public string GetTrait(int traitIndex)
    {
        string value;
        value = "\t{\n" +

        // trait type
        "\t\t\"trait_type\"" + ": \"" + 
        randomScriptReferences[traitIndex].traitType  
        + "\",\n" +

        // value
        "\t\t\"value\"" + ": \"" + 
        randomScriptReferences[traitIndex].currentEntryValue 
        + "\"\n" +

        "\t}\n";
        // end trait function

        //set DNA value
        
        string traitDNA = randomScriptReferences[traitIndex].traitType + randomScriptReferences[traitIndex].currentEntryValue;
        DNACode += traitDNA;
        
        return value;


    }

    public string GetBGColorTrait()
    {
        string value;
        value = "\t{\n" +

        // trait type
        "\t\t\"trait_type\"" + ": \"" + 
        randomBodyTextureScriptReferences.traitType  
        + "\",\n" +

        // value
        "\t\t\"value\"" + ": \"" + 
        randomBodyTextureScriptReferences.currentEntryValue 
        + "\"\n" +

        "\t}\n";
        // end trait function

        string traitDNA = randomBodyTextureScriptReferences.traitType + randomBodyTextureScriptReferences.currentEntryValue;
        DNACode += traitDNA;
        return value;

    }

    public string GetRobotColorTrait()
    {
        string value;
        value = "\t{\n" +

        // trait type
        "\t\t\"trait_type\"" + ": \"" + 
        randomBGScriptReferences.traitType  
        + "\",\n" +

        // value
        "\t\t\"value\"" + ": \"" + 
        randomBGScriptReferences.currentEntryValue 
        + "\"\n" +

        "\t}\n";
        // end trait function
        string traitDNA = randomBGScriptReferences.traitType + randomBGScriptReferences.currentEntryValue;
        DNACode += traitDNA;
        return value;

    }

    public string GetAllTraits()
    {
        string allTraits;
        
        // check if superrule 1 is true or false
        if(superRuleReference.CheckSuperRule1()){

        allTraits = 
          GetTrait(0) + "," 
        + GetTrait(1) + ","
        + GetTrait(2) + ","
        + GetTrait(3) + ","
        + GetTrait(4) + ","
        + GetTrait(5) + ","
        + GetTrait(6) + ","
        + GetRobotColorTrait() + ","
        + GetBGColorTrait();
        // end trait function
        }else{

        allTraits = 
          GetTrait(0) + "," 
        + GetTrait(1) + ","
        + GetTrait(2) + ","
        + GetTrait(3) + ","
        + GetTrait(4) + ","
        + GetTrait(5) + ","
        + GetTrait(6) + ","
        + GetRobotColorTrait() + ","
        + GetBGColorTrait();

        }

        //  assign the DNACode generated to the currentDNA string to be stored in a list assigned to the genID
        string CurrentDNA = DNACode;
        if(!DNAList.Contains(CurrentDNA)){
            DNAList.Add(CurrentDNA);
        }
    
        Debug.Log ("CurrentDNA is " + CurrentDNA); 

        /// Clear DNA Code
        DNACode = "";

        return allTraits;

        /* string allTraits;
        allTraits = 
          GetTrait(0) + "," 
        + GetTrait(1) + ","
        + GetTrait(2) + ","
        + GetTrait(3) + ","
        + GetTrait(4) + ","
        + GetTrait(5) + ","
        + GetTrait(6) + ","
        + GetRobotColorTrait() + ","
        + GetBGColorTrait();
        // end trait function
        return allTraits; */

    }

    /* public string CreateDNAString(){
        string DNA;
        
        if(superRuleReference.CheckSuperRule1()){

        DNA = 
          GetTraitDNA(0) + "," 
        + GetTraitDNA(1) + ","
        + GetTraitDNA(2) + ","
        + GetTraitDNA(3) + ","
        + GetTraitDNA(4) + ","
        + GetTraitDNA(5) + ","
        + GetTraitDNA(6) + ","
        + GetRobotColorTraitDNA() + ","
        + GetBGColorTraitDNA();
        // end trait function
        }else{

        DNA = 
          GetTrait(0) + "," 
        + GetTrait(1) + ","
        + GetTrait(2) + ","
        + GetTrait(3) + ","
        + GetTrait(4) + ","
        + GetTrait(5) + ","
        + GetTrait(6) + ","
        + GetRobotColorTrait() + ","
        + GetBGColorTrait();

        }



        return DNA;
    } */

   
}




 