using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class DNAManager : MonoBehaviour
{
    public OptionsManager optionsManager;
    public VRMAuthoringManager vrmAuthoringManager;

    public PrintRandomValue[] randomScriptReferences;
    public BGColorRandomizer randomBGScriptReferences;
    public BGColorRandomizer randomBodyTextureScriptReferences;
    public BGColorRandomizer randomBoomboxTextureScriptReferences;
    public BGColorRandomizer randomBorderTextureScriptReferences;
    public PoseRandomizer randomPoseScriptReferences;

    public RandomizeAll randomizeAllScriptReference;
    public SuperRule superRuleReference;
   
    public string[] allLayerTraits;
    //public string BGColorTrait;
    public string description;
    public string name;
    public string externalUrl;
    public string ipfsUrl;
    public string vrmUrl;
    public string createdBy;
    public string aniamtionUrl;

    [TextArea(10,10)]
    public string jsonOutputPreview;
    public int genID;
    public TextMeshProUGUI genIDLabel;

    public TextMeshProUGUI JSONBody;
    public TextMeshProUGUI[] traitLabel;
    public TextMeshProUGUI BGTraitLabel;
    public TextMeshProUGUI BGTraitValue;

    public TextMeshProUGUI DescriptionTraitLabel;
    public TextMeshProUGUI DescriptionTraitValue;
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
        DescriptionTraitLabel.text = "Description";
        DescriptionTraitValue.text = description;
        genIDLabel.text = genID.ToString();
        URLTraitValue.text = externalUrl.ToString();
        
    }

    public void ExportJsonToText(string attributes = "")
    {
       
        jsonOutputPreview = "{ \n" + 
        //"\"editionId\": " + "\"" + (genID+1).ToString() + "\",\n" +
        "\"name\": " + "\"" + name + " #" + (genID+1).ToString() + "\",\n" +
        "\"created_by\": " + "\"" + createdBy + "\",\n" +
        "\"external_url\": " + "\"" + externalUrl + "\",\n" +
        "\"description\": " + "\"" + description + "\",\n" +
        "\"vrm_url\": " + "\"" + vrmUrl + "/boomboxhead" + (genID+1).ToString() + ".vrm" + "\",\n" +
        "\"animation_url\": " + "\"" + aniamtionUrl + "/boomboxhead" + (genID+1).ToString() + ".glb" + "\",\n" +
        "\"image\": " + "\"" + ipfsUrl + "/boomboxhead" + (genID+1).ToString() + ".jpg" + "\",\n" +
        
        
        
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

        "\t}";
        //"\t}\n";
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

        "\t}";
        //"\t}\n";
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

        "\t}";
        //"\t}\n";
        // end trait function
        string traitDNA = randomBGScriptReferences.traitType + randomBGScriptReferences.currentEntryValue;
        DNACode += traitDNA;
        return value;

    }

    public string GetBoomboxColorTrait()
    {
        string value;
        value = "\t{\n" +

        // trait type
        "\t\t\"trait_type\"" + ": \"" + 
        randomBoomboxTextureScriptReferences.traitType  
        + "\",\n" +

        // value
        "\t\t\"value\"" + ": \"" + 
        randomBoomboxTextureScriptReferences.currentEntryValue 
        + "\"\n" +

        "\t}";
        //"\t}\n";
        // end trait function
        string traitDNA = randomBoomboxTextureScriptReferences.traitType + randomBoomboxTextureScriptReferences.currentEntryValue;
        DNACode += traitDNA;
        return value;

    }

    public string GetPoseTrait()
    {
        string value;
        value = "\t{\n" +

        // trait type
        "\t\t\"trait_type\"" + ": \"" + 
        randomPoseScriptReferences.traitType  
        + "\",\n" +

        // value
        "\t\t\"value\"" + ": \"" + 
        randomPoseScriptReferences.currentEntryValue 
        + "\"\n" +

        "\t}";
        //"\t}\n";
        // end trait function
        string traitDNA = randomPoseScriptReferences.traitType + randomPoseScriptReferences.currentEntryValue;
        DNACode += traitDNA;
        return value;

    }

    public string GetBorderTrait()
    {
        string value;
        value = "\t{\n" +

        // trait type
        "\t\t\"trait_type\"" + ": \"" + 
        randomBorderTextureScriptReferences.traitType  
        + "\",\n" +

        // value
        "\t\t\"value\"" + ": \"" + 
        randomBorderTextureScriptReferences.currentEntryValue 
        + "\"\n" +

        "\t}";
        //"\t}\n";
        // end trait function
        string traitDNA = randomBorderTextureScriptReferences.traitType + randomBorderTextureScriptReferences.currentEntryValue;
        DNACode += traitDNA;
        return value;

    }

    public string GetAllTraits()
{
    string allTraits = "";
    int attributeCount = 0;
    if (superRuleReference.CheckSuperRule1())
    {
        for (int i = 0; i < randomScriptReferences.Length; i++)
        {
            if (randomScriptReferences[i].currentEntryValue != "0")
            {
                if (attributeCount > 0)
                {
                    allTraits += ",\n";
                }
                allTraits += GetTrait(i);
                attributeCount++;
            }
        }
        if (randomBGScriptReferences.currentEntryValue != "0")
        {
            if (attributeCount > 0)
            {
                allTraits += ",\n";
            }
            allTraits += GetBGColorTrait();
            attributeCount++;
        }
        if (randomPoseScriptReferences.currentEntryValue != "0")
        {
            if (attributeCount > 0)
            {
                allTraits += ",\n";
            }
            allTraits += GetPoseTrait();
            attributeCount++;
        }
        if (randomBodyTextureScriptReferences.currentEntryValue != "0")
        {
            if (attributeCount > 0)
            {
                allTraits += ",\n";
            }
            allTraits += GetRobotColorTrait();
            attributeCount++;
        }
        if (randomBoomboxTextureScriptReferences.currentEntryValue != "0")
        {
            if (attributeCount > 0)
            {
                allTraits += ",\n";
            }
            allTraits += GetBoomboxColorTrait();
            attributeCount++;
        }
        if (randomBorderTextureScriptReferences.currentEntryValue != "0")
        {
            if (attributeCount > 0)
            {
                allTraits += ",\n";
            }
            allTraits += GetBorderTrait();
            attributeCount++;
        }
    }
    else
    {
        for (int i = 0; i < randomScriptReferences.Length; i++)
        {
          if (randomScriptReferences[i].currentEntryValue != "0")
            {
                if (attributeCount > 0)
                {
                    allTraits += ",\n";
                }
                allTraits += GetTrait(i);
                attributeCount++;
            }
        }
        if (randomBGScriptReferences.currentEntryValue != "0")
        {
            if (attributeCount > 0)
            {
                allTraits += ",\n";
            }
            allTraits += GetBGColorTrait();
            attributeCount++;
        }
        if (randomPoseScriptReferences.currentEntryValue != "0")
        {
            if (attributeCount > 0)
            {
                allTraits += ",\n";
            }
            allTraits += GetPoseTrait();
            attributeCount++;
        }
        if (randomBodyTextureScriptReferences.currentEntryValue != "0")
        {
            if (attributeCount > 0)
            {
                allTraits += ",\n";
            }
            allTraits += GetRobotColorTrait();
            attributeCount++;
        }
        if (randomBoomboxTextureScriptReferences.currentEntryValue != "0")
        {
            if (attributeCount > 0)
            {
                allTraits += ",\n";
            }
            allTraits += GetBoomboxColorTrait();
            attributeCount++;
        }
        if (randomBorderTextureScriptReferences.currentEntryValue != "0")
        {
            if (attributeCount > 0)
            {
                allTraits += ",\n";
            }
            allTraits += GetBorderTrait();
            attributeCount++;
        }
    }
    //  assign the DNACode generated to the currentDNA string to be stored in a list assigned to the genID
    string CurrentDNA = DNACode;
    if (!DNAList.Contains(CurrentDNA))
    {
        DNAList.Add(CurrentDNA);
    }
    Debug.Log("CurrentDNA is " + CurrentDNA);
    // Clear DNA Code
    DNACode = "";
    return allTraits;
}
   
}




 