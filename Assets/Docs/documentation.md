# Boom-Tools Documentation
## All Classes / Scripts

### Class-level attributes 

<details>
<summary>Click here to see the details and code</summary>
Description: Attributes are declarative tags that can be applied to certain language elements, such as classes or methods. They can be used to specify metadata about the element, such as how it should be serialized or whether it is visible to certain tools.

(such as the [Serializable] attribute) or other metadata that is associated with your class. 

###### Examples:

```c# 
[Serializable]
public class MyClass
{
    // class implementation goes here
} 
```
</details>

---


### CodebaseConstructor.cs
##### Component name: ```public class CodebaseConstructor : MonoBehaviour```
<details>
<summary>Click here to see the details and code</summary>
    
The `Start` function is called when the script is first initialized. It gets a reference to a `Button` object and adds a listener for clicks on the button. It also gets a list of all the files in the `Application.dataPath` directory with the `.cs` file extension and stores them in the `files` array. It also creates a new directory in the `Application.streamingAssetsPath` directory.

The `SearchAllScripts` function reads through each `file` in the `files` array, reads all the lines in each file, and looks for lines that start with `public class` or `private class`. If it finds such a line, it extracts the class name from the line and stores it in the `className` variable. It then adds an entry to the `codebase` dictionary where the key is the class name and the value is a new empty dictionary.

The `GenerateMarkdown` function takes the `codebase` dictionary as an argument and generates a string in Markdown format. It does this by iterating over the entries in the `codebase` dictionary and adding a heading for each entry to the `markdown` string. The heading level is specified by the `headingLevel` argument.

The `TaskOnClick` function is called when the button that was set up in the `Start` function is clicked. It calls the `SearchAllScripts` function and then generates a Markdown string by calling the `GenerateMarkdown` function. It then calls the `CreateTextFile` function, which creates a new text file in the directory specified by `Application.streamingAssetsPath + "/Code_Output/"`, and writes the `markdown` string to the file. If an error occurs while writing the file, it is logged to the console. 
    

```c#
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class CodebaseConstructor : MonoBehaviour
{
    public Button buttonReference;
    public string[] files;
    public string markdown = "";
    public string filePath = "";
    //private int exportNumber;
    public string className;
    Dictionary<string, Dictionary<string, List<string>>> codebase = new Dictionary<string, Dictionary<string, List<string>>>();

    void Start()
    {
        Button btn = buttonReference.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick); 
        files = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);
        Directory.CreateDirectory(Application.streamingAssetsPath + "/Code_Output/");
    }

    void Update()
    {
        //Debug.Log(files); 
    }

    public void SearchAllScripts()
    {
        foreach (string file in files)
        {
            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines)
            {
                  if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                        if (line.TrimStart().StartsWith("public class") || line.TrimStart().StartsWith("private class")  )
                        {
                            className = line.Split(' ')[2];
                            codebase[className] = new Dictionary<string, List<string>>();
                        }
            }
        }
      Debug.Log(codebase.ToString());
    }


    string GenerateMarkdown(Dictionary<string, Dictionary<string, List<string>>> codebase, int headingLevel)
    {
        Debug.Log(codebase);
        string markdown = "";
            
        foreach (KeyValuePair<string, Dictionary<string, List<string>>> entry in codebase)
        {
        
            string heading = "";
            for (int i = 0; i < headingLevel; i++) heading += "#";
            heading += " " + entry.Key;
            markdown += heading + "\n";

        }
        return markdown;
    }

    void TaskOnClick()
    {
		SearchAllScripts();
        string filePath = Application.streamingAssetsPath + "/Code_Output/output.txt";
        markdown = GenerateMarkdown(codebase, 3);
        CreateTextFile();
   
	}

    void CreateTextFile(){

        
        try
        {
            File.WriteAllText(Application.streamingAssetsPath + "/Code_Output/output.txt", markdown);
            Debug.Log ("You have created a text file of the codebase"); 
            Debug.Log(markdown);
        }
        catch (Exception e)
        {
            Debug.LogError("Error writing file: " + e.Message);
        }
        
        
    }
}


```
                                             
</details>

---
    
### GameModeController.cs
##### Component name: ```public class GameModeController : MonoBehaviour```
    
<details>
<summary>Click here to see the details and code</summary>
In summary, this script controls the game mode of the game and updates the UI and other game objects based on the current game mode.

It has a public `GameMode` enum that defines two possible game modes: `Random` and `Import`.

The script has a `GameModeController` class that derives from the `MonoBehaviour` class. This allows the script to be attached to a game object in the Unity scene and receive updates at each frame. The class has a number of public fields that reference other game objects and scripts in the scene.

The `Start` function is called when the script is first initialized and sets the `currentGameMode` field to the `Random` game mode. It also sets the text of a `TextMeshProUGUI` object to the string representation of the `currentGameMode` field.

The `SwitchGameMode` function is a public function that allows other scripts to change the `currentGameMode` field. It takes a `GameMode` argument and sets the `currentGameMode` field to the value of this argument.

The `Update` function is called once per frame and updates the game based on the current value of the `currentGameMode` field. If the `currentGameMode` is `Random`, it activates the `RandomScriptsReference` game object, deactivates the `ImportModeJSONUIReference` game object, and disables the `ImportJSONScriptsReference` script. It also sets the text of the `modeValueTextReference` object to the string representation of the `currentGameMode` field. If the `currentGameMode` is `Import`, it does the opposite.
 
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameMode
{
    Random,
    Import
}

public class GameModeController : MonoBehaviour
{
    public GameMode currentGameMode;
    public GameObject RandomScriptsReference;
    public GameObject RandomModeUIReference;
    public GameObject ImportModeJSONUIReference;
    public ImportJSON ImportJSONScriptsReference;
    public TextMeshProUGUI modeValueTextReference;

    void Start()
    {
        currentGameMode = GameMode.Random;
        modeValueTextReference.text = currentGameMode.ToString();
    }

    public void SwitchGameMode(GameMode newGameMode)
    {
        currentGameMode = newGameMode;
    }

    void Update()
    {
        switch (currentGameMode)
        {
            case GameMode.Random:
                // Show UI for Mode 1
                RandomScriptsReference.SetActive(true);
                RandomModeUIReference.SetActive(true);
                ImportModeJSONUIReference.SetActive(false);
                ImportJSONScriptsReference.enabled = false;
                modeValueTextReference.text = currentGameMode.ToString();
                break;
            case GameMode.Import:
                // Show UI for Mode 2
                RandomScriptsReference.SetActive(false);
                RandomModeUIReference.SetActive(false);
                ImportModeJSONUIReference.SetActive(true);
                ImportJSONScriptsReference.enabled = true;
                modeValueTextReference.text = currentGameMode.ToString();
                
                break;
        }
    }
}
    
```
</details>    


    
### AllSceneData.cs
##### Component name: ```AllSceneData```
    
<details>
<summary>Click here to see the details and code</summary>

It has a `Start` function that gets a reference to a `Button` object and adds a listener for clicks on the button. When the button is clicked, the `TaskOnClick` function is called, which logs a message to the console and calls the `GatherAllSceneData` function. The `GatherAllSceneData` function finds all the active `GameObjects` in the scene, prints their `names` to the console, and returns them as an array.
  
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class AllSceneData : MonoBehaviour
{
   
    private string[] allObjectNames;
    private string[] allComponentNames;
    private string[] allParamaters;

     public Button buttonReference;
    

    // Start is called before the first frame update
    void Start()
    {

        // Reference to Button
        Button btn = buttonReference.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);    

    }

    // Update is called once per frame
    void Update()
    {
    }

    void TaskOnClick()
    {
		Debug.Log ("You have clicked the button"); 
        GatherAllSceneData();
    }

    public GameObject[] GatherAllSceneData(){

        ArrayList list = new ArrayList(FindObjectsOfType(typeof(GameObject)));
        GameObject[] allObjects = (GameObject[]) list.ToArray(typeof(GameObject));
        
        for (int i = 0; i < allObjects.Length; i++)
        {
           
            print(allObjects[i].name +" is an active object") ;
    
        }

        return allObjects;

    }
}
                                            
```
                                              
</details>  
    
---   
    
### AnimationSwapper.cs
##### Component name: ```AnimationSwapper```
    
<details>
<summary>Click here to see the details and code</summary>

This is a temp classs setup to swap out the animations during randomization, needs to be designed.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimationSwapper : MonoBehaviour
{
   public GameObject avatarReference;
   public Animator avatarAnimatorReference;
   public Button buttonNextReference;
   public Button buttonPrevReference;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

```
</details>  
    
 ---   

### AttributeClass.cs
##### Component name: ```AttributeClass```
<details>
<summary>Click here to see the details and code</summary>

This code defines two C# classes: `AttributeClass` and `TraitsToLoad`.

The AttributeClass class has two fields:

`trait_type`, which is a `string`
`value`, which is a `string`
The `TraitsToLoad` class has five fields:

`description`, which is a `string`
`external_url`, which is a `string`
`image`, which is a `string`
`name`, which is a `string`
`attributes`, which is an array of `AttributeClass` objects
Both classes are decorated with the `[System.Serializable]` attribute, which means that they can be serialized and deserialized by the Unity engine. This means that an instance of either class can be converted to a format that can be stored (for example, as a JSON file) and then restored back into a new instance of the class at a later time.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttributeClass
{
    public string trait_type;
    public string value;
}

[System.Serializable]
public class TraitsToLoad
{
    public string description;
    public string external_url;
    public string image;
    public string name;
    public AttributeClass[] attributes;
}
```
</details> 
        
---    
    
### AutoRotate.cs
##### Component name: ```AutoRotate```
<details>
<summary>Click here to see the details and code</summary>

This code defines a class called `AutoRotate` that has the following characteristics:

It is a `MonoBehaviour`, which means it is a component that can be attached to a GameObject in a Unity project.

It has four public fields:
* `ySpeed`, which is a float representing the speed of rotation around the y-axis
* `xSpeed`, which is a float representing the speed of rotation around the x-axis
* `zSpeed`, which is a float representing the speed of rotation around the z-axis
* `thingToRotate`, which is a `GameObject`

It has a single method called `Update`, which is called once per frame.
The purpose of this code is to cause a GameObject to continuously rotate around a specified axis or axes. The `thingToRotate` field specifies which GameObject to rotate, and the `xSpeed`, `ySpeed`, and `zSpeed` fields specify the rotational speed around the respective axes. The `Update` method uses the `Rotate` method of the `Transform` component of the `thingToRotate` GameObject to apply the rotation. The rotation is applied using the delta time, which is the time in seconds that has passed since the last frame was rendered. This helps to ensure that the rotation is consistent regardless of the frame rate.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{

  
  public float ySpeed = 0.3f;
  public float xSpeed = 0f;
  public float zSpeed = 0f;

  public GameObject thingToRotate; 


    void Update()
   {
       thingToRotate.transform.Rotate(xSpeed * Time.deltaTime, ySpeed * Time.deltaTime, zSpeed * Time.deltaTime);
   }

}
```
</details>     
        
---    
    
### BGColorRandomizer.cs
##### Component name: ```BGColorRandomizer```
<details>
<summary>Click here to see the details and code</summary>

`Start`: This method is called when the script is first enabled, before any update frame. It gets a reference to a `Button` component attached to a GameObject and adds a listener to it that calls the `TaskOnClick` method when the button is clicked. It also calls the `GetRandomValue` method and uses the return value to change the material of a different GameObject.
`Update`: This method is called once per frame. It updates the text of a text field with the current value of a `currentEntryValue` field.
`GetRandomValue`: This method takes a list of WeightedValue objects as input and returns a random value from the list. The returned value is chosen based on the weights of the WeightedValue objects, with higher weights being more likely to be chosen. The method also updates the `currentEntryValue` field with the chosen value.
`TaskOnClick`: This method is called when the button added in the Start method is clicked. It calls the GetRandomValue method, changes the material of a GameObject based on the return value, and calls a method called ExportJsonToText.
`RandomCheck`: uses GetRandomValue(weightedValues); and to change the material
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
 
public class BGColorRandomizer : MonoBehaviour
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
        //Debug.Log(randomValue ?? "No entries found");
        

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
        //Debug.Log(randomValue ?? "No entries found");

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
```
</details>     
        
---    
    
### BoomboxColorRandomizer.cs
##### Component name: ```BoomboxColorRandomizer```
<details>
<summary>Click here to see the details and code</summary>

`Start`: This method is called when the script is first enabled, before any update frame. It gets a reference to a `Button` component attached to a GameObject and adds a listener to it that calls the `TaskOnClick` method when the button is clicked. It also calls the `GetRandomValue` method and uses the return value to change the material of a different GameObject.
`Update`: This method is called once per frame. It updates the text of a text field with the current value of a `currentEntryValue` field.
`GetRandomValue`: This method takes a list of WeightedValue objects as input and returns a random value from the list. The returned value is chosen based on the weights of the WeightedValue objects, with higher weights being more likely to be chosen. The method also updates the `currentEntryValue` field with the chosen value.
`TaskOnClick`: This method is called when the button added in the Start method is clicked. It calls the GetRandomValue method, changes the material of a GameObject based on the return value, and calls a method called ExportJsonToText.
`RandomCheck`: uses GetRandomValue(weightedValues); and to change the material
    
```c#
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
```
</details>     
        
---    
    
### CameraCapture.cs
##### Component name: ```CameraCapture```
<details>
<summary>Click here to see the details and code</summary>

This code defines a class called `CameraCapture` that has the following characteristics:

It is a `MonoBehaviour`, which means it is a component that can be attached to a GameObject in a Unity project.

It has three public fields:
* `screenshotKey`, which is a `KeyCode` representing the key that will be used to take a screenshot
* `Camera`, which is a `Camera`
* `dnaManagerReference`, which is an object of a class called `DNAManager`

The `CameraCapture` class has two methods:

`LateUpdate`: This method is called after all Update methods have been called. It listens for the user to press the screenshotKey and calls the `Capture` method if the key is pressed.

`Capture`: This method captures a screenshot from the Camera and saves it to a file in the `StreamingAssets/Images` directory with a filename based on the `genID` field of the `dnaManagerReference` object.

In summary, this code is intended to allow the user to take a screenshot of the camera's view by pressing a specified key, and to save the screenshot to a file in the `StreamingAssets/Images` directory.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
 
public class CameraCapture : MonoBehaviour
{
    //public int fileCounter;
    public KeyCode screenshotKey;
    public Camera Camera;
    public DNAManager dnaManagerReference;
   
 
    private void LateUpdate()
    {
        if (Input.GetKeyDown(screenshotKey))
        {
            Capture();
        }
    }
 
    public void Capture()
    {
        RenderTexture activeRenderTexture = RenderTexture.active;
        RenderTexture.active = Camera.targetTexture;
 
        Camera.Render();
 
        Texture2D image = new Texture2D(Camera.targetTexture.width, Camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, Camera.targetTexture.width, Camera.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = activeRenderTexture;
 
        byte[] bytes = image.EncodeToPNG();
        Destroy(image);
 
        File.WriteAllBytes(Application.dataPath + "/StreamingAssets/Images/" + dnaManagerReference.genID + ".png", bytes);
        //fileCounter++;
    }
}
```
</details>     
        
---    
    
### ConfigureRandomizer.cs
##### Component name: ```ConfigureRandomizer```
<details>
<summary>Click here to see the details and code</summary>

This code defines a class called `ConfigureRandomizer` that has the following characteristics:

It is a `MonoBehaviour`, which means it is a component that can be attached to a GameObject in a Unity project.

It has several public fields:

* `totalNFTs`, which is an integer representing the total number of NFTs (non-fungible tokens) to generate
* `dnaManagerReference`, which is an object of a class called DNAManager
* `randomizeAllScriptReference`, which is an object of a class called `RandomizeAll`
* `buttonReference`, which is a `Button`
* `cameraCaptureReference`, which is an object of the `CameraCapture` class defined in the previous code example
* `toTxtFileRef`, which is an object of a class called `ToTextFile`
* `delaySpeed`, which is a float representing the delay between generating each NFT
* `vrmRuntimeExporterRef`, which is an object of a class called `VRMRuntimeExporter1` that was a copy of the vrm exporter class
* `modelToExportToVRM`, which is a `GameObject`
* `exportVRMFromRandomTrait`, which is an object of a class called `SetObjectsVisibility`
* `vrmTitle`, `vrmAuthor`, `vrmContactInformation`, `vrmReference`, and `vrmVersion`, which are strings representing metadata for VRM (Virtual Reality Model) export

It has three methods:

`Start`: This method is called when the script is first enabled, before any update frame. It gets a reference to the `Button` component attached to a GameObject and adds a listener to it that calls the `TaskOnClick` method when the button is clicked.

`Update`: This method is called once per frame. It does not appear to have any functionality in this code.

`TaskOnClick`: This method is called when the button added in the `Start` method is clicked. It calls a method called `GenerateAllNFTsSlow` with the totalNFTs field as an argument.

`GenerateAllNFTs`: This method takes an integer argument `totalNFTs` and generates that number of NFTs. It does this by calling the `RamdomizeAll` method of the `randomizeAllScriptReference` object, capturing a screenshot with the `Capture` method of the `cameraCaptureReference` object, creating a text file with the `CreateTextFile` method of the `toTxtFileRef` object, and exporting a VRM file with the `Export` method of the `vrmRuntimeExporterRef` object.

`GenerateAllNFTsSlow`: This method is similar to `GenerateAllNFTs`, but it generates the NFTs one at a time with a delay in between each one. It does this by using a `Coroutine` and the `yield return` keyword to pause execution between each iteration of the loop. It also adds metadata to the `modelToExportToVRM` GameObject before exporting it as a VRM file.

In summary, this code is intended to allow the user to generate a specified number of NFTs by clicking a button. Each NFT is generated by randomizing some values, capturing a screenshot, creating a text file, and exporting a VRM file. The `GenerateAllNFTs` method generates all the NFTs at once, while the `GenerateAllNFTsSlow` method generates them one at a time with a delay in between.
    
```c#
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
        if (dnaManagerReference.optionsManager != null)
            dnaManagerReference.optionsManager.AttachDataToDNA(dnaManagerReference);
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

```
</details>     
        
---    
    
### DNA.cs
##### Component name: ```DNA```
<details>
<summary>Click here to see the details and code</summary>

This code defines a class called `DNA` that has the following characteristics:

It is a `MonoBehaviour`, which means it is a component that can be attached to a `GameObject` in a Unity project.

It has one public field: `dna`, which is a string representing a DNA code.

It has one constructor: `DNA`, which takes a string argument `CurrentDNA` and assigns it to the `dna` field.

In summary, this code is intended to allow the user to create an object of the `DNA` class, which stores a `DNA` code as a string.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DNA : MonoBehaviour
{
    public string dna;

    public DNA(string CurrentDNA){
        dna = CurrentDNA;
    }
}
```
</details>     
        
---    
    
### DropdownHandler.cs
##### Component name: ```DropdownHandler```
<details>
<summary>Click here to see the details and code</summary>

This code defines a class called `DropdownHandler` that has the following characteristics:

It is a `MonoBehaviour`, which means it is a component that can be attached to a GameObject in a Unity project.

It has several public fields:

* `debugPanel`, which is an array of `GameObjects`
* `exportButton`, which is an array of `GameObjects`
* `exportDropDownReference`, which is a `GameObject` representing a dropdown menu
* `TextBox`, which is a `TMP_Text` component

It has two methods:

`Start`: This method is called when the script is first enabled, before any update frame. It gets a reference to the `TMP_Dropdown` component attached to the `exportDropDownReference` GameObject, adds items to the dropdown menu, and adds a listener to the dropdown menu that calls the `DropdownItemSelected` method when the selected value changes.

`DropdownItemSelected`: This method takes a `TMP_Dropdown` object as an argument and does the following:
Gets the index of the selected item in the dropdown menu
Sets the text of the `TextBox` to the text of the selected item
Enables or disables the `debugPanel` and `exportButton` GameObjects based on the selected index of the dropdown menu

 In summary, this code is intended to allow the user to select an item from a dropdown menu, and then show or hide certain GameObjects based on the selected item. It does this by adding items to the dropdown menu, and then using a listener to trigger changes to the GameObjects when the selected value of the dropdown menu changes.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour
{
    public GameObject[] debugPanel;
    public GameObject[] exportButton;
    public GameObject exportDropDownReference;
   

    public TMP_Text TextBox;
    // Start is called before the first frame update
    void Start()
    {
        var dropdown = exportDropDownReference.transform.GetComponent<TMP_Dropdown>();

        List<string> items = new List<string>();

        /* items.Add("Item 1");
        items.Add("Item 1"); */

        // fill dropdown with items
        foreach(var item in items){
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = item});
        }

        DropdownItemSelected(dropdown);

        dropdown.onValueChanged.AddListener(delegate {DropdownItemSelected(dropdown); } );
    }

    void DropdownItemSelected(TMP_Dropdown dropdown){
       int index = dropdown.value;
        TextBox.text = dropdown.options[index].text;
        
        
        for (int i = 0; i < debugPanel.Length; i++)
		{
			debugPanel[i].SetActive(false);
		}

        debugPanel[dropdown.value].SetActive(true);

        for (int i = 0; i < exportButton.Length; i++)
		{
			exportButton[i].SetActive(false);
		}

        exportButton[dropdown.value].SetActive(true);
        

    }

    // Update is called once per frame
    void Update()  
    {
       
    }
}

```
</details>    
        
---    
     
### ExportToWebaverse.cs
##### Component name: ```ExportToWebaverse```
<details>
<summary>Click here to see the details and code</summary>

This is test code to parse output json to Webaverse, not working quite right just yet
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class ExportToWebaverse : MonoBehaviour
{
    public Button buttonReference;

    [TextArea(10,10)]
    public string webaJSONOutputPreview;

    public TextMeshProUGUI WebaJSONBody;
    private int exportNumber;

    // reference to the allObjestdata Array
    

    void Start()
    {       
        // create a folder
        Directory.CreateDirectory(Application.streamingAssetsPath + "/WEBA_Output/");

        // Reference to Button
        Button btn = buttonReference.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);    
    }

    
    void Update()
    {      
    }

    void TaskOnClick()
    {
		Debug.Log ("You have exported a webaverse scene file"); 
       
        //GatherAllLightsAndParameters();
        //GatherAllCamerasAndParamaters();
        //GatherAllObjectsComponentsAndParamaters();

        //  go through all objects in scene and define an array of all the objects and components and their respective parameters

        //typecheck all of the object and build a dictionary of all of these objects

        // assign all of the paramaters to the correct export variable containers

        // do all of the error checking and contructing of the objects strings

        ExportWebaverseJsonToText();

        if(webaJSONOutputPreview == "")
        {
            return;
        }
       
        // create and print the text file
        CreateTextFile();
        exportNumber++;         
	}

    

    

public void ExportWebaverseJsonToText()
    {
       
        webaJSONOutputPreview = "{\n" + 
        // open objects array
        "  \"objects\": [\n" +

        GetAllObjects() +
        
        // close object array
        "  ]\n" +
        "}";

        WebaJSONBody.text = webaJSONOutputPreview;
    }

    // object constructor
     // use \t to tab and \n to line break
    public string GetObject(int objectIndex)
    {
        string value;

        // start string constructor using "value" to temp hold the data
        value = 

        // light object example
        "    {\n" +
        "      \"type\": \"application/light\",\n" +
        "      \"content\": {\n" +
        "        \"lightType\": \"ambient\",\n" +
        "        \"args\": [\n" +
        "           [\n" +
        "             225,\n" +
        "             225,\n" +
        "             225\n" +
        "           ],\n" +
        "           0.1\n" +
        "         ]\n" +
        "       }\n" +
        "     },\n" +

        // 2nd light example

        "     {\n" +
        "      \"type\": \"application/light\",\n" +
        "      \"content\": {\n" +
        "        \"lightType\": \"directional\",\n" +
        "        \"args\": [\n" +
        "           [\n" +
        "             255,\n" +
        "             255,\n" +
        "             255\n" +
        "           ],\n" +
        "           0.5\n" +
        "         ],\n" +
        "         \"position\": [\n" +
        "           22,\n" +
        "           177,\n" +
        "           360\n" +
        "         ],\n" +
        "         \"shadow\": [\n" +
        "            150,\n" +
        "            5120,\n" +
        "            0.1,\n" +
        "            10000,\n" +
        "            -0.0001\n" +
        "         ]\n" +
        "       }\n" +
        "    },\n" +

        // wind example

        "     {\n" +
        "      \"type\": \"application/wind\",\n" +
        "      \"content\": {\n" +
        "        \"windType\": \"directional\",\n" +
        "        \"direction\": [\n" +
        "           22,\n" +
        "           177,\n" +
        "           360\n" +
        "         ],\n" +
        "         \"windForce\": 0.5,\n" +
        "         \"noiseScale\": 1,\n" +
        "         \"windFrequency\": 1\n" +
        "       }\n" +
        "    },\n";


        

        // end string constructor and return the value as as a string
        return value;

    }

    public string GetAllObjects()
    {
        string allObjects;
        allObjects = GetObject(0);
          //GetObject(0) + "," 
        //+ GetObject(1) + ","
        //+ GetObject(2) + ","
        //+ GetObject(4);
        // end trait function
        return allObjects;

    }

    public void CreateTextFile(){

        // name it
        string txtDocumentName = Application.streamingAssetsPath + "/WEBA_Output/" + "/WEBA_Output" + exportNumber + ".txt";

        // check to see if exists
        if (!File.Exists(txtDocumentName))
        {
            // Write to File
            File.WriteAllText(txtDocumentName, webaJSONOutputPreview);
        }
    }
}

```
</details>     
        
---    
    
### ImportJSON.cs
##### Component name: ```ImportJSON```
<details>
<summary>Click here to see the details and code</summary>

This script contains a number of methods that perform various tasks related to importing JSON data and using it to modify objects in a scene.

The `LoadJSON` method is responsible for loading a JSON file with a specific name, specified in the `filename` field, from the Resources folder. It then parses the JSON data and stores it in a `TraitsToLoad` object. It also searches for game objects in the scene with names that match values specified in the JSON data and sets them to active.

The `TaskOnImportClick` method is called when a button is clicked, and it calls the `LoadJSON` method.

The `TaskOnBrowseButtonClick` method is called when a button is clicked, and it opens a file explorer window for the user to browse for and select a JSON file. It then calls the GetJSONFile method, which downloads the selected file and loads it into the jsonFile field.

The `OnPathValueChanged` method is called whenever the value in an input field is changed, and it updates the filename field with the new value.

The `OpenFileExplorer` method opens a file explorer window for the user to browse for and select a file. It then stores the selected file's path in the `path` field and updates the input field with the file's name.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using TMPro;
using UnityEngine.Networking;

public class ImportJSON : MonoBehaviour
{
    string[] jsonFileNames = { "JSON_Output1" };
    public static List<TraitsToLoad> TraitsToLoadList = new List<TraitsToLoad>();
    public static TextAsset jsonFile;
    public static TraitsToLoad obj;
    public string filename;
    public Button buttonReference;
    public Button browseButtonReference;
    public int totalNFTs;
    public DNAManager dnaManagerReference;
    public TMP_InputField inputField;
    private string myString; 
    string path;    

    void Start(){
        Button btn = buttonReference.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnImportClick); 
        Button brzbtn = browseButtonReference.GetComponent<Button>();
        brzbtn.onClick.AddListener(TaskOnBrowseButtonClick); 
        filename = inputField.text;
        inputField.onValueChanged.AddListener(OnPathValueChanged);
    }

    void Update()
    {
        
        //LoadMultiFileJSONIntoList();
        //Debug.Log(jsonFile);
    }

    void LoadJSON()
    {   
        jsonFile = Resources.Load(filename) as TextAsset;
        obj = JsonUtility.FromJson<TraitsToLoad>(jsonFile.text);
        GameObject[] objects = (GameObject[])Object.FindObjectsOfTypeAll(typeof(GameObject));
       
        foreach (AttributeClass attr in obj.attributes)
        {
            string traitType = attr.trait_type;
            string value = attr.value;
            //Debug.Log(traitType + ":" + value);

            // create variables for names of objects to be found
            string name = attr.value;
            //Debug.Log("the value is " + value);
            

            // set objects active that match the name
            foreach (GameObject objs in objects)
            {
                if (objs.name == name)
                {
                    objs.SetActive(true);
                    Debug.Log(name + "this object was set to" + objs.activeInHierarchy);
                    break;
                }
            }


        }

              
    }

    /* void LoadMultiFileJSONIntoList()
    {
        List<TraitsToLoad> TraitsToLoadListMultiFile = new List<TraitsToLoad>();
     
        foreach (string jsonFileName in jsonFileNames)
        {
            jsonFile = Resources.Load("JSON_Output1") as TextAsset;
            obj = JsonUtility.FromJson<TraitsToLoad>(jsonFile.text);
            TraitsToLoadListMultiFile.Add(obj);
            
            foreach (AttributeClass attr in obj.attributes)
                {
                string traitType = attr.trait_type;
                string value = attr.value;
              
                Debug.Log(traitType + ":" + value);

                }
            // Do something with the object of traits
        }  
    } */

    void TaskOnImportClick()
    {
		Debug.Log ("You have imported a json text file"); 
        LoadJSON();
	}

    void TaskOnBrowseButtonClick()
    {
        OpenFileExplorer();
		Debug.Log ("You have clicked browse"); 
        GetJSONFile();
	}

    void OnPathValueChanged(string newValue)
    {   
    filename = inputField.text;
    }

    public void OpenFileExplorer()
   {
        path = EditorUtility.OpenFilePanel("Show all json", "/Resources", "txt");
        string fileName = Path.GetFileNameWithoutExtension(path);
        string finalString = fileName;
        inputField.text = finalString;
   }

   IEnumerator GetJSONFile()
    {
       UnityWebRequest www = UnityWebRequest.Get("file:///" + path);
       www.downloadHandler = new DownloadHandlerBuffer();
       yield return www.SendWebRequest();
       

       if (www.isHttpError || www.isNetworkError)
       {
            Debug.Log(www.error); 
       }
       else
       {
            string text = www.downloadHandler.text;
            filename = text;
       }
    }
}

```
</details>     
        
---    
    
### Instructions.cs
##### Component name: ```Instructions```
<details>
<summary>Click here to see the details and code</summary>

Temp class made to deal with help / instructions, needs to be completed
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

```
</details>     
        
---    
    
### ItemManager.cs
##### Component name: ```ItemManager```
<details>
<summary>Click here to see the details and code</summary>

This `ItemManager` script is used to cycle through a list of `GameObject`s using the Page Up and Page Down keys. The script starts by storing the total number of items in the `itemsOnDisplay` array and setting the `currentItemIndex` to 0. In the `Update` function, the script checks if either the Page Up or Page Down keys have been pressed. If the Page Up key is pressed, the script increases the `currentItemIndex` by 1 and then sets all of the items in the `itemsOnDisplay` array to `inactive`. It then sets the item at the `currentItemIndex` in the array to `active`. If the `currentItemIndex` is greater than the total number of items, it is reset to the total number of items. If the Page Down key is pressed, the script decreases the `currentItemIndex` by 1 and then follows the same process as when the Page Up key is pressed. If the `currentItemIndex` is less than 0, it is reset to 0.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
   public GameObject[] itemsOnDisplay;
    private int totalItems;
    private int currentItemIndex = 0;

    void Start()
    {
        // check the total length of the array
        totalItems = itemsOnDisplay.Length;
    }

    void Update()
    {

    if (Input.GetKeyDown(KeyCode.PageUp))
            
            {
            // You Pushed Page Up
            //check to see if the items index in the array is still within range
            if(currentItemIndex < totalItems-1){
                currentItemIndex += 1;
            }
            

            for (int i = 0; i < totalItems; i++)
						{
						   itemsOnDisplay[i].SetActive(false);   
						}
						
			itemsOnDisplay[currentItemIndex].SetActive(true);
                        
                        
                        if(currentItemIndex > itemsOnDisplay.Length){
                            currentItemIndex = totalItems;
                        }

                        
                }

    if (Input.GetKeyDown(KeyCode.PageDown))
			
            {
			// You Pushed Page Down
            //check to see if the items index in the array is still within range
            if(currentItemIndex > 0){
                currentItemIndex -= 1;
            }
           	  

            for (int i = 0; i < totalItems; i++)
						{
						   itemsOnDisplay[i].SetActive(false);   
						}
						
						
            itemsOnDisplay[currentItemIndex].SetActive(true);            
                        
                        if(currentItemIndex < 0){
                            currentItemIndex = 0;
                        }


			}


    }
}
```
</details>     
        
---    
    
### JSONPreview.cs
##### Component name: ```JSONPreview```
<details>
<summary>Click here to see the details and code</summary>

Class to store references  to the JSON Preview Object GUI for debugging in the editor
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class JSONPreview : MonoBehaviour
{
    
    public PrintRandomValue[] JSONData;

    public string[] JSONStringLayerTitles;

    public TextMeshProUGUI traitLayer1Text;
    public TextMeshProUGUI traitLayer2Text; 
    public TextMeshProUGUI traitLayer3Text;
    public TextMeshProUGUI traitLayer4Text;
    public TextMeshProUGUI traitLayer5Text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

```
</details>     
        
---    
    
### PrintRandomValue.cs
##### Component name: ```PrintRandomValue```
<details>
<summary>Click here to see the details and code</summary>

The `PrintRandomValue` class is responsible for randomly selecting a value from a list of `WeightedValue` objects and displaying it. It has a `Button` component attached to it, which when clicked, will call the `TaskOnClick()` method. The `TaskOnClick()` method calls the `GetRandomValue()` method, which returns a random value from the `weightedValues` list. The returned value is then used to activate or deactivate elements in the `totalObjectsInLayer` array, depending on whether the value matches the corresponding element in the `layerStringData` array. The `PrintDNA()` method simply returns the current value of `currentEntryValue`.
    
```c#
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
        //Debug.Log(randomValue ?? "No entries found");
        

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
```
</details>     
        
---    
    
### ProgressBar.cs
##### Component name: ```ProgressBar```
<details>
<summary>Click here to see the details and code</summary>

This script is a simple progress bar that displays the current value of `dnaManagerReference.genID` on a slider, and it has a maximum value of `randomRollRef.totalSpins`. It also has a TextMeshProUGUI object, mText, and a GameObject, `mTextGameObjectRef`, that is associated with it. The `mTextGameObjectRef` object is set to inactive at the start of the script, and when the value of the progress slider reaches the maximum value, `mTextGameObjectRef` is set to active. 
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBar : MonoBehaviour
{
        public DNAManager dnaManagerReference;
        public Slider progressSlider;
        public RandomRoll randomRollRef;
        public TextMeshProUGUI mText;
        public GameObject mTextGaameObjectRef;

    // Start is called before the first frame update
    void Start()
    {
        mTextGaameObjectRef.SetActive(false);
        progressSlider.maxValue = randomRollRef.totalSpins;
    }

    // Update is called once per frame
    void Update()
    {
        progressSlider.value = dnaManagerReference.genID;

        if(progressSlider.value == progressSlider.maxValue)
        {
            mTextGaameObjectRef.SetActive(true);
        }
    }
}

```
</details>     
        
---    
    
### RandomAudioClipPlayer.cs
##### Component name: ```RandomAudioClipPlayer```
<details>
<summary>Click here to see the details and code</summary>

Plays random audio clips
    
```c#
using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class RandomAudioClipPlayer : MonoBehaviour {

	public AudioClip[] clips;

	void Start () {
		GetComponent<AudioSource>().clip = clips[Random.Range (0, clips.Length)];
		GetComponent<AudioSource>().Play();
	}

	void Update () {

	}
}
```
</details>     
        
---    
    
### RandomizeAll.cs
##### Component name: ```RandomizeAll```
<details>
<summary>Click here to see the details and code</summary>

This script is used to randomize the traits of an avatar in a game or application. It has a list of "weighted values" which are used to determine the random values that will be assigned to various traits of the avatar. When the script is started, it listens for a button press, and when the button is pressed, it calls the `RamdomizeAll` function. This function randomly assigns values to the avatar's traits using the `RandomCheck` function of various other script components that are attached to the avatar. It also exports the avatar's traits to a JSON file using the `ExportJsonToText` function of the `DNAManager` component. Finally, it sets the randomly generated traits of the avatar using the `AttachBlendshapesToAvatar` function.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VRM;

public class RandomizeAll : MonoBehaviour
{
    public GameObject parentRandomTraitCaller;

    public List<WeightedValue> weightedValues;
    public PrintRandomValue[] randomScriptReferences;
    public BGColorRandomizer randomBGScriptReference;
    public BGColorRandomizer randomBodyTextureScriptReferences;
    public BGColorRandomizer randomBoomboxTextureScriptReferences;
    public Button buttonReference;
    public DNAManager dnaManagerReference;

    public SetObjectsVisibility exportVRMFromRandomTrait;

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
        if (dnaManagerReference.optionsManager != null)
            dnaManagerReference.optionsManager.AttachDataToDNA(dnaManagerReference);

        if (!dnaManagerReference.DNAList.Contains(dnaManagerReference.DNACode)) {
            RamdomizeAll();
        } else {
            RamdomizeAll();
        }


    }

    public void RamdomizeAll() {
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
        randomBoomboxTextureScriptReferences.RandomCheck();
        dnaManagerReference.ExportJsonToText(newTraits);
    }

    private string RandomizeParentGetJsonData()
    {
        ActionCaller[] traits = parentRandomTraitCaller.GetComponentsInChildren<ActionCaller>(true);
        SuperRules[] rules = parentRandomTraitCaller.GetComponentsInChildren<SuperRules>(true);
        string result = "";
        List<Object> extraData = new List<Object>();
        foreach (ActionCaller t in traits)
        {
            t.SetPreSetup();
        }
        foreach (ActionCaller t in traits)
        {
            t.SetRandomTrait(); 
        }
        foreach (SuperRules r in rules)
        {
            r.ApplyRule();
        }
        //call rules here
        foreach (ActionCaller t in traits)
        {
            t.SetAction();
            result += t.GetJsonedObject(true, 1);
            extraData.AddRange(t.GetExtraData());
        }
        foreach (ActionCaller t in traits)
        {
            t.SetPostSetup();
        }
            

        // fetch all blendshape clips data
        List<BlendShapeClip> shapeClips = new List<BlendShapeClip>();
        for (int i =0; i < extraData.Count; i++)
        {
            if (ObjectType(extraData[i], typeof(BlendShapeClip)))
            {
                shapeClips.Add(extraData[i] as BlendShapeClip);
            }
        }
        AttachBlendshapesToAvatar(shapeClips);

        //return json data
        return result.Substring(0, result.Length - 2) + "\n";
    }

    private bool ObjectType(Object obj, System.Type type)
    {
        if (obj.GetType() == type)
            return true;
        return false;
    }

    private void AttachBlendshapesToAvatar(List<BlendShapeClip>clips)
    {
        if (clips.Count > 0)
        {
            if (exportVRMFromRandomTrait != null)
            {
                BlendShapeAvatar avatar = new BlendShapeAvatar
                {
                    Clips = clips
                };
                GameObject root = exportVRMFromRandomTrait.selectedObject as GameObject;
                VRMBlendShapeProxy proxy = root.GetComponent<VRMBlendShapeProxy>();
                if (proxy == null)
                    proxy = root.AddComponent<VRMBlendShapeProxy>();
                proxy.BlendShapeAvatar = avatar;
            }
            else
            {
                Debug.LogError("Blend shapes found, but exportVRMFromRandomTrait as not set");
            }
        }
    }
}

```
</details>     
        
---    
    
### RandomRoll.cs
##### Component name: ```RandomRoll```
<details>
<summary>Click here to see the details and code</summary>

This script is a script for the Unity game engine that is attached to a game object called `RandomRoll`. It defines a class called `RandomRoll` that is derived from the `MonoBehaviour` class, which is a base class for components that can be attached to game objects in Unity.

The script has several public variables:

`ramdomizeAllRef` is a reference to a script called `RandomizeAll` that is attached to a different game object.
`totalSpins` is an integer that specifies how many times the object should `spin` (the specific meaning of this is not clear from the script).
`delaySpeed` is a float that specifies how long the object should wait between each spin.
`buttonReference` is a reference to a UI button object in the game.
`dnaManagerReference` is a reference to a script called `DNAManager` that is attached to a different game object.
`audioSource` is a reference to an audio source component that is attached to the same game object as this script.
`startingPitch` is an integer that specifies the starting pitch of the audio source.
`timeToDecrease` is an integer that specifies how long it takes for the pitch of the audio source to decrease.
`progressSound` is an audio clip that is played when the object is spinning.
The script also has several private variables:

`rollCoroutine` is a reference to a coroutine (a type of function that can be paused and resumed).
`isRolling` is a boolean that specifies whether the object is currently spinning.
The script has several functions:

`Start()` is a function that is called when the object is first created. It gets a reference to the button component and adds a listener for the `onClick` event, which will call the `TaskOnClick()` function when the button is clicked. It also sets the pitch of the audio source to the `startingPitch` value.
`Update()` is a function that is called once per frame and can be used to update the objects state. This function is currently empty.
`TaskOnClick()` is a function that is called when the button is clicked. It starts a coroutine called `Roll()` with the total number of spins specified by the `totalSpins` variable.
`Roll()` is a coroutine function that performs the spinning of the object. It first sets the `genID` variable in the `DNAManager` script to 0 and the `delaySpeed` variable to 0.01. It then enters a loop that runs for the number of spins specified by the `totalSpins` variable. In each iteration of the loop, the function increments the `delaySpeed` variable, plays a sound, and then waits for the amount of time specified by the `delaySpeed` variable before continuing to the next iteration. When the loop finishes, the function sets the `isRolling` variable to false.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class RandomRoll : MonoBehaviour
{
    public RandomizeAll ramdomizeAllRef;
    public int totalSpins;
    //private int lengthOfSpin = 3;
    public float delaySpeed = 0.01f;
    public Button buttonReference;
    private Coroutine rollCoroutine;
     public DNAManager dnaManagerReference;
     public AudioSource audioSource;
     public int startingPitch = 1;
     public int timeToDecrease = 5;
     public AudioClip progressSound;
     private bool isRolling = false;
    


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
        

        // check to see if it is already rolling
        if (isRolling == false){
                    // Generate NFts SLow
                    rollCoroutine = StartCoroutine(Roll(totalSpins));
                    isRolling = true;
        }
        
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

        isRolling = false;
            
    }
}

```
</details>     
        
---    
    
### RandomTests.cs
##### Component name: ```RandomTests```
<details>
<summary>Click here to see the details and code</summary>


Test script
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTests : MonoBehaviour
{
    int randomNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        randomNumber = UnityEngine.Random.Range(1, 100);  //Random number between 1 and 99
        Debug.Log("Hello: " + randomNumber);
    }
}

```
</details>     
        
---    
    
### SelectionDropDownHandler.cs
##### Component name: ```SelectionDropDownHandler```
<details>
<summary>Click here to see the details and code</summary>

This script is a script for the Unity game engine that is attached to a game object called `SelectionDropDownHandler`. It defines a class called `SelectionDropDownHandler` that is derived from the `MonoBehaviour` class, which is a base class for components that can be attached to game objects in Unity.

The script has several public variables:

`DropDownReference` is a reference to a game object with a `TMP_Dropdown` component attached to it, which is a dropdown UI element in the Unity engine.
`UiSelectionManagerRef` is a reference to a script called `UiSelectionManager` that is attached to a different game object.
The script has a `Start()` function that is called when the object is first created. It gets a reference to the `TMP_Dropdown` component on the `DropDownReference` game object and adds a list of strings as options to the dropdown. It then adds a listener for the `onValueChanged` event on the dropdown, which will call the `DropdownItemSelected()` function when the selected option in the dropdown is changed.

The `DropdownItemSelected()` function is called when the selected option in the dropdown is changed. It gets the index of the selected option and passes it to the `SelectSlot()` function of the `UiSelectionManager` script.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectionDropDownHandler : MonoBehaviour
{

    public GameObject DropDownReference;
    //public Image[] thisImgThumbnail;
    public UiSelectionManager UiSelectionManagerRef;


    


    void Start()
    {
        var dropdown = DropDownReference.transform.GetComponent<TMP_Dropdown>();

        List<string> items = new List<string>();

        
      /*   items.Add("Hat");
        items.Add("Pet");
        items.Add("Glasses");
        items.Add("Weapon");  */

        // fill dropdown with items
        foreach(var item in items){
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = item});
        }

        //DropdownItemSelected(dropdown);

        dropdown.onValueChanged.AddListener(delegate {DropdownItemSelected(dropdown); } );
    }

    void DropdownItemSelected(TMP_Dropdown dropdown)
    {
       int index = dropdown.value;
        UiSelectionManagerRef.SelectSlot(index);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}

```
</details>     
        
---    
    
### ShowHideUI.cs
##### Component name: ```ShowHideUI```
<details>
<summary>Click here to see the details and code</summary>

This script is a script for the Unity game engine that is attached to a game object called `ShowHideUI`. It defines a class called `ShowHideUI` that is derived from the `MonoBehaviour` class, which is a base class for components that can be attached to game objects in Unity.

The script has a public variable called `UIReference` which is a reference to a game object that represents some part of the user interface (UI) in the game.

The script also has a private variable called `isUIOn` which is a boolean that specifies whether the UI is currently being displayed or not.

The script has a `Start()` function that is called when the object is first created, but this function is currently empty.

The `Update()` function is called once per frame and checks if the `Tilde` key has been pressed. If it has, the function toggles the value of the `isUIOn` variable and sets the active state of the `UIReference` game object accordingly. If `isUIOn` is true, the UI is made active (displayed); if it is false, the UI is made inactive (hidden).
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideUI : MonoBehaviour
{
    public GameObject UIReference;
	private bool isUIOn = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // toggle UI
			if (Input.GetKeyDown(KeyCode.Tilde))
			{
				
				isUIOn = !isUIOn;
				
				if (isUIOn)
				{ 
             UIReference.SetActive(true);
			
				}else{
             UIReference.SetActive(false);
			
				}
			}
    }

   
}

```
</details>     
        
---    
    
### SuperRule.cs
##### Component name: ```SuperRule```
<details>
<summary>Click here to see the details and code</summary>
Summary
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SuperRule : MonoBehaviour
{
  
   public GameObject[] layerGroupReferences;
    public DNAManager dnaManagerRef;
    public GameObject ifThisObjectAppears;
    public GameObject[] excludeTheseObjects;
    private string whatObjectIsCurrentlyEnabled;
  
    public GameObject[] ifTheseObjectAppear;
    public GameObject includeThisObject;
  
    public GameObject[] ifTheseObjectAlllAppear;
    public GameObject excludeAllTheseObjects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     // check superrule 1 is true of false
    public bool CheckSuperRule1()
    {
        // init local variable to a default value
        bool superRule1 = false;
        // end super rule 1 check
        superRule1 = ifThisObjectAppears.activeSelf;
        return superRule1;
    }


    // chgeck superrule 2 is true of false
    public bool CheckSuperRule2()
    {
        // init local variable to a default value
        bool allAreTrue = false;
       
        // create an array of game objects to test if they are active
        GameObject[] superRule2ArrayObjects = new GameObject[ifTheseObjectAppear.Length];

        // create an array of booleans to store each objects active state
        bool[] superRule2BoolArray = new bool[ifTheseObjectAppear.Length];

        // loop through the objects and assgn their state to the corresponding bool index
        for(int i = 0; i < ifTheseObjectAppear.Length ; i++)
        {
            superRule2BoolArray[i] = ifTheseObjectAppear[i].activeSelf;
            
            // loop thru all bools and check to see if all objects are active
                if (!superRule2BoolArray[i])
                {
                    // if one is false then set superRule2 to false
                    return allAreTrue = false;
                }else{

                    // or else the superrule returns true
                    allAreTrue = true;
                }    
        }

        // return superrule
        return allAreTrue;
    }

    // A check to see what layers are active, it requires the users to pass in the index of the array of objects references, this is used in the superrule logic
    public bool IsLayerActive(int layer)
    {
        // init local variable to a default value
        bool isLayerActive = false;
        // end super rule 1 check
        isLayerActive = layerGroupReferences[layer].activeSelf;
        return isLayerActive; 
    }

}

```
</details>     
        
---    
    
### ToggleControls.cs
##### Component name: ```ToggleControls```
<details>
<summary>Click here to see the details and code</summary>
Summary
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleControls : MonoBehaviour
{
    // declare variables
                  
            
                    
            public GameObject studioLightsReference;
            private bool showStudioLights = false;

			public GameObject sunLightReference;
			private bool sunLightRefernceOn = false;

			public GameObject postPossingReference;
			private bool PostProcessingVolumeOn = false;

			public GameObject UIReference;
			private bool isUIOn = true;

			public RandomizeAll randomizeAllref;
            


    void Start()
    {
        
    }

    void Update()
    {
       
     

			// toggle ui
            if (Input.GetKeyDown(KeyCode.Tab))
			{
				isUIOn = !isUIOn;
				
				if (isUIOn) 
             UIReference.SetActive(true);
				else
             UIReference.SetActive(false);
				
			}

			if (Input.GetKeyDown(KeyCode.Space))
			{
				randomizeAllref.RamdomizeAll();
				
			}

            // sunlight Reference Right Bracket
            if (Input.GetKeyDown(KeyCode.RightBracket))
			{
				sunLightRefernceOn = !sunLightRefernceOn;
				
				if (sunLightRefernceOn) 
             sunLightReference.SetActive(true);
				else
             sunLightReference.SetActive(false);
				
			}


            // studio lights toggle "T"
			if (Input.GetKeyDown(KeyCode.T))
			{
				
				showStudioLights = !showStudioLights;
				
				if (showStudioLights) 
             studioLightsReference.SetActive(true);
				else
             studioLightsReference.SetActive(false);
			}

		
			// PostProcessing ShowHide
			if (Input.GetKeyDown(KeyCode.KeypadDivide))
			{
				
				PostProcessingVolumeOn = !PostProcessingVolumeOn;
				
				if (PostProcessingVolumeOn)
				{ 
             postPossingReference.SetActive(true);
				}else{
             postPossingReference.SetActive(false);
				}
			}

			// randomize all



    }
}

```
</details>     
        
---    
    
### ToTextFile.cs
##### Component name: ```ToTextFile```
<details>
<summary>Click here to see the details and code</summary>
Summary
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ToTextFile : MonoBehaviour
{

    public DNAManager dnaIDReference;
    public Button buttonReference;
    //private int batchNumber;
    

    void Start(){
            //batchNumber = 1;
            // create a folder
            Directory.CreateDirectory(Application.streamingAssetsPath + "/JSON_Output/");
        
            // Reference to Button
            Button btn = buttonReference.GetComponent<Button>();
		    btn.onClick.AddListener(TaskOnClick);
    }

    public void CreateTextFile(){

        // name it
        string txtDocumentName = Application.streamingAssetsPath + "/JSON_Output/JSON_Output" + dnaIDReference.genID + ".json";

        // check to see if exists
        if (!File.Exists(txtDocumentName))
        {
            // Write to File
            File.WriteAllText(txtDocumentName, dnaIDReference.jsonOutputPreview);
        }
    }

    void TaskOnClick(){
		Debug.Log ("You have created a json file"); 
       
        if(dnaIDReference.jsonOutputPreview == ""){
            return;
        }
       
        // create and print the text file
        CreateTextFile();
                  
	}
}

```
</details>    
        
---    
     
### UILayerDropdownHandler.cs
##### Component name: ```UILayerDropdownHandler```
<details>
<summary>Click here to see the details and code</summary>

This script is a script for the Unity game engine that is attached to a game object called `UILayerDropdownHandler`. It defines a class called `UILayerDropdownHandler` that is derived from the `MonoBehaviour` class, which is a base class for components that can be attached to game objects in Unity.

The script has several public variables:

`layerButton` is an array of game objects that represent UI buttons.
`exportDropDownReference` is a reference to a game object with a `TMP_Dropdown` component attached to it, which is a dropdown UI element in the Unity engine.
`TextBox` is a reference to a `TMP_Text` component, which is a text UI element in the Unity engine.
The script has a `Start()` function that is called when the object is first created. It gets a reference to the `TMP_Dropdown` component on the `exportDropDownReference` game object and adds a list of strings as options to the dropdown. It then calls the `DropdownItemSelected()` function with the dropdown as an argument. Finally, it adds a listener for the `onValueChanged` event on the dropdown, which will call the `DropdownItemSelected()` function when the selected option in the dropdown is changed.

The `DropdownItemSelected()` function is called when the selected option in the dropdown is changed. It gets the index of the selected option, sets the text of the `TextBox` to the selected option, and activates the corresponding UI button in the `layerButton` array while deactivating all other buttons.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UILayerDropdownHandler : MonoBehaviour
{

    public GameObject[] layerButton;
    public GameObject exportDropDownReference;
   

    public TMP_Text TextBox;
    // Start is called before the first frame update
    void Start()
    {
        var dropdown = exportDropDownReference.transform.GetComponent<TMP_Dropdown>();

        List<string> items = new List<string>();

        /* items.Add("Item 1");
        items.Add("Item 1"); */

        // fill dropdown with items
        foreach(var item in items){
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = item});
        }

        DropdownItemSelected(dropdown);

        dropdown.onValueChanged.AddListener(delegate {DropdownItemSelected(dropdown); } );
    }

    void DropdownItemSelected(TMP_Dropdown dropdown){
       int index = dropdown.value;
        TextBox.text = dropdown.options[index].text;
        

        for (int i = 0; i < layerButton.Length; i++)
		{
			layerButton[i].SetActive(false);
		}

        layerButton[dropdown.value].SetActive(true);
        

    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
```
</details>     
        
---    
    
### UIRandomDropdownHandler.cs
##### Component name: ```UIRandomDropdownHandler```
<details>
<summary>Click here to see the details and code</summary>

A simple dropdown menu that displays a different element of an array based on the dropdown value selected. The dropdown is populated with a list of `strings`. When an item is selected from the dropdown, the element of the `layerButton` array corresponding to the index of the selected item is displayed, and all other elements of the array are hidden. The `TextBox` object's `text` property is also set to the text of the selected dropdown item. 
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UILayerDropdownHandler : MonoBehaviour
{

    public GameObject[] layerButton;
    public GameObject exportDropDownReference;
   

    public TMP_Text TextBox;
    // Start is called before the first frame update
    void Start()
    {
        var dropdown = exportDropDownReference.transform.GetComponent<TMP_Dropdown>();

        List<string> items = new List<string>();

        /* items.Add("Item 1");
        items.Add("Item 1"); */

        // fill dropdown with items
        foreach(var item in items){
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = item});
        }

        DropdownItemSelected(dropdown);

        dropdown.onValueChanged.AddListener(delegate {DropdownItemSelected(dropdown); } );
    }

    void DropdownItemSelected(TMP_Dropdown dropdown){
       int index = dropdown.value;
        TextBox.text = dropdown.options[index].text;
        

        for (int i = 0; i < layerButton.Length; i++)
		{
			layerButton[i].SetActive(false);
		}

        layerButton[dropdown.value].SetActive(true);
        

    }

    // Update is called once per frame
    void Update()
    {
       
    }
}

```
</details>     
        
---    
    
### UiSelectionManager.cs
##### Component name: ```UiSelectionManager```
<details>
<summary>Click here to see the details and code</summary>

This script appears to be a script for managing the selection of slots in a UI. When an element in the `clickableSlots` array is clicked, the corresponding element in the `thisImg` array will change color and the `slotPreviewImg` will change its sprite to the corresponding element of the `thisImgThumbnail` array. The `SelectSlot()` method is called when a slot is clicked, and it takes an integer `slotNumber` as an argument. It sets the color of all elements of `thisImg` to the default color `m_MyColor`, and sets the color of the element of `thisImg` corresponding to `slotNumber` to the alternate color `m_MyColor_whenClicked`. It also sets the sprite of `slotPreviewImg` to the corresponding element of `thisImgThumbnail`.

The `PrevSlot()` and `NextSlot()` methods are called when the `prevButtonReference` and `nextButtonReference` buttons are clicked, respectively. They take an integer n as an argument, which is always 0 in the current implementation of these methods. The `PrevSlot()` method decreases the value of `currentSlotNumber` by `n` if `currentSlotNumber` is greater than 0, and the `NextSlot()` method increases the value of `currentSlotNumber` by `n` if `currentSlotNumber` is less than the length of `isSelected` minus 1. In both cases, the `SelectSlot()` method is then called with the updated value of `currentSlotNumber` as the argument.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiSelectionManager : MonoBehaviour
{

    public GameObject[] clickableSlots;
    public Image[] thisImg;
    public Image[] thisImgThumbnail;
    public Image slotPreviewImg;
    private Color m_MyColor;
    private Color m_MyColor_whenClicked;
    public bool[] isSelected;
    public int currentSlotNumber;
    public Button prevButtonReference;
    public Button nextButtonReference;
   
    // Start is called before the first frame update
    void Start()
    {
        //Button btnPrev = prevButtonReference.GetComponent<Button>();
        //Button btnNext = nextButtonReference.GetComponent<Button>();
        //btnNext.onClick.AddListener(NextSlot(0));   
        //btnPrev.onClick.AddListener(PrevSlot(0)); 

        // set default color values
        m_MyColor = Color.red;
        m_MyColor_whenClicked = Color.blue;
        
        // loop through and get references to all the iamges on the clickable slots
        for (int i = 0; i < clickableSlots.Length; i++)
		{
			thisImg[i] = clickableSlots[i].GetComponent<Image>();
		}

        // initialize
        SelectSlot(0);
 
    }

    // the Select slot method takes in the slot number assigned in the UI event when clicked
    public void SelectSlot(int slotNumber)
    {
        if(slotNumber >= 0 && slotNumber <= clickableSlots.Length)
        {
                
                
                if(!isSelected[slotNumber])
                {
                    for (int i = 0; i < clickableSlots.Length; i++)
                        {
                            thisImg[i].color = m_MyColor; 
                            isSelected[i] = false;
                        }

                        thisImg[slotNumber].color = m_MyColor_whenClicked;
                        isSelected[slotNumber] = true;
                }

                
                slotPreviewImg.sprite = thisImgThumbnail[slotNumber].sprite;
        }

        currentSlotNumber = slotNumber;

    }

    public void PrevSlot(int n)
    {  
            if(currentSlotNumber > 0)
            {
                SelectSlot(currentSlotNumber + n);    
            }
            
    }

    public void NextSlot(int n)
    {
            if(currentSlotNumber < isSelected.Length -1)
            {
                SelectSlot(currentSlotNumber + n);  
            } 
    }
}

```
</details>     
        
---    
    
### WearableOffsetManager.cs
##### Component name: ```WearableOffsetManager```
<details>
<summary>Click here to see the details and code</summary>

A script for managing the offsets of a `GameObject` in a Unity project. The script has several public variables that are references to UI elements in the Unity project, including three sliders (`PSR`), three dropdown menus (`PSRMode`), and several buttons and game objects.

The script has several functions, including the `Start()` function, which is called when the script is first run. In the `Start()` function, an event listener is added to the `buttonReference` button, which calls the `TaskOnClick()` function when clicked. This function calls the `ResetPSR()` function, which resets the position of the `objectToOffset` GameObject to the default values in the `PSRDefaults` array.

The `Update()` function is called every frame, and it sets the positions, scales, and rotations of the `allGameObjectsToControl` array to the corresponding elements of the `allPositionContainerReferences` array. This updates the positions of the `allGameObjectsToControl` objects based on the positions of the allPositionContainerReferences objects.

The `ValueChangeCheck()` function is called when the value of any of the `PSR` sliders is changed. It sets the local position of the `objectToOffset` object to the values of the `PSR` sliders, which allows the user to adjust the position of the `objectToOffset` object by manipulating the `PSR` sliders.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
#endif

[ExecuteInEditMode]
public class WearableOffsetManager : MonoBehaviour
{
    
    public Slider[] PSR;
    public TMP_Dropdown[] PSRMode;
    public GameObject objectToOffset;
    private Vector3[] PSRDefaults = new Vector3[3];
    //public Vector3[] PSRVectors;
    public Button buttonReference;

    public GameObject[] allGameOjbectsToControl;
    public GameObject[] allPositionContainerReferences;
  

    
    
    // Start is called before the first frame update
    void Start()
    {

        // Declare ResetPSR Button
        Button btn = buttonReference.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);

           
        // set Default values for the PSR
        /* for (int i = 0; i < PSR.Length; ++i) 
        {
            PSRDefaults[i].position = new Vector3(0,0,0);
        } */

        
            // error check to make sure there is items in the array
            if (PSR.Length != 0)
            {

            // declare and event listener for each slider in the array
             for (int i = 0; i < PSR.Length; i++) 
                {
                PSR[i].onValueChanged.AddListener (delegate {ValueChangeCheck ();});      
                }
            }

            
    }

    // Update is called once per frame
    void Update()
    {
            
            //objectToOffset.transform.localRotation = allPositionContainerReferences[0].transform.localRotation;
            // This code goes through all of the game object references in the array assignes the position of the Position Controllers to the positions of each asset container object
             for (int i = 0; i < allGameOjbectsToControl.Length; i++) 
                {
                   
                        allGameOjbectsToControl[i].transform.position = allPositionContainerReferences[i].transform.position;
                        allGameOjbectsToControl[i].transform.localScale = allPositionContainerReferences[i].transform.localScale;
                        allGameOjbectsToControl[i].transform.rotation = allPositionContainerReferences[i].transform.rotation;
                }        

    }

    public void ResetPSR()
    {

        for (int i = 0; i < PSR.Length; i++)
        {
            objectToOffset.transform.localPosition = PSRDefaults[i];       
        }
        
    }

    public void ValueChangeCheck()
    {   
                        //Debug.Log(PSR[0].value);    
                        objectToOffset.transform.localPosition = new Vector3(PSR[0].value,PSR[1].value,PSR[2].value);
	}

   public void TaskOnClick()
    {
		Debug.Log ("You have reset the PSR"); 
        ResetPSR();
       
                  
	}

   
}

```
</details>     
        
---    
    
### WeightedValue.cs
##### Component name: ```WeightedValue```
<details>
<summary>Click here to see the details and code</summary>

Defines a `WeightedValue` class that has two fields: `value` and `weight`. The `value` field is a `string`, and the `weight` field is an `integer`. The `WeightedValue` class has the `[Serializable]` attribute applied to it, which means that it can be serialized into a format such as JSON, and can be restored from that format at a later time. We use this script to store all of the traits and their weights.
    
```c#
using System;
 
[Serializable]
public class WeightedValue
{
    public string value;
    public int weight;
}

```
</details>     
        
---    
    
### Wiggle.cs
##### Component name: ```Wiggle```
<details>
<summary>Click here to see the details and code</summary>

Wiggles an object around randomly
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour
{
      public AnimationCurve curve;
     public Vector3 distance;
     public float speed;
     public GameObject whatToWiggle;
 
     private Vector3 startPos, toPos;
     private float timeStart;
 
     void randomToPos() {
         toPos = startPos;
         toPos.x += Random.Range(-1.0f, +1.0f) * distance.x;
         toPos.y += Random.Range(-1.0f, +1.0f) * distance.y;
         toPos.z += Random.Range(-1.0f, +1.0f) * distance.z;
         timeStart = Time.time;
     }
 
     // Use this for initialization
     void Start () {
         startPos = transform.position;
         randomToPos();
     }
     
     // Update is called once per frame
     void Update () {
         float d = (Time.time - timeStart) / speed, m = curve.Evaluate(d);
         if (d > 1) {
             randomToPos();
         } else if (d < 0.5) {
             whatToWiggle.transform.position = Vector3.Lerp(startPos, toPos, m * 2.0f);
         } else {
             whatToWiggle.transform.position = Vector3.Lerp(toPos, startPos, (m - 0.5f) * 2.0f);
         }
     }
 }
```
</details>     
        
---    
    
### ExportToGLTF.cs
##### Component name: ```ExportToGLTF```
<details>
<summary>Click here to see the details and code</summary>

Exports a `GameObject` to the `GLTF` file format in a Unity project. The script has several public variables, including a reference to a button (`buttonReference`), a reference to a GameObject (`gameObjectToExport`), and a reference to a `gltfExporter` object (`gltfExporterReference`). The script also has a `path` string field that is serialized, used to store the path to a file amd directory it will export to.

In the `Start()` function, the script creates a directory at the specified path if it does not already exist, and adds an event listener to the `buttonReference` button that calls the `TaskOnClick()` function when the button is clicked. The `TaskOnClick()` function does not currently have any functionality, but it could be used to perform some action when the button is clicked.
    
```c#
using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UniGLTF;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VRMShaders;
//using Plattar;

public class ExportToGLTF : MonoBehaviour
{
    //public DNAManager dnaIDReference;
    public Button buttonReference;
    public GameObject gameObjectToExport;
    public gltfExporter gltfExporterReference;
   
    [SerializeField]
    string path;

    
    
    // Start is called before the first frame update
    void Start()
    {
        // create a folder
           Directory.CreateDirectory(Application.streamingAssetsPath + "/GLTF_Output/");

            // Reference to Button
            Button btn = buttonReference.GetComponent<Button>();
		    btn.onClick.AddListener(TaskOnClick);

    }

    // Update is called once per frame
    void Update()
    {
   
    }



    void TaskOnClick(){
		//Debug.Log ("You have exported a GLTF"); 
        // export glTF
        //gltfExporterReference.Export(textureSerializer);
                  
	}

    

}

```
</details>     
        
---    
    
### RenameChilds.cs
##### Component name: ```RenameChilds```
<details>
<summary>Click here to see the details and code</summary>

This script appears to be a script for renaming the child GameObjects of a parent GameObject in a Unity project. The script has a single public boolean field `removeLastChars` and a single public integer field `quantity`. The script also has the `[ExecuteAlways]` attribute applied to it, which means that it will be executed even in Edit mode.

When the script is enabled, it gets an array of all the child `Transform` components of the parent `GameObject`, and iterates through each `Transform` in the array. If the `removeLastChars` field is `true`, and the name of the GameObject associated with the `Transform` contains a period, the script will remove the specified number of characters from the end of the name using the `Substring()` method.

For example, if the `quantity` field is set to 3, and the script is applied to a parent GameObject with child GameObjects named "Child 1", "Child 2", and "Child 3", the names of the child GameObjects would be changed to "Chi", "Chi", and "Chi", respectively. If the `removeLastChars` field is false, the names of the child GameObjects would not be changed. 

This is used to modify the names of child `GameObjects` based on the values of the `removeLastChars` and `quantity` fields.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class RenameChilds : MonoBehaviour
{
    public bool removeLastChars = false;
    public int quantity = 0;
    private void OnEnable()
    {
        if (removeLastChars)
        {
            Transform[] childs = GetComponentsInChildren<Transform>();
            foreach (Transform t in childs)
            {
                if (t.gameObject.name.Contains("."))
                    t.gameObject.name = t.gameObject.name.Substring(0, t.gameObject.name.Length - quantity);
            }
        }
    }
}
```
</details>     
        
---    
    
### DNAManager.cs
##### Component name: ```DNAManager```
<details>
<summary>Click here to see the details and code</summary>

A script for managing and generating DNA data in a Unity project. The script has several public fields, including references to other scripts, arrays of strings and GameObjects, and TextMeshPro GUI elements. The script also has several methods, including `Start()`, `Update()`, `ExportJsonToText()`, `GetTrait()`, `GetAllTraits()`, `OnCopyToClipboard()`, and `OnCopyToClipboard()`.

The `Start()` method appears to initialize and set the values of some of the script's public fields. The `Update()` method appears to update the values of some of the script's public fields and GUI elements based on the values of other fields. The `ExportJsonToText()` method appears to generate and output a JSON string based on the values of the script's public fields, and the `GetTrait()` method appears to generate and return a string representation of a single DNA trait. The `GetAllTraits()` method appears to call the `GetTrait()` method for each trait and return a concatenated string of all the traits. The `OnCopyToClipboard()` and `OnPasteFromClipboard()` methods handle copying and pasting data to and from the clipboard.

The `OnCopyToClipboard()` and `OnPasteFromClipboard()` methods in the DNAManager script are used to handle copying and pasting data to and from the clipboard.

The `OnCopyToClipboard()` method is called when the user clicks the "Copy to Clipboard" button in the Unity user interface. When this method is called, it copies the value of the `jsonOutputPreview` field to the clipboard. This allows the user to paste the JSON string generated by the script into another application.

The `OnPasteFromClipboard()` method is called when the user clicks the "Paste from Clipboard" button in the Unity user interface. When this method is called, it retrieves the value currently stored in the clipboard and sets the value of the `jsonOutputPreview` field to the clipboard value. This allows the user to paste JSON data from another application into the `DNAManager` script.

Here is an example of how these methods might be used:

* The user clicks the "Copy to Clipboard" button in the Unity user interface.
* The `OnCopyToClipboard()` method is called, which copies the value of the `jsonOutputPreview` field to the clipboard.
* The user switches to a text editor or another application that accepts text input.
* The user pastes the clipboard contents into the application by pressing "Ctrl+V" or using the paste command in the application.
* The user can now edit or use the JSON data as needed in the other application.
* The user makes changes to the JSON data in the other application.
* The user copies the modified JSON data back to the clipboard by highlighting the data and pressing "Ctrl+C" or using the copy command in the application.
* The user switches back to the Unity editor and clicks the "Paste from Clipboard" button.
* The `OnPasteFromClipboard()` method is called, which retrieves the value currently stored
    
```c#
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

    public string GetAllTraits()
    {
        string allTraits;
        
        // check if superrule 1 is true or false
        if(superRuleReference.CheckSuperRule1()){

        allTraits = 
          GetTrait(0) + ",\n" 
        + GetTrait(1) + ",\n"
        + GetTrait(2) + ",\n"
        + GetTrait(3) + ",\n"
        + GetTrait(4) + ",\n"
        + GetTrait(5) + ",\n"
        + GetTrait(6) + ",\n"
        + GetRobotColorTrait() + ",\n"
        + GetBoomboxColorTrait() + ",\n"
        + GetBGColorTrait();
        // end trait function
        }else{

        allTraits = 
          GetTrait(0) + ",\n" 
        + GetTrait(1) + ",\n"
        + GetTrait(2) + ",\n"
        + GetTrait(3) + ",\n"
        + GetTrait(4) + ",\n"
        + GetTrait(5) + ",\n"
        + GetTrait(6) + ",\n"
        + GetRobotColorTrait() + ",\n"
        + GetBoomboxColorTrait() + ",\n"
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




 
```
</details>                 
        
---    
    
### BoomToolsGUIStyles.cs
##### Component name: ```BoomToolsGUIStyles```
<details>
<summary>Click here to see the details and code</summary>

This script defines a utility class called `BoomToolsGUIStyles` that provides two methods for creating custom `GUIStyle`s for use in Unity's GUI system. The first method, `CustomLabel()`, takes three boolean parameters: `center`, `bold`, and `wordWrap`. It creates a new `GUIStyle` based on the default label style and sets its alignment, font style, and word wrap properties based on the values of the parameters. The second method, `CustomColorLabel()`, is similar to the first method but also takes a `color` parameter that is used to set the text color of the new `GUIStyle`. These methods can be used to easily create custom labels with various styles and colors in Unity's GUI system.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomToolsGUIStyles
{
    public static GUIStyle CustomLabel(bool center, bool bold, bool wordWrap)
    {
        TextAnchor anchor = center ? TextAnchor.MiddleCenter : TextAnchor.MiddleLeft;
        FontStyle style = bold ? FontStyle.Bold : FontStyle.Normal;
        return new GUIStyle(GUI.skin.label) { alignment = anchor, fontStyle = style, wordWrap = wordWrap};
    }
    public static GUIStyle CustomColorLabel(bool center, bool bold, bool wordWrap, Color color)
    {
        TextAnchor anchor = center ? TextAnchor.MiddleCenter : TextAnchor.MiddleLeft;
        FontStyle style = bold ? FontStyle.Bold : FontStyle.Normal;
        return new GUIStyle(GUI.skin.label) { alignment = anchor, fontStyle = style, wordWrap = wordWrap, normal = { textColor = color }, hover = { textColor = color } };
    }
}
```
</details>     
        
---    
    
### DNAManager_Editor.cs
##### Component name: ```DNAManager_Editor```
<details>
<summary>Click here to see the details and code</summary>

This script is an editor script for the `DNAManager` class.

Editor scripts allow you to customize the Unity editor interface. This script is specifically creating a custom inspector for the `DNAManager` component, which allows you to view and edit the properties of the component in the Unity editor.

The script is decorated with the `CustomEditor` attribute, which tells Unity to use this script as the custom inspector for the `DNAManager` component.

The script extends the `Editor` class and overrides the OnInspectorGUI method. The `OnInspectorGUI` method is called by the Unity editor when it needs to draw the inspector for the component.

The script defines two buttons: one for the `optionsManager` and another for the `vrmAuthoringManager`. If either of these fields is `null`, the script will display a button that allows you to add a new component of the appropriate type to the component's game object. If either of these fields is not null, the script will display a button that allows you to select the component's game object in the editor.

Finally, the script calls the base `OnInspectorGUI` method, which will draw the default inspector for the component.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DNAManager))]
public class DNAManager_Editor : Editor
{
    DNAManager myScript;
    private void OnEnable()
    {
        myScript = (DNAManager)target;
    }
    public override void OnInspectorGUI()
    {
        if (myScript.optionsManager == null)
        {
            if (GUILayout.Button("Add options manager", GUILayout.Height(30f)))
            {
                myScript.optionsManager = myScript.transform.root.gameObject.AddComponent<OptionsManager>();
                myScript.optionsManager.dnaManager = myScript;
                Selection.activeGameObject = myScript.transform.root.gameObject;
            }
        }
        else
        {
            if (GUILayout.Button("Select options manager", GUILayout.Height(30f)))
            {
                Selection.activeGameObject = myScript.optionsManager.gameObject;
            }
        }



        if (myScript.vrmAuthoringManager == null)
        {
            if (GUILayout.Button("Add VRM meta data", GUILayout.Height(30f)))
            {
                GameObject vrmManagerGameObject = new GameObject("VRM Manager");
                vrmManagerGameObject.transform.parent = myScript.transform.root;
                vrmManagerGameObject.transform.SetAsFirstSibling();
                myScript.vrmAuthoringManager = vrmManagerGameObject.AddComponent<VRMAuthoringManager>();
                myScript.vrmAuthoringManager.dnaManager = myScript;
                Selection.activeGameObject = vrmManagerGameObject;
            }
        }
        else
        {
            if (GUILayout.Button("Select VRM meta data", GUILayout.Height(30f)))
            {
                Selection.activeGameObject = myScript.vrmAuthoringManager.gameObject;
            }
        }
        base.OnInspectorGUI();
    }
}

```
</details>     
        
---    
    
### OptionsManager_Editor.cs
##### Component name: ```OptionsManager_Editor```
<details>
<summary>Click here to see the details and code</summary>

Needs Documentation
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VRM;
[CustomEditor(typeof(OptionsManager))]
public class OptionsManager_Editor : Editor
{
    OptionsManager myScript;
    private void OnEnable()
    {
        myScript = (OptionsManager)target;
    }

    GUIStyle styleCentered, styleTitle, styleTitleCentered, styleCorrect, styleWarning, styleWrong, textFieldStyle, buttonStyle;

    string newOptionsName = "";
    string identifierName = "";
    string traitName = "";

    bool isEditing = false;
    public override void OnInspectorGUI()
    {
        styleCentered = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, wordWrap = true };
        styleTitle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };
        styleTitleCentered = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
        styleCorrect = new GUIStyle(GUI.skin.label) { normal = { textColor = Color.green } };
        styleWarning = new GUIStyle(GUI.skin.label) { normal = { textColor = Color.yellow } };
        styleWrong = new GUIStyle(GUI.skin.label) { normal = { textColor = Color.red } };


        textFieldStyle = new GUIStyle(GUI.skin.textField) { alignment = TextAnchor.MiddleCenter };
        buttonStyle = new GUIStyle(GUI.skin.button);
        //buttonStyle.normal.
        //base.OnInspectorGUI();
        switch (myScript.setupStage) {
            case 0:
                InitialSetup();
                break;
            case 1:
                CharacterSetup();
                break;
            case 2:
                OptionSetup();
                break;
            case 3:
                ActionSetup();
                break;
            case 4:
                RulesSetup();
                break;
        }


    }

    private void InitialSetup()
    {
        if (GUILayout.Button("Go to DNA manager", GUILayout.Height(30f)))
        {
            if (myScript.dnaManager == null)
            {
                myScript.dnaManager = GameObject.FindObjectOfType<DNAManager>();
            }
            if (myScript.dnaManager != null)
            {
                Selection.activeGameObject = myScript.dnaManager.gameObject;
            }
        }
        GUILayout.Space(5f);
        if (GUILayout.Button("Main Models", GUILayout.Height(30f)))
        {
            myScript.CreateBasicCharacterSetup();
            myScript.setupStage = 1;
        }
        bool guiEnabled = true;
        if (myScript.mainCharacterOptions != null)
        {
            if (myScript.mainCharacterOptions.objects == null)
                guiEnabled = false;
            else
                if (myScript.mainCharacterOptions.objects.Count == 0)
                guiEnabled = false;
        }
        else
            guiEnabled = false;

        GUI.enabled = guiEnabled;
        if (GUILayout.Button("Options", GUILayout.Height(30f)))
        {
            myScript.setupStage = 2;
        }

        //check if there is at least 1 options in random objects
        if (myScript.randomObjects != null)
        {
            if (myScript.randomObjects.Count == 0)
                guiEnabled = false;
        }
        else
            guiEnabled = false;

        GUI.enabled = guiEnabled;

        if (GUILayout.Button("Actions", GUILayout.Height(30f)))
        {
            myScript.setupStage = 3;
        }

        if (myScript.actionCallers == null)
            guiEnabled = false;
        else if (myScript.actionCallers.Count == 0)
            guiEnabled = false;
        GUI.enabled = guiEnabled;
        if (GUILayout.Button("Rules", GUILayout.Height(30f)))
        {
            myScript.setupStage = 4;
        }
    }
    private void CharacterSetup()
    {

        // GoBackButton();
        GUILayout.Space(5f);

        #region Main Edit
        if (ActiveEditorTracker.sharedTracker.isLocked)
        {
            isEditing = true;

            if (GUILayout.Button("Finish Editing", GUILayout.Height(30f)))
            {
                ActiveEditorTracker.sharedTracker.isLocked = false;
                Selection.activeGameObject = myScript.gameObject;
            }

            GUILayout.Label("EDIT MODE\n\n *Select gameObjects and click Add selected (you may choose more than 1 at a time)\n*Use Add blendshape only if any of your characters contain this information\n", BoomToolsGUIStyles.CustomColorLabel(true,false,true,Color.yellow));

            // add new options
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Selected", GUILayout.Height(30f)))
            {
                Undo.RecordObject(myScript.mainCharacterOptions, "Add Humanoid Model");

                foreach (Object obj in Selection.objects)
                {
                    if (myScript.mainCharacterOptions.IsValidObjectType(obj))
                    {
                        if (!myScript.mainCharacterOptions.ObjectExists(obj))
                            myScript.mainCharacterOptions.AddObject(obj);
                        else
                            Debug.Log(myScript.mainCharacterOptions.objectName + " already added");
                    }
                    else
                    {
                        Debug.Log("need to add a GameObject from the scene");
                    }
                }
                EditorUtility.SetDirty(myScript);
            }

            if (GUILayout.Button("Add Blendshape", GUILayout.Height(30f)))
            {
                Undo.RecordObject(myScript.mainCharacterAction, "Add Blendshape");
                myScript.mainCharacterAction._AddBlendShape();
            }
            EditorGUILayout.EndHorizontal();

        }
        else
        {
            GoBackButton();
            GUILayout.Label("=Instructions= \n\n Choose gameObjects in your scene hierarchy with Animator component. Animator must have human avatar definition.\n", styleCentered);
            isEditing = false;
            if (GUILayout.Button("Edit", GUILayout.Height(30f)))
                ActiveEditorTracker.sharedTracker.isLocked = true;

        }

        if (myScript.mainCharacterOptions.objects.Count > 1)
        {
            GUILayout.Space(10f);
            string characterTraitName = EditorGUILayout.TextField("Trait name: ", myScript.mainCharacterAction.traitName);
            if (characterTraitName != myScript.mainCharacterAction.traitName)
            {
                Undo.RecordObject(myScript.mainCharacterAction, "Change Character Trait Name");
                myScript.mainCharacterAction.traitName = characterTraitName;
            }
        }
        #endregion

        #region Selected Humanoids
        if (myScript.mainCharacterOptions.objects?.Count > 0)
        {
            GUILayout.Space(5f);
            EditorGUILayout.LabelField("== Humanoid Options ==", styleCentered);
            GUILayout.Space(5f);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Humanoid", BoomToolsGUIStyles.CustomLabel(false, true, false), GUILayout.MinWidth(30f));
            EditorGUILayout.LabelField("Trait Name", BoomToolsGUIStyles.CustomLabel(false, true, false), GUILayout.Width(200f));
            EditorGUILayout.LabelField("Weight", BoomToolsGUIStyles.CustomLabel(true, true, false), GUILayout.Width(40f));
            if (isEditing)
                EditorGUILayout.LabelField("X", BoomToolsGUIStyles.CustomLabel(true,true,false), GUILayout.Width(20f));
            EditorGUILayout.EndHorizontal();
        }
        bool fixables = false;
        for (int i = 0; i < myScript.mainCharacterOptions.objects.Count; i++)
        {
            if (myScript.mainCharacterOptions.objects[i] != null)
            {

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.ObjectField(myScript.mainCharacterOptions.objects[i], typeof(Object), true);

                GameObject go = myScript.mainCharacterOptions.objects[i] as GameObject;
                Animator goAnim = go.GetComponent<Animator>();
                Avatar animAvt = goAnim == null ? null : goAnim.avatar;

                string errorField = "";
                if (goAnim == null) errorField = "No animator in gameObject";
                else if (goAnim.avatar == null) errorField = "No Avatar in Animator";
                else if (!goAnim.avatar.isHuman) errorField = "Non Human Avatar setup";
                else if (!goAnim.avatar.isValid) errorField = "Non Valid Animator Avatar setup";

                SkinnedMeshRenderer rend = go.GetComponentInChildren<SkinnedMeshRenderer>();
                bool correctRend = true;
                if (rend == null)
                {
                    errorField = "No skinned mesh render";
                    correctRend = false;
                }
                else
                {
                    if (!rend.sharedMesh.isReadable)
                    {
                        errorField = "Non readable mesh";
                        correctRend = false;
                    }
                }

                if (isEditing)
                {
                    if (errorField == "")
                    {
                        string trait = EditorGUILayout.TextField(myScript.mainCharacterOptions.nameTraits[i], GUILayout.Width(200f));
                        if (trait != myScript.mainCharacterOptions.nameTraits[i])
                        {
                            Undo.RecordObject(myScript.mainCharacterOptions, "Set Trait Name Value");
                            myScript.mainCharacterOptions.nameTraits[i] = trait;
                        }
                        int weight = EditorGUILayout.IntField(myScript.mainCharacterOptions.weights[i], BoomToolsGUIStyles.CustomLabel(true,false,false), GUILayout.Width(40f));
                        if (weight != myScript.mainCharacterOptions.weights[i])
                        {
                            Undo.RecordObject(myScript.mainCharacterOptions, "Set Weight Value");
                            myScript.mainCharacterOptions.weights[i] = weight;
                        }
                        if (GUILayout.Button("x", GUILayout.Width(20f)))
                        {
                            Undo.RecordObject(myScript.mainCharacterOptions, "Remove Object");
                            myScript.mainCharacterOptions.RemoveAtIndex(i);
                        }
                    }
                    else
                    {

                        EditorGUILayout.LabelField(errorField, BoomToolsGUIStyles.CustomColorLabel(false, false, false, Color.red), GUILayout.Width(200f));

                        if (PrefabUtility.IsPartOfModelPrefab(go))
                        {
                            if (goAnim == null || goAnim.avatar == null)
                            {
                                fixables = true;
                                if (GUILayout.Button("Fix", GUILayout.Width(40f)))
                                {
                                    TryFixHumanoidOptions(go);
                                }
                            }
                            else
                            {
                                if (GUILayout.Button("MFix", GUILayout.Width(40f)))
                                {
                                    Object obj = PrefabUtility.GetCorrespondingObjectFromOriginalSource(go);
                                    ActiveEditorTracker.sharedTracker.isLocked = false;
                                    EditorGUIUtility.PingObject(obj);
                                    Selection.activeObject = obj;
                                }
                            }
                        }
                        else
                        {
                            EditorGUILayout.LabelField("Fix", GUILayout.Width(40f));
                        }
                        if (GUILayout.Button("x", GUILayout.Width(20f)))
                        {
                            Undo.RecordObject(myScript.mainCharacterOptions, "Remove Object");
                            myScript.mainCharacterOptions.RemoveAtIndex(i);
                        }

                    }

                }
                else
                {
                    if (errorField == "")
                    {
                        EditorGUILayout.LabelField(myScript.mainCharacterOptions.nameTraits[i], BoomToolsGUIStyles.CustomColorLabel(false, false, false, Color.green), GUILayout.Width(200f));
                        EditorGUILayout.LabelField(myScript.mainCharacterOptions.weights[i].ToString(), GUILayout.Width(40f));
                    }
                    else
                    {
                        EditorGUILayout.LabelField(errorField, BoomToolsGUIStyles.CustomColorLabel(false, false, false, Color.red), GUILayout.Width(200f));
                        if (PrefabUtility.IsPartOfModelPrefab(go))
                        {
                            if (goAnim == null || goAnim.avatar == null || !correctRend)
                            {
                                fixables = true;
                                if (GUILayout.Button("Fix", GUILayout.Width(40f)))
                                {
                                    TryFixHumanoidOptions(go);
                                }
                            }
                            else
                            {
                                if (GUILayout.Button("MFix", GUILayout.Width(40f)))
                                {
                                    Object obj = PrefabUtility.GetCorrespondingObjectFromOriginalSource(go);
                                    ActiveEditorTracker.sharedTracker.isLocked = false;
                                    EditorGUIUtility.PingObject(obj);
                                    Selection.activeObject = obj;
                                }
                            }
                        }
                        else
                        {
                            EditorGUILayout.LabelField("Fix", GUILayout.Width(40f));
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                myScript.mainCharacterOptions.RemoveAtIndex(i);
            }
        }
        if (fixables)
        {
            EditorGUILayout.LabelField("*Objects are required to be humanoid");
            if (GUILayout.Button("Try quick fix", GUILayout.Height(30f)))
            {
                FixAllHumanoids(myScript.mainCharacterOptions.objects);
            }
        }
        #endregion

        #region Blendshapes
        GUILayout.Space(5f);
        if (isEditing)
        {

            if (myScript.mainCharacterAction.blendShapes?.Count > 0)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };

                GUILayout.Space(5f);
                EditorGUILayout.LabelField("== BlendShapes ==", style);
                GUILayout.Space(5f);

                EditorGUILayout.LabelField("*Non case sensitive", GUILayout.Height(20f));
                EditorGUILayout.LabelField("*Leave empty text field to use shape name", GUILayout.Height(20f));
                EditorGUILayout.Space();

                for (int i = 0; i < myScript.mainCharacterAction.blendShapes.Count; i++)
                {
                    BlendShapePreset shape = myScript.mainCharacterAction.blendShapes[i];
                    string name = myScript.mainCharacterAction.blendShapeNames[i];
                    EditorGUILayout.BeginHorizontal();
                    shape = (BlendShapePreset)EditorGUILayout.EnumPopup(shape);
                    name = EditorGUILayout.TextField(name);

                    if (name != myScript.mainCharacterAction.blendShapeNames[i] || shape != myScript.mainCharacterAction.blendShapes[i])
                    {
                        Undo.RecordObject(myScript.mainCharacterAction, "Set blendshapes value");
                        myScript.mainCharacterAction.blendShapeNames[i] = name;
                        myScript.mainCharacterAction.blendShapes[i] = shape;
                    }

                    if (GUILayout.Button("X"))
                    {
                        Undo.RecordObject(myScript.mainCharacterAction, "Remove Blendshape");
                        myScript.mainCharacterAction._RemoveBlendshape(i);
                    }
                    EditorGUILayout.EndVertical();
                }
            }
        }
        #endregion 
    }
    private void FixAllHumanoids(List<Object> humanoids)
    {
        foreach (Object obj in humanoids)
        {
            TryFixHumanoidOptions(obj as GameObject, false);
        }
    }
    private void TryFixHumanoidOptions(GameObject go, bool debug = true)
    {
        string path = AssetDatabase.GetAssetPath(PrefabUtility.GetCorrespondingObjectFromOriginalSource(go));

        ModelImporter importer = ModelImporter.GetAtPath(path) as ModelImporter;

        if (importer == null)
        {
            if (debug) Debug.LogWarning("Cant fix, require manual fixing");
        }
        else
        {
            importer.animationType = ModelImporterAnimationType.Human;
            importer.isReadable = true;
            importer.autoGenerateAvatarMappingIfUnspecified = true;

            AssetDatabase.ImportAsset(path);

            Animator anim = go.GetComponent<Animator>();
            if (anim == null)
                anim = go.AddComponent<Animator>();
            if (anim.avatar == null)
                anim.avatar = importer.sourceAvatar;
        }
    }
    private void OptionSetup()
    {
        GoBackButton();
        GUILayout.Space(5f);
        GUILayout.Label("=Instructions= \n\n Create options for traits: Type a name for the options, then use the buttons below to define the type of options they can choose from.\n", styleCentered);

        GUILayout.Space(5f);
        newOptionsName = EditorGUILayout.TextField("New options Name:", newOptionsName);
        GUILayout.Space(5f);

        if (newOptionsName.Length == 0)
            GUI.enabled = false;
        else
            GUI.enabled = true;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("GameObjects", GUILayout.Height(30f)))
        {
            Undo.RegisterCreatedObjectUndo(myScript.AddRandomObjectOption(typeof(RandomGameObject), newOptionsName), "Object Option");
            newOptionsName = "";
            GUI.FocusControl(null);
        }
        if (GUILayout.Button("Textures", GUILayout.Height(30f)))
        {
            Undo.RegisterCreatedObjectUndo(myScript.AddRandomObjectOption(typeof(RandomTexture), newOptionsName), "Texture Option");
            newOptionsName = "";
            GUI.FocusControl(null);
        }
        if (GUILayout.Button("Materials", GUILayout.Height(30f)))
        {
            Undo.RegisterCreatedObjectUndo(myScript.AddRandomObjectOption(typeof(RandomMaterial), newOptionsName), "Material Option");
            newOptionsName = "";
            GUI.FocusControl(null);
        }
        //if (GUILayout.Button("Materials")) Selection.activeTransform = myScript.AddRandomObjectOption(typeof(RandomMaterial));
        EditorGUILayout.EndHorizontal();
        GUI.enabled = true;
        GUILayout.Space(5f);
        if (myScript.randomObjects?.Count > 0)
        {
            EditorGUILayout.LabelField("== Current OPTIONS ==\n\n Click on view to add, remove, edit and set weight on options\n", styleCentered);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Name", styleTitle, GUILayout.MinWidth(30f));
            EditorGUILayout.LabelField("Type", styleTitle, GUILayout.Width(70f));
            EditorGUILayout.LabelField("Options", styleTitle, GUILayout.Width(70f));
            EditorGUILayout.LabelField("Edit", styleTitleCentered, GUILayout.Width(60f));
            EditorGUILayout.LabelField("X", styleTitleCentered, GUILayout.Width(20f));
            EditorGUILayout.EndHorizontal();


            for (int i = 0; i < myScript.randomObjects.Count; i++)
            {
                if (myScript.randomObjects[i] == null)
                {
                    myScript.RemoveElement(ref myScript.randomObjects,i);
                }
                else
                {
                    RandomObject ro = myScript.randomObjects[i].GetComponent<RandomObject>();

                    string labelType;
                    switch (ro.GetType().ToString())
                    {
                        case "RandomGameObject":
                            labelType = "GObject";
                            break;
                        case "RandomTexture":
                            labelType = "Texture";
                            break;
                        case "RandomMaterial":
                            labelType = "Material";
                            break;
                        default:
                            labelType = "Generic";
                            break;
                    }
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(myScript.randomObjects[i].name, BoomToolsGUIStyles.CustomColorLabel(false,false,false, ro.HasCorrectSetup() ? Color.green:Color.red), GUILayout.MinWidth(30f));
                    EditorGUILayout.LabelField(labelType, GUILayout.Width(70f));
                    int val = ro.objects == null ? 0 : ro.objects.Count;
                    EditorGUILayout.LabelField(val.ToString(), val == 0 ? styleWrong : styleCorrect, GUILayout.Width(70f));


                    if (GUILayout.Button("View", GUILayout.Width(60f))) Selection.activeGameObject = myScript.randomObjects[i];
                    if (GUILayout.Button("X", GUILayout.Width(20f)))
                    {
                        Undo.RegisterFullObjectHierarchyUndo(myScript.optionsHolder, "Remove Random Options");
                        Undo.RecordObject(myScript, "Remove Random Options");
                        myScript.RemoveElement(ref myScript.randomObjects, i);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
    }

    private void ActionSetup()
    {
        GoBackButton();
        GUILayout.Space(5f);
        GUILayout.Label("=Instructions= " +
            "\n\nAdd action traits and assign previous options to them.\n" +
            "*Identifier name: a custom name to quickly identify this action\n" +
            "*Trait name: this name will be exported in the final json file\n", styleCentered);

        GUILayout.Space(5f);

        identifierName = EditorGUILayout.TextField("Identifier Name: ", identifierName);
        traitName = EditorGUILayout.TextField("Trait Name: ", traitName);

        GUILayout.Space(5f);

        if (identifierName.Length == 0 || traitName.Length == 0)
            GUI.enabled = false;
        else
            GUI.enabled = true;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Display Chosen Object", GUILayout.Height(30f)))
        {
            GameObject newObj = myScript.AddActionCaller(typeof(SetObjectsVisibility), identifierName, traitName);
            Undo.RegisterCreatedObjectUndo(newObj, "New Set Object Visibility Action");
            traitName = "";
            identifierName = "";
            GUI.FocusControl(null);
        }
        if (GUILayout.Button("Set Texture to material", GUILayout.Height(30f)))
        {
            GameObject newObj = myScript.AddActionCaller(typeof(SetTextureToMaterial), identifierName, traitName);
            Undo.RegisterCreatedObjectUndo(newObj, "New Set Texture To Material Action");
            traitName = "";
            identifierName = "";
            GUI.FocusControl(null);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Set Material To Model", GUILayout.Height(30f)))
        {
            GameObject newObj = myScript.AddActionCaller(typeof(SetMaterialToMesh), identifierName, traitName);
            Undo.RegisterCreatedObjectUndo(newObj, "New Set Material To Mesh Action");
            traitName = "";
            identifierName = "";
            GUI.FocusControl(null);
        }
        EditorGUILayout.EndHorizontal();
        GUI.enabled = true;
        if (myScript.actionCallers?.Count > 0)
        {
            EditorGUILayout.LabelField("== Current ACTIONS ==", styleCentered, GUILayout.Height(20f));
            EditorGUILayout.LabelField("\nIf text displays red, it means some values are still required for this action to work, a green color means it's ready, and yellow the option type required\n", styleCentered);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Name", styleTitle, GUILayout.MinWidth(30f));
            EditorGUILayout.LabelField("Type", styleTitle, GUILayout.Width(60f));
            EditorGUILayout.LabelField("Option", styleTitle, GUILayout.Width(100f));
            EditorGUILayout.LabelField("Trait", styleTitle, GUILayout.Width(70f));
            EditorGUILayout.LabelField("Edit", styleTitleCentered, GUILayout.Width(50f));
            EditorGUILayout.LabelField("X", styleTitleCentered, GUILayout.Width(20f));
            EditorGUILayout.EndHorizontal();
            for (int i = 0; i < myScript.actionCallers.Count; i++)
            {
                if (myScript.actionCallers[i] == null)
                {
                    myScript.RemoveElement(ref myScript.actionCallers, i);
                }
                else
                {
                    ActionCaller ac = myScript.actionCallers[i].GetComponent<ActionCaller>();

                    string labelType;
                    switch (ac.GetType().ToString())
                    {
                        case "SetObjectsVisibility":
                            labelType = "GObject";
                            break;
                        case "SetTextureToMaterial":
                            labelType = "Texture";
                            break;
                        case "SetMaterialToMesh":
                            labelType = "Material";
                            break;
                        default:
                            labelType = "Generic";
                            break;
                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(myScript.actionCallers[i].name, ac.IsValidTrait() ? styleCorrect : styleWrong, GUILayout.MinWidth(30f));
                    EditorGUILayout.LabelField(labelType, styleTitle, GUILayout.Width(60f));
                    if (ac.randomTarget == null)
                        EditorGUILayout.LabelField("--", GUILayout.Width(100f));
                    else
                        EditorGUILayout.LabelField(ac.randomTarget.gameObject.name, GUILayout.Width(100f));
                    EditorGUILayout.LabelField(ac.traitName, GUILayout.Width(70f));
                    if (GUILayout.Button("Edit", GUILayout.Width(50f))) Selection.activeGameObject = myScript.actionCallers[i];
                    if (GUILayout.Button("X", GUILayout.Width(20f)))
                    {
                        Undo.RegisterFullObjectHierarchyUndo(myScript.actionsHolder, "Remove Random Options");
                        Undo.RecordObject(myScript, "Remove Random Options");
                        myScript.RemoveElement(ref myScript.actionCallers, i);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
    }

    string ruleIdentifier = "";
    private void RulesSetup()
    {
        GoBackButton();
        GUILayout.Space(5f);
        GUILayout.Label("=Instructions= " +
            "\n\nAdd Rules, what options can exist and cannot when other options are chosen.\n", styleCentered);
        GUILayout.Space(5f);

        
        ruleIdentifier = EditorGUILayout.TextField("Rule Name: ", ruleIdentifier);


        if (ruleIdentifier.Length == 0)
            GUI.enabled = false;
        else
            GUI.enabled = true;
        if (GUILayout.Button("Add New Rule", GUILayout.Height(30f)))
        {
            GameObject newObj = myScript.AddRule(ruleIdentifier);
            Undo.RegisterCreatedObjectUndo(newObj, "New Super Rule");
            ruleIdentifier = "";
            GUI.FocusControl(null);
        }
        GUILayout.Space(5f);


        GUI.enabled = true;
        if (myScript.superRules?.Count > 0)
        {
            EditorGUILayout.LabelField("== Current Rules ==", styleCentered, GUILayout.Height(20f));
            EditorGUILayout.LabelField("\nIf text displays red, it means some values are still required for this rules to apply and green color means it's ready\n", styleCentered);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Name", styleTitle, GUILayout.MinWidth(30f));
            EditorGUILayout.LabelField("Edit", styleTitleCentered, GUILayout.Width(50f));
            EditorGUILayout.LabelField("X", styleTitleCentered, GUILayout.Width(20f));
            EditorGUILayout.EndHorizontal();


            for (int i = 0; i < myScript.superRules.Count; i++)
            {
                if (myScript.superRules[i] == null)
                {
                    myScript.RemoveElement(ref myScript.superRules, i);
                }
                else
                {
                    SuperRules rule = myScript.superRules[i].GetComponent<SuperRules>();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(myScript.superRules[i].name, rule.IsActiveRule() ? styleCorrect : styleWrong, GUILayout.MinWidth(30f));
                    if (GUILayout.Button("Edit", GUILayout.Width(50f))) Selection.activeGameObject = myScript.superRules[i];
                    if (GUILayout.Button("X", GUILayout.Width(20f)))
                    {
                        Undo.RegisterFullObjectHierarchyUndo(myScript.rulesHolder, "Remove Rule GameObject");
                        Undo.RecordObject(myScript, "Remove Super Rule");
                        myScript.RemoveElement(ref myScript.superRules, i);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
    }
    private void GoBackButton()
    {
        
        if (GUILayout.Button("Back To Manager", GUILayout.Height(30f)))
        {
            Selection.activeGameObject = myScript.gameObject;
            myScript.setupStage = 0;
            GUI.FocusControl(null);
            ActiveEditorTracker.sharedTracker.isLocked = false;
        }
    }

}

```
</details>     
        
---    
    
### VRMAuthoringManager_Editor
##### Component name: ```VRMAuthoringManager_Editor```
<details>
<summary>Click here to see the details and code</summary>

This script is a custom editor for the `VRMAuthoringManager` script. A custom editor allows you to customize the Inspector GUI for a script. In this case, the Inspector GUI will display a "Back To DNA Manager" button if the `dnaManager` field is not null. Otherwise, it will display an object field where you can assign a `DNAManager` object to the `dnaManager` field. When the "Back To DNA Manager" button is clicked, the selected GameObject in the editor will be set to the GameObject that the `dnaManager` is attached to. The base implementation of `OnInspectorGUI` is also called, which will draw the default Inspector GUI for the script.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VRMAuthoringManager))]
public class VRMAuthoringManager_Editor : Editor
{
    VRMAuthoringManager myScript;
    private void OnEnable()
    {
        myScript = (VRMAuthoringManager)target;
    }

    public override void OnInspectorGUI()
    {
        if (myScript.dnaManager != null)
        {
            if (GUILayout.Button("Back To DNA Manager", GUILayout.Height(30f))) Selection.activeGameObject = myScript.dnaManager.gameObject;
        }
        else
        {
            myScript.dnaManager = (DNAManager)EditorGUILayout.ObjectField("DNA Manager", myScript.dnaManager, typeof(DNAManager),true);
        }
        base.OnInspectorGUI();
    }
}

```
</details>     
        
---    
    
### OptionsManager.cs
##### Component name: ```OptionsManager```
<details>
<summary>Click here to see the details and code</summary>

This script is a manager for options, actions, and rules in a game or application. It has several methods that allow you to add different types of options, actions, and rules to the scene, as well as methods for removing elements and retrieving lists of elements of specific types.

The `OptionsManager` has several fields for storing lists of options, actions, and rules. It also has several fields for storing reference to objects that act as holders for these elements.

The `AddRandomObjectOption`, `AddActionCaller`, and `AddRule` methods allow you to add new options, actions, and rules to the scene, respectively. These methods create new GameObjects, add the appropriate component to them based on the specified type, set the parent of the GameObject to the appropriate holder, and add the GameObject to the appropriate list. The `RemoveElement` and `RemoveActionCaller` methods allow you to remove elements from their respective lists and destroy the GameObjects. The `GetActionCallersOfType` method allows you to retrieve a list of action callers of a specific type. The `GetRandomObjectsOfType` method allows you to retrieve a list of random objects of a specific type.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    public List<GameObject> randomObjects;
    public List<GameObject> actionCallers;
    public List<GameObject> superRules;

    public RandomGameObject mainCharacterOptions;
    public SetObjectsVisibility mainCharacterAction;

    public RandomGameObject mainVRMStructure;

    public GameObject optionsHolder;
    public GameObject actionsHolder;
    public GameObject characterHolder;
    public GameObject rulesHolder;

    public int setupStage = 0;

    public DNAManager dnaManager;
    // random objects
    public GameObject AddRandomObjectOption(System.Type type, string name = "")
    {
        if (type.IsSubclassOf(typeof(RandomObject)))
        {
            if (optionsHolder == null) CreateHolder(ref optionsHolder, "options");   
            if (randomObjects == null) randomObjects = new List<GameObject>();
            
            GameObject newObj = new GameObject(name);
            newObj.transform.parent = optionsHolder.transform;
            newObj.AddComponent(type).GetComponent<RandomObject>().optionsManager = this;
            randomObjects.Add(newObj);

            return newObj;
        }
        else
        {
            Debug.LogWarning("Not a valid random object type: " + type);
            return null;
        }
    }






    // action callers
    public GameObject AddActionCaller(System.Type type,string name = "", string traitName = "")
    {
        if (type.IsSubclassOf(typeof(ActionCaller)))
        {
            if (actionsHolder == null) CreateHolder(ref actionsHolder, "actions");
            if (actionCallers == null) actionCallers = new List<GameObject>();

            GameObject newObj = new GameObject(name);
            newObj.transform.parent = actionsHolder.transform;
            newObj.AddComponent(type);
            ActionCaller newAction = newObj.GetComponent<ActionCaller>();
            newAction.optionsManager = this;
            newAction.traitName = traitName;

            actionCallers.Add(newObj);
            return newObj;
        }
        else
        {
            Debug.LogWarning("Not a valid action caller type: " + type);
            return null;
        }
    }

    public GameObject AddRule(string name)
    {
        if (rulesHolder == null) CreateHolder(ref rulesHolder, "rules");
        if (superRules == null) superRules = new List<GameObject>();

        GameObject newObj = new GameObject(name);
        newObj.transform.parent = rulesHolder.transform;
        newObj.AddComponent(typeof(SuperRules));
        SuperRules newRule = newObj.GetComponent<SuperRules>();
        newRule.optionsManager = this;

        superRules.Add(newObj);
        return newObj;
    }

    private void CreateHolder (ref GameObject storeGo, string name)
    {
        storeGo = new GameObject(name);
        storeGo.transform.SetParent(transform);
    }

    public void RemoveElement(ref List<GameObject> list, int index)
    {
        if (list != null)
        {
            if (list[index] != null)
            {
                DestroyImmediate(list[index]);
            }
            list.RemoveAt(index);
        }
    }

    public void RemoveActionCaller(int index)
    {
        if (actionCallers[index] != null)
            DestroyImmediate(actionCallers[index]);
        actionCallers.RemoveAt(index);
    }


    public List<ActionCaller> GetActionCallersOfType(System.Type type)
    {
        List<ActionCaller> result = new List<ActionCaller>();
        foreach (GameObject go in actionCallers)
        {
            if (go != null)
            {
                ActionCaller caller = go.GetComponent(type) as ActionCaller;
                if (caller != null)
                    result.Add(caller);

            }
        }
        return result;
    }

    public List<RandomObject> GetRandomObjectOfType(System.Type type)
    {
        List<RandomObject> result = new List<RandomObject>();
        foreach (GameObject go in randomObjects)
        {
            if (go != null)
            {
                RandomObject rand = go.GetComponent(type) as RandomObject;
                if (rand != null)
                    result.Add(rand);

            }
        }
        return result;
    }

    //
    public void SetMainVRM()
    {
        Debug.Log("set main action caller vrm");
    }

    public void CreateBasicCharacterSetup()
    {

        if (characterHolder == null)
        {
            characterHolder = new GameObject("character");
            characterHolder.transform.parent = transform;
        }

        if (mainCharacterOptions == null) 
        {
            mainCharacterOptions = characterHolder.AddComponent<RandomGameObject>();
            mainCharacterOptions.ResetObjects();
        }

        if (mainCharacterAction == null)
        {
            mainCharacterAction = characterHolder.AddComponent<SetObjectsVisibility>();
            mainCharacterAction.randomTarget = mainCharacterOptions;
            mainCharacterAction.traitName = "body";
        }
  
    }


    public void AttachDataToDNA(DNAManager dna)
    {
        ConfigureRandomizer randomizer = dna.transform.parent.GetComponentInChildren<ConfigureRandomizer>();
        randomizer.exportVRMFromRandomTrait = mainCharacterAction;

        RandomizeAll randomizeAll = dna.transform.parent.GetComponentInChildren<RandomizeAll>();
        randomizeAll.exportVRMFromRandomTrait = mainCharacterAction;
        randomizeAll.parentRandomTraitCaller = gameObject;

    }
}

```
</details>     
        
---    
    
### VRMAuthoringManager.cs
##### Component name: ```VRMAuthoringManager```
<details>
<summary>Click here to see the details and code</summary>

This script is a manager for VRM (Virtual Reality Model) authoring. It has several fields that store information about the VRM, such as the title, creator, and contact information. The `allowedUser` field is an enumeration that specifies who is allowed to use the VRM, and the `license` field specifies the license type for the VRM. The `storeScreenshotsInVRM` field specifies whether screenshots of the VRM should be stored in the VRM file. The dnaManager field is a reference to a `DNAManager` object, but it is marked as `[HideInInspector]`, which means that it will not be displayed in the Inspector GUI.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;
public class VRMAuthoringManager : MonoBehaviour
{
    [HideInInspector]
    public DNAManager dnaManager;

    public string title, creator, contactInformation, reference, version, additionalLicenseInfoURL;

    public bool depictionOfViolence = false, depictionOfSexualActs = false, commercialUse = false ;

    public AllowedUser allowedUser = AllowedUser.OnlyAuthor;
    public LicenseType license = LicenseType.CC0;

    public bool storeScreenshotsInVRM = false;

}

```
</details>     
        
---    
    
### ChatController.cs
##### Component name: ```ChatController```
<details>
<summary>Click here to see the details and code</summary>

This script is a chat controller for a chat application. It has several public fields:

* `ChatInputField`: a reference to a TMP (Text Mesh Pro) Input Field object that allows the user to input text
* `ChatDisplayOutput`: a reference to a TMP Text object that displays the chat output
* `ChatScrollbar`: a reference to a Scrollbar object that is used to scroll through the chat output

When the script is enabled, it adds a listener to the `onSubmit` event of the `ChatInputField`. This event is triggered when the user submits their chat message by pressing the Enter key or clicking the submit button. The listener is a method called `AddToChatOutput`, which adds the new chat message to the `ChatDisplayOutput` field and formats it with a timestamp. The script also sets the value of the `ChatScrollbar` to 0 to make sure that the most recent message is visible at the bottom of the chat window.
    
```c#
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatController : MonoBehaviour {


    public TMP_InputField ChatInputField;

    public TMP_Text ChatDisplayOutput;

    public Scrollbar ChatScrollbar;

    void OnEnable()
    {
        ChatInputField.onSubmit.AddListener(AddToChatOutput);
    }

    void OnDisable()
    {
        ChatInputField.onSubmit.RemoveListener(AddToChatOutput);
    }


    void AddToChatOutput(string newText)
    {
        // Clear Input Field
        ChatInputField.text = string.Empty;

        var timeNow = System.DateTime.Now;

        string formattedInput = "[<#FFFF80>" + timeNow.Hour.ToString("d2") + ":" + timeNow.Minute.ToString("d2") + ":" + timeNow.Second.ToString("d2") + "</color>] " + newText;

        if (ChatDisplayOutput != null)
        {
            // No special formatting for first entry
            // Add line feed before each subsequent entries
            if (ChatDisplayOutput.text == string.Empty)
                ChatDisplayOutput.text = formattedInput;
            else
                ChatDisplayOutput.text += "\n" + formattedInput;
        }

        // Keep Chat input field active
        ChatInputField.ActivateInputField();

        // Set the scrollbar to the bottom when next text is submitted.
        ChatScrollbar.value = 0;
    }

}

```
</details>     
        
---    
    
### DropdownSample:.cs
##### Component name: ```DropdownSample```
<details>
<summary>Click here to see the details and code</summary>

Sample of a dropdown menu
    
```c#
using TMPro;
using UnityEngine;

public class DropdownSample: MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI text = null;

	[SerializeField]
	private TMP_Dropdown dropdownWithoutPlaceholder = null;

	[SerializeField]
	private TMP_Dropdown dropdownWithPlaceholder = null;

	public void OnButtonClick()
	{
		text.text = dropdownWithPlaceholder.value > -1 ? "Selected values:\n" + dropdownWithoutPlaceholder.value + " - " + dropdownWithPlaceholder.value : "Error: Please make a selection";
	}
}
```
</details>     
        
---    
    
### EnvMapAnimator.cs
##### Component name: ```EnvMapAnimator```
<details>
<summary>Click here to see the details and code</summary>

This script is an animator for an environment map (a type of texture that can be used to add reflections to an object). It has a public field called `RotationSpeeds` that stores the rotation speeds for the environment map. It also has two private fields:

* `m_textMeshPro`: a reference to a TMP (Text Mesh Pro) Text object
* `m_material`: a reference to the material that is used by the `m_textMeshPro` object

The `Awake` function gets a reference to the `m_textMeshPro` and m_material fields when the script is enabled. The `Start` function is a coroutine that updates the `_EnvMatrix` property of the `m_material` with a matrix that rotates the environment map based on the values in the `RotationSpeeds` field. This causes the environment map to animate as it rotates. The coroutine runs indefinitely, and the environment map continues to rotate as long as the script is enabled.
    
```c#
using UnityEngine;
using System.Collections;
using TMPro;

public class EnvMapAnimator : MonoBehaviour {

    //private Vector3 TranslationSpeeds;
    public Vector3 RotationSpeeds;
    private TMP_Text m_textMeshPro;
    private Material m_material;
    

    void Awake()
    {
        //Debug.Log("Awake() on Script called.");
        m_textMeshPro = GetComponent<TMP_Text>();
        m_material = m_textMeshPro.fontSharedMaterial;
    }

    // Use this for initialization
	IEnumerator Start ()
    {
        Matrix4x4 matrix = new Matrix4x4(); 
        
        while (true)
        {
            //matrix.SetTRS(new Vector3 (Time.time * TranslationSpeeds.x, Time.time * TranslationSpeeds.y, Time.time * TranslationSpeeds.z), Quaternion.Euler(Time.time * RotationSpeeds.x, Time.time * RotationSpeeds.y , Time.time * RotationSpeeds.z), Vector3.one);
             matrix.SetTRS(Vector3.zero, Quaternion.Euler(Time.time * RotationSpeeds.x, Time.time * RotationSpeeds.y , Time.time * RotationSpeeds.z), Vector3.one);

            m_material.SetMatrix("_EnvMatrix", matrix);

            yield return null;
        }
	}
}

```
</details>     
        
---    
    
### ActionCaller_Editor.cs
##### Component name: ```ActionCaller_Editor```
<details>
<summary>Click here to see the details and code</summary>

This script is a custom editor for a class called `ActionCaller`. A custom editor is a script that allows you to customize the Inspector GUI for a particular script or component in Unity.

The custom editor has several fields:

* `myScript`: a reference to the `ActionCaller` script that is being edited
* `options`: a string array that stores a list of options for a dropdown menu in the Inspector GUI
* `randomList`: a list of `RandomObject` objects
* `curRandom`: an integer that stores the index of the currently selected option in the dropdown menu

The `OnEnable` function gets a reference to the `myScript` field and initializes the `options` and `randomList` fields by calling the `FetchRandomOptions` and `GetCurrentSelectedRandom` functions.

The `OnInspectorGUI` function is called when the Inspector GUI is rendered. It has several elements that allow the user to edit the properties of the `ActionCaller` script. It has a dropdown menu that displays the list of options stored in the `options` field, and it allows the user to select one of these options as the target for the `ActionCaller`. It also has a text field that allows the user to enter a trait name for the `ActionCaller`. If the user makes any changes to these fields, the custom editor records an undo operation and updates the corresponding properties of the `ActionCaller` script.

The `FetchRandomOptions` function initializes the `options` and `randomList` fields by calling the `GetRandomObjectOfType` method of the `OptionsManager` script and passing it the type of `RandomObject` that is valid for the `ActionCaller`. It then populates the `options` field with the names of the `RandomObject` objects that are returned by `GetRandomObjectOfType`, and it stores these objects in the `randomList` field. This allows the custom editor to display the names of the `RandomObject` objects as options in the dropdown menu and to reference the actual objects when the user selects one of these options.

the script is specifically for a custom editor for the ActionCaller script, and it is used to customize the Inspector GUI for the ActionCaller script. The custom editor allows the user to select a target `RandomObject` for the `ActionCaller` from a dropdown menu, as well as enter a trait name for the `ActionCaller`. It also records an undo operation and updates the corresponding properties of the `ActionCaller` script when the user makes any changes to these fields.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActionCaller), true)]
public class ActionCaller_Editor : Editor
{
    ActionCaller myScript;
    string[] options;
    List<RandomObject> randomList;
    [SerializeField]
    int curRandom;
    protected virtual void OnEnable()
    {
        myScript = (ActionCaller)target;
        if (myScript.optionsManager != null)
        {
            FetchRandomOptions(myScript.optionsManager, myScript.GetRandomObjectValidType());
            curRandom = GetCurrentSelectedRandom(myScript.randomTarget);
        }
    }
    public override void OnInspectorGUI()
    {
        //temporal
        //base.OnInspectorGUI();

        if (myScript.optionsManager != null)
        {
            if (GUILayout.Button("Back to create actions", GUILayout.Height(30f)))
            {
                Selection.activeObject = myScript.optionsManager.gameObject;
            }

            int newRandom = EditorGUILayout.Popup("Target Options: ", curRandom, options);
            if (newRandom != curRandom)
            {
                Undo.RecordObject(myScript, "Change Random Target");
                Undo.RecordObject(this, "Change Random Target");
                myScript.randomTarget = randomList[newRandom];
                curRandom = newRandom;
            }
            string traitName = EditorGUILayout.TextField("Trait Name: ", myScript.traitName);
            if (traitName != myScript.traitName)
            {
                Undo.RecordObject(myScript, "Change Trait Name");
                myScript.traitName = traitName;
            }
        }
        else
        {
            base.OnInspectorGUI();
        }



        if (myScript.randomTarget != null)
        {
            if (!myScript.IsValidType())
            {
                myScript.randomTarget = null;
                Debug.LogWarning("Not a valid type of script for targetRandom in: " + myScript.gameObject.name);
            }
        }
    }


    private void FetchRandomOptions(OptionsManager optionManager, System.Type type)
    {
        randomList = optionManager.GetRandomObjectOfType(type);
        options = new string[randomList.Count];
        for (int i =0; i < randomList.Count; i++)
        {
            options[i] = randomList[i].gameObject.name;
        }
    }
    private int GetCurrentSelectedRandom(RandomObject curRandom)
    {
        if (curRandom == null)
            return -1;
        foreach (RandomObject rand in randomList)
        {
            if (curRandom == rand)
                return randomList.IndexOf(rand);
        }
        return -1;
    }
}

```
</details>     
        
---    
    
### SetMaterialToMesh_Editor.cs
##### Component name: ```SetMaterialToMesh_Editor```
<details>
<summary>Click here to see the details and code</summary>

This script is a custom editor for a script called `SetMaterialToMesh`. It allows you to customize the Inspector GUI for the `SetMaterialToMesh` script in Unity. The custom editor has a reference to the `SetMaterialToMesh` script that is being edited, and it has a method called `OnInspectorGUI` which is called when the Inspector GUI is rendered.

The `OnInspectorGUI` method first calls the `OnInspectorGUI` method of the base class (`ActionCaller_Editor`) to render the elements of the `ActionCaller_Editor`'s Inspector GUI. It then checks if the `SetMaterialToMesh` script has an `OptionsManager` script assigned to it, and if it does, it displays a list of `renderers` that are currently assigned to the `SetMaterialToMesh` script. It also allows the user to add additional `renderers` to this list by providing an `Object Field` where the user can select a renderer. If the user adds a `renderer` to the list, the custom editor records an undo operation and updates the corresponding property of the `SetMaterialToMesh` script. If the user clicks the "X" button next to a renderer in the list, the custom editor removes that renderer from the list and records an undo operation.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SetMaterialToMesh), true)]
public class SetMaterialToMesh_Editor : ActionCaller_Editor
{
    SetMaterialToMesh myScript;
    protected override void OnEnable()
    {
        base.OnEnable();
        myScript = (SetMaterialToMesh)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (myScript.optionsManager != null)
        {
            bool hasRend = myScript.HasRenderer();
            if (hasRend)
            {

                for (int i = 0; i < myScript.targetRenderers.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.ObjectField("Renderer: " + (i + 1).ToString(), myScript.targetRenderers[i], typeof(Renderer), true);
                    if (GUILayout.Button("X", GUILayout.Width(20f)))
                    {
                        Undo.RecordObject(myScript, "Remove Renderer");
                        myScript.RemoveRendererAt(i);
                    }
                    EditorGUILayout.EndHorizontal();
                }

            }
            string targetString = (hasRend ? "Additional Renderer: " : "Target Renderer: ");
            Renderer selRend = null;
            Renderer rend = (Renderer)EditorGUILayout.ObjectField(targetString, selRend, typeof(Renderer), true);
            if (rend != selRend)
            {
                Undo.RecordObject(myScript, "Add Renderer To Array");
                myScript.AddRenderer(rend);
            }
        }
    }
}

```
</details>     
        
---    
    
### SetObjectsVisibility_Editor.cs
##### Component name: ```SetObjectsVisibility_Editor```
<details>
<summary>Click here to see the details and code</summary>

This script is a custom editor for a class called `SetObjectsVisibility`. A custom editor is a script that allows you to customize the Inspector GUI for a particular script or component in Unity.

The custom editor has several fields:

`myScript`: a reference to the `SetObjectsVisibility` script that is being edited
`newParentName`: a `string` field that allows the user to enter a name for the new parent object
`setbonesSkinToVRM`: a `boolean` field that enables or disables the reparenting of skin bones to VRM
`blendShapes`: a list of `BlendShapePreset` objects
`blendShapeNames`: a list of `strings`

The `OnEnable` function gets a reference to the `myScript` field and initializes the `rootParentOnChosen` field by setting it to the `mainCharacterOptions` field of the `OptionsManager` script.

The `OnInspectorGUI` function is called when the Inspector GUI is rendered. It displays a `text` field that allows the user to enter a name for the new parent object, and it has a button that allows the user to enable or disable the reparenting of skin bones to VRM. It also has a button that allows the user to add a new `BlendShapePreset` object to the `blendShapes` list and a corresponding `string` to the `blendShapeNames` list. The function also displays a list of `BlendShapePreset` objects and their corresponding names and allows the user to edit or delete them. If the user makes any changes to these fields, the custom editor records an undo operation and updates the corresponding properties of the `SetObjectsVisibility` script.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VRM;

[CustomEditor(typeof(SetObjectsVisibility), true)]
public class SetObjectsVisibility_Editor : ActionCaller_Editor
{
    SetObjectsVisibility myScript;
    protected override void OnEnable()
    {
        base.OnEnable();
        myScript = (SetObjectsVisibility)target;
        if (myScript.optionsManager != null)
        {
            if (myScript.rootParentOnChosen == null)
                myScript.rootParentOnChosen = myScript.optionsManager.mainCharacterOptions;
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        // works only for manager
        if (myScript.optionsManager != null)
        {
            string newParentName = EditorGUILayout.TextField("Parent Name (optional): ", myScript.newParentName);
            if (newParentName != myScript.newParentName)
            {
                Undo.RecordObject(myScript, "Set New Parent Name");
                myScript.newParentName = newParentName;
            }
            //EditorGUILayout.ObjectField("Root On Parent", myScript.rootParentOnChosen,typeof(RandomGameObject),true);
        }
        // works for all
        if (!myScript.setbonesSkinToVRM)
        {
            if (GUILayout.Button("Enable reparent skin bones to vrm", GUILayout.Height(30f)))
                myScript.setbonesSkinToVRM = true;
        }
        else
        {
            if (GUILayout.Button("Disable reparent skin bones to vrm", GUILayout.Height(30f)))
                myScript.setbonesSkinToVRM = false;
        }

        if (GUILayout.Button("Add Blendshape Identifier", GUILayout.Height(30f))) myScript._AddBlendShape();
        if (myScript.blendShapes != null)
        {
            if (myScript.blendShapes.Count > 0)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
                EditorGUILayout.LabelField("= Non case sensitive =", style, GUILayout.Height(30f));
                EditorGUILayout.LabelField("*Leave empty text field to use shape name", style, GUILayout.Height(30f));
                EditorGUILayout.Space();

                for (int i = 0; i < myScript.blendShapes.Count; i++)
                {
                    BlendShapePreset shape = myScript.blendShapes[i];
                    string name = myScript.blendShapeNames[i];
                    EditorGUILayout.BeginHorizontal();
                    shape = (BlendShapePreset)EditorGUILayout.EnumPopup(shape);
                    name = EditorGUILayout.TextField(name);

                    if (name != myScript.blendShapeNames[i] || shape != myScript.blendShapes[i])
                    {
                        Undo.RecordObject(myScript, "Set blendshapes value");
                        myScript.blendShapeNames[i] = name;
                        myScript.blendShapes[i] = shape;
                    }

                    if (GUILayout.Button("X"))
                    {
                        Undo.RecordObject(myScript, "Remove Blendshape");
                        myScript._RemoveBlendshape(i);
                    }
                    EditorGUILayout.EndVertical();
                }
            }
        }
    }
}

```
</details>     
        
---    
    
### SetTextureToMaterial_Editor.cs
##### Component name: ```SetTextureToMaterial_Editor```
<details>
<summary>Click here to see the details and code</summary>

This script is a custom editor for a class called `SetTextureToMaterial`. A custom editor is a script that allows you to customize the Inspector GUI for a particular script or component in Unity.

The custom editor has several fields:

`myScript`: a reference to the `SetTextureToMaterial` script that is being edited

The OnEnable function gets a reference to the myScript field.

The `OnInspectorGUI` function is called when the Inspector GUI is rendered. It has several elements that allow the user to edit the properties of the `SetTextureToMaterial` script. It has a list of `objects` that are of type `Renderer`, and it allows the user to add or remove renderers from this list. It also has a field for adding additional renderers to the list. If the user makes any changes to these fields, the custom editor records an undo operation and updates the corresponding properties of the `SetTextureToMaterial` script.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SetTextureToMaterial),true)]
public class SetTextureToMaterial_Editor : ActionCaller_Editor
{
    SetTextureToMaterial myScript;
    protected override void OnEnable()
    {
        base.OnEnable();
        myScript = (SetTextureToMaterial)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (myScript.optionsManager != null)
        {
            bool hasRend = myScript.HasRenderer();
            if (hasRend)
            {
                for (int i =0; i < myScript.targetRenderers.Count; i++)
                {
                    if (myScript.targetRenderers[i] == null)
                        myScript.targetRenderers.RemoveAt(i);
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.ObjectField("Renderer: " + (i+1).ToString(), myScript.targetRenderers[i], typeof(Renderer), true);
                    if (GUILayout.Button("X", GUILayout.Width(20f))) {
                        Undo.RecordObject(myScript, "Remove Renderer");
                        myScript.RemoveRendererAt(i);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                
            }
            string targetString = (hasRend ? "Additional Renderer: ":"Target Renderer: ");
            Renderer selRend = null;
            Renderer rend = (Renderer)EditorGUILayout.ObjectField(targetString, selRend, typeof(Renderer), true);
            if (rend != selRend)
            {
                Undo.RecordObject(myScript, "Add Renderer To Array");
                myScript.AddRenderer(rend);
            }
        }
    }
}

```
</details>     
        
---    
    
### RandomGameObject_Editor.cs
##### Component name: ```RandomGameObject_Editor```
<details>
<summary>Click here to see the details and code</summary>

The `RandomGameObject_Editor` script is a custom editor for the `RandomGameObject` script. A custom editor allows you to customize the Inspector GUI for a particular script or component in Unity.

The `RandomGameObject` script is a script that allows you to select a random `GameObject` from a list of options in the Inspector GUI. The custom editor adds additional functionality to the Inspector GUI for the `RandomGameObject` script. It allows you to specify a parent name for each option in the list, and it also allows you to specify whether the option is readable.

The custom editor has several functions:

* `OnEnable`: This function gets a reference to the `RandomGameObject` script that is being edited.
* `ValidateListSize`: This function ensures that the lists for parent names and readability are the same size as the list of options.
* `PostOptions`: This function displays a button that allows you to make all options readable.
* `Titles`: This function displays buttons that allow you to enable or disable reparenting, and it also displays labels and text fields for the parent names and readability of each option in the list.
* `Options`: This function displays the options in the list and allows you to edit their parent names and readability.
* `ValidateReadWriteEnabled`: This function checks if each option in the list is readable and updates the isReadable list accordingly.
* `isOptionReadable`: This function checks if a given option is readable.
* `FixAllRenderers`: This function makes all options in the list readable.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomGameObject))]
public class RandomGameObject_Editor : RandomObject_Editor
{
    RandomGameObject goScript;
    protected override void OnEnable()
    {
        goScript = (RandomGameObject)target;
        base.OnEnable();
    }
    protected override void ValidateListSize()
    {
        base.ValidateListSize();
        if (goScript.objects != null)
        {
            if (goScript.parentName == null) goScript.parentName = new List<string>(goScript.objects.Count);
            if (goScript.isReadable == null) goScript.isReadable = new List<bool>(goScript.objects.Count);

            if (goScript.parentName.Count != goScript.objects.Count)
            {
                for (int i = goScript.parentName.Count; i < goScript.objects.Count; i++)
                {
                    goScript.parentName.Add("");
                }

            }
            if (goScript.isReadable.Count != goScript.objects.Count)
            {
                for (int i = goScript.isReadable.Count; i < goScript.objects.Count; i++)
                {
                    goScript.isReadable.Add(true);
                }

            }
        }
        ValidateReadWriteEnabled();
    }
    protected override void PostOptions()
    {
        if (!AllReadable())
        {
            EditorGUILayout.LabelField("*Imported Objects must have read write enable in order to export them for vrm.\n", BoomToolsGUIStyles.CustomLabel(true, true, true));
            if (GUILayout.Button("Make All Read Write Enabled", GUILayout.Height(30f)))
            {
                FixAllRenderers ();
            }
        }
    }
    private bool AllReadable()
    {
        foreach(bool bo in goScript.isReadable)
        {
            if (!bo)
                return false;
        }
        return true;
    }
    private void ValidateReadWriteEnabled()
    {
        if (goScript.objects != null)
        {
            for (int i = 0; i < goScript.objects.Count; i++)
            {
                goScript.isReadable[i] = isOptionReadable(goScript.objects[i] as GameObject);
            }
        }
    }
    private bool isOptionReadable(GameObject go)
    {
        
        SkinnedMeshRenderer[] skinRends = go.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        foreach (SkinnedMeshRenderer sr in skinRends)
        {
            if (sr != null)
            {
                if (!sr.sharedMesh.isReadable)
                {
                    return false;
                }
            }
        }
        MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>(true);
        foreach (MeshFilter mf in meshFilters)
        {
            if (mf != null)
            {
                if (!mf.sharedMesh.isReadable)
                {
                    return false;
                }
            }
        }
        return true;
    }
    protected override void Titles()
    {
        EditorGUILayout.LabelField("*Reparenting will set a new parent to the selected option, use it when an option needs to be parented to a specific bone (hand, feet, head, etc...)", BoomToolsGUIStyles.CustomLabel(true,false,true));
        if (!goScript.setNewParent) {
            if (GUILayout.Button("Enable reparenting", GUILayout.Height(30f)))
                goScript.setNewParent = true;
        }
        else
        {
            if (GUILayout.Button("Disable reparenting", GUILayout.Height(30f)))
                goScript.setNewParent = false;
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(goScript.objectName, GUILayout.MinWidth(50f));
        EditorGUILayout.LabelField("Read", GUILayout.Width(40f));
        EditorGUILayout.LabelField("Trait Name", GUILayout.Width(100f));
        if (goScript.setNewParent) EditorGUILayout.LabelField("Parent Name", GUILayout.Width(100f));
        EditorGUILayout.LabelField("Weight", GUILayout.Width(60f));
        EditorGUILayout.EndHorizontal();
    }

    protected override void OnAddObjectClick()
    {
        ValidateReadWriteEnabled ();
    }
    private void FixAllRenderers()
    {
        for (int i =0; i < goScript.objects.Count; i++)
        {
            if (!goScript.isReadable[i])
            {
                FixReadableRenderer(i);
            }
        }
    }
    private void FixReadableRenderer(int index)
    {
        GameObject go = goScript.objects[index] as GameObject;
        Renderer[] rends = go.GetComponentsInChildren<Renderer>(true);

        foreach (Renderer rend in rends)
        {
            string path = AssetDatabase.GetAssetPath(PrefabUtility.GetCorrespondingObjectFromOriginalSource(rend));

            ModelImporter importer = ModelImporter.GetAtPath(path) as ModelImporter;

            if (importer != null)
            {
                Debug.Log("fixing");
                importer.isReadable = true;
                AssetDatabase.ImportAsset(path);
            }
        }

        goScript.isReadable[index] = isOptionReadable(go);
    }
    protected override void Option(int index)
    {
        EditorGUILayout.ObjectField(goScript.objects[index], typeof(Object), true);

        if (editing)
        {
            if (goScript.isReadable[index])
            {
                EditorGUILayout.LabelField("Yes", BoomToolsGUIStyles.CustomColorLabel(true, false, false, Color.green), GUILayout.Width(40f));
            }
            else
            {
                if(GUILayout.Button("Fix", GUILayout.Width(40f))) FixReadableRenderer(index);
            }
            
            string trait = EditorGUILayout.TextField(goScript.nameTraits[index], GUILayout.Width(100f));
            if (trait != goScript.nameTraits[index])
            {
                Undo.RecordObject(goScript, "Set Trait Name Value");
                goScript.nameTraits[index] = trait;
            }
            if (goScript.setNewParent)
            {
                string parent = EditorGUILayout.TextField(goScript.parentName[index], GUILayout.Width(100f));
                if (parent != goScript.parentName[index])
                {
                    Undo.RecordObject(goScript, "Set Parent Name");
                    goScript.parentName[index] = parent;
                }
            }
            int weight = EditorGUILayout.IntField(goScript.weights[index], GUILayout.Width(40f));
            if (weight != goScript.weights[index])
            {
                Undo.RecordObject(goScript, "Set Weight Value");
                goScript.weights[index] = weight;
            }
            if (GUILayout.Button("x", GUILayout.Width(20f)))
            {
                Undo.RecordObject(goScript, "Remove Object");
                goScript.RemoveAtIndex(index);
            }
        }
        else
        {
            EditorGUILayout.LabelField(goScript.isReadable[index] ? "Yes":"No", goScript.isReadable[index] ? 
                BoomToolsGUIStyles.CustomColorLabel(true, false, false, Color.green):
                BoomToolsGUIStyles.CustomColorLabel(true, false, false, Color.red), GUILayout.Width(40f));
            EditorGUILayout.LabelField(goScript.nameTraits[index], GUILayout.Width(100f));
            if (goScript.setNewParent)EditorGUILayout.LabelField(goScript.parentName[index] == "" ? "  -  " : goScript.parentName[index], GUILayout.Width(100f));
            EditorGUILayout.LabelField(goScript.weights[index].ToString(), GUILayout.Width(40f));
            EditorGUILayout.LabelField("", GUILayout.Width(20f));

        }
    }
}

```
</details>     
        
---    
    
### RandomObject_Editor.cs
##### Component name: ```RandomObject_Editor```
<details>
<summary>Click here to see the details and code</summary>

This script is a custom editor for the `RandomObject` script. It provides a GUI interface in the Unity Inspector window for the user to edit the objects and weights in the `RandomObject` script, as well as to add or remove objects to the list. The script also provides filtering options for the user to filter the list by object name, object weight, or object type. The script also has a button that allows the user to clear the filters. The script also has a button to allow the user to add all selected objects of the specified type to the list, as well as a button to remove all objects from the list. Finally, the script allows the user to lock the Inspector window to prevent accidental changes while editing the list.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomObject), true)]
public class RandomObject_Editor : Editor
{
    RandomObject myScript;

    protected string filterByObject = "";
    protected string filterByName = "";
    protected int filterByWeight = -1;
    bool enabledFilters = false;
    int operation = 0;
    string[] operationOptions;
    public bool editing = false;
    public GUIStyle style;
    protected virtual void OnEnable()
    {
        myScript = (RandomObject)target;
        myScript.SetObjectName();
        ValidateListSize();
        operationOptions = new string[3];
        operationOptions[0] = "equals";
        operationOptions[1] = "smaller";
        operationOptions[2] = "greater";
    }

    
    protected virtual void ValidateListSize()
    {
        if (myScript.objects != null)
        {
            if (myScript.weights == null) myScript.weights = new List<int>(myScript.objects.Count);
            if (myScript.nameTraits == null) myScript.nameTraits = new List<string>(myScript.objects.Count);

            if (myScript.weights.Count != myScript.objects.Count)
            {
                for (int i = myScript.weights.Count; i < myScript.objects.Count; i++)
                {
                    myScript.weights.Add(1);
                }

            }
            if (myScript.nameTraits.Count != myScript.objects.Count)
            {
                for (int i = myScript.nameTraits.Count; i < myScript.objects.Count; i++)
                {
                    myScript.nameTraits.Add(myScript.objects[i].name);
                }
            }
        }
    }
    public override void OnInspectorGUI()
    {
        editing = ActiveEditorTracker.sharedTracker.isLocked;
        style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, wordWrap = true };
        GUIStyle styleCenteredYellow = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, wordWrap = true, normal = { textColor = Color.yellow }, hover = { textColor = Color.yellow } };

        if (!editing)
        {
            if (myScript.optionsManager != null)
            {
                if (GUILayout.Button("Back to main options", GUILayout.Height(30f)))
                {
                    Selection.activeObject = myScript.optionsManager.gameObject;
                    myScript.optionsManager.setupStage = 2;
                }
            }
            if (GUILayout.Button("Edit", GUILayout.Height(30f)))
            {
                ActiveEditorTracker.sharedTracker.isLocked = true;
            }
        }
        else
        {
            if (GUILayout.Button("Finish Editing", GUILayout.Height(30f)))
            {
                ActiveEditorTracker.sharedTracker.isLocked = false;
                Selection.activeGameObject = myScript.gameObject;
                EditorUtility.SetDirty(myScript);
            }
            GUILayout.Space(5f);
            GUILayout.Label("EDIT MODE\n\n" + myScript.gameObject.name + "\nSelect Objects of type " + myScript.objectName + " Then click add selected to add them to the list\n", styleCenteredYellow);
            
            if (GUILayout.Button("Add Selected " + myScript.objectName + "s: ", GUILayout.Height(30f)))
            {
                ClearFilters();
                Undo.RecordObject(myScript, "Add " + myScript.objectName + "s: ");
                // If there is no current objects list, crerate a new one
                if (myScript.objects == null)
                    myScript.ResetObjects();

                foreach (Object obj in Selection.objects)
                {
                    if (myScript.IsValidObjectType(obj))
                    {
                        if (!myScript.ObjectExists(obj))
                            myScript.AddObject(obj);
                        else
                            Debug.Log(myScript.objectName + " already added");
                    }
                    else
                    {
                        Debug.Log("not a " + myScript.objectName + ": " + obj.name);
                    }
                }
                EditorUtility.SetDirty(myScript);
                OnAddObjectClick();
            }
            if (GUILayout.Button("Remove All", GUILayout.Height(30f)))
            {
                Undo.RecordObject(myScript, "Remove " + myScript.objectName + "s: ");
                myScript.ResetObjects();
                EditorUtility.SetDirty(myScript);
            }

            // filter section
            if (enabledFilters)
            {
                if (GUILayout.Button("Disable filters", GUILayout.Height(30f)))
                {
                    enabledFilters = false;
                    ClearFilters();
                }
                GUILayout.Label("== filters: ==", style, GUILayout.Height(30f));
                EditorGUILayout.LabelField("Quickly search through your added options with keywords and weights (value below 0 in weight will ignore this field)\n", style);

                if (GUILayout.Button("Clear filters", GUILayout.Height(30f)))
                    ClearFilters();
                filterByObject = EditorGUILayout.TextField("Object: ", filterByObject);
                filterByName = EditorGUILayout.TextField("Trait: ", filterByName);
                EditorGUILayout.BeginHorizontal();
                filterByWeight = EditorGUILayout.IntField("Weight: ", filterByWeight);
                operation = GUILayout.SelectionGrid(operation, operationOptions, 3);
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                if (GUILayout.Button("Enable filters", GUILayout.Height(30f)))
                {
                    enabledFilters = true;
                    GUI.FocusControl(null);
                }
            }
        }
       

        if (myScript.objects?.Count > 0)
        {
            DisplayAllOptions();
        }
        else
        {
            EditorGUILayout.LabelField("*No options. Click Edit button to start adding options", style);
        }
    }
    protected virtual void OnAddObjectClick()
    {
        /*override*/
    }
    protected virtual void DisplayAllOptions()
    {
        GUILayout.Space(5f);
        GUILayout.Label("== " + myScript.objectName + "s ==", style);

        if (editing)
        {
            GUILayout.Label("\n*Trait name: final name of option exported in the json trait\n*Weight: How probably is for this option to get chosen from other options\n", style);
        }

        Titles();

        for (int i = 0; i < myScript.objects.Count; i++)
        {
            if (myScript.objects[i] == null)
                myScript.RemoveAtIndex(i);
            EditorGUILayout.BeginHorizontal();

            if (filterByObject == "" && filterByName != "" && filterByWeight < 0)
            {
                Option(i);
            }
            else
            {
                if (FilterValue(i))
                {
                    Option(i);
                }
            }


            EditorGUILayout.EndHorizontal();
        }
        GUILayout.Space(5f);
        PostOptions();
    }
    protected virtual void PostOptions()
    {
        /* override for buttons */
    }

    protected virtual void Titles()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(myScript.objectName, GUILayout.MinWidth(50f));
        EditorGUILayout.LabelField("Trait Name", GUILayout.Width(200f));
        EditorGUILayout.LabelField("Weight", GUILayout.Width(60f));
        EditorGUILayout.EndHorizontal();
    }
    protected virtual void Option(int index)
    {
        EditorGUILayout.ObjectField(myScript.objects[index], typeof(Object), true);

        if (editing)
        {
            string trait = EditorGUILayout.TextField(myScript.nameTraits[index], GUILayout.Width(160f));
            if (trait != myScript.nameTraits[index])
            {
                Undo.RecordObject(myScript, "Set Trait Name Value");
                myScript.nameTraits[index] = trait;
            }
            int weight = EditorGUILayout.IntField(myScript.weights[index], GUILayout.Width(40f));
            if (weight != myScript.weights[index])
            {
                Undo.RecordObject(myScript, "Set Weight Value");
                myScript.weights[index] = weight;
            }
            if (GUILayout.Button("x", GUILayout.Width(20f)))
            {
                Undo.RecordObject(myScript, "Remove Object");
                myScript.RemoveAtIndex(index);
            }
        }
        else
        {
            EditorGUILayout.LabelField(myScript.nameTraits[index], GUILayout.Width(200f));
            EditorGUILayout.LabelField(myScript.weights[index].ToString(), GUILayout.Width(40f));
        }
    }

    private void ClearFilters()
    {
        operation = 0;
        filterByWeight = -1;
        filterByName = "";
        filterByObject = "";
        GUI.FocusControl(null);
    }

    protected bool FilterValue(int index)
    {

        if (filterByObject != "")
        {
            if (!myScript.objects[index].name.ToLower().Contains(filterByObject.ToLower()))
                return false;
        }
        if (filterByName != "")
        {
            if (!myScript.nameTraits[index].ToLower().Contains(filterByName.ToLower()))
                return false;
        }
        if (filterByWeight != -1)
        {
            switch (operation)
            {
                case 0: // equals
                    if (filterByWeight != myScript.weights[index])
                        return false;
                    break;
                case 1: // smaller
                    if (myScript.weights[index] >= filterByWeight)
                        return false;
                    break;
                case 2: // greater
                    if (myScript.weights[index] <= filterByWeight)
                        return false;
                    break;
                default:
                    Debug.Log("invalid operation");
                    break;
            }
        }

        return true;
    }


}

```
</details>     
        
---    
    
### RandomTexture_Editor.cs
##### Component name: ```RandomTexture_Editor```
<details>
<summary>Click here to see the details and code</summary>

This script is an editor script for a class called `RandomTexture`. It customizes the inspector view for the `RandomTexture` class.

The `RandomTexture` class is used for randomly selecting and applying a texture to an object. The editor script allows users to customize the options for the random texture selection, such as the list of available textures and their corresponding weights, as well as optional metallic and smoothness values. It also provides various buttons for enabling or disabling the metallic and smoothness options, adding selected textures to the list, and deleting textures from the list. The script also includes filtering options for searching through the list of textures by name, object, or weight.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomTexture))]
public class RandomTexture_Editor : RandomObject_Editor
{
    RandomTexture txrScript;
    protected override void OnEnable()
    {
        txrScript = (RandomTexture)target;
        base.OnEnable();
    }
    protected override void ValidateListSize()
    {
        base.ValidateListSize();

        if (txrScript.objects != null) {
            if (txrScript.metallicProperty == null) txrScript.metallicProperty = new List<float>(txrScript.objects.Count);
            if (txrScript.smoothnessProperty == null) txrScript.smoothnessProperty = new List<float>(txrScript.objects.Count);

            if (txrScript.metallicProperty.Count != txrScript.objects.Count)
            {
                for (int i = txrScript.metallicProperty.Count; i < txrScript.objects.Count; i++)
                {
                    txrScript.metallicProperty.Add(0f);
                }

            }
            if (txrScript.smoothnessProperty.Count != txrScript.objects.Count)
            {
                for (int i = txrScript.smoothnessProperty.Count; i < txrScript.objects.Count; i++)
                {
                    txrScript.smoothnessProperty.Add(0f);
                }
            }
        }
    }
    protected override void Titles()
    {
        EditorGUILayout.BeginHorizontal();
        if (!txrScript.setMetallic)
        {
            if (GUILayout.Button("Enable Metallic",GUILayout.Height(30f)))
                txrScript.setMetallic = true;
        }
        else
        {
            if (GUILayout.Button("Disable Metallic", GUILayout.Height(30f)))
                txrScript.setMetallic = false;
        }

        if (!txrScript.setSmoothness)
        {
            if (GUILayout.Button("Enable Smoothness", GUILayout.Height(30f)))
                txrScript.setSmoothness = true;
        }
        else
        {
            if (GUILayout.Button("Disable Smoothness", GUILayout.Height(30f)))
                txrScript.setSmoothness = false;
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(txrScript.objectName, GUILayout.MinWidth(50f));
        EditorGUILayout.LabelField("Trait Name", GUILayout.Width(140f));
        if (txrScript.setMetallic) EditorGUILayout.LabelField("Metal", GUILayout.Width(40f));
        if (txrScript.setSmoothness) EditorGUILayout.LabelField("Smooth", GUILayout.Width(40f));
        EditorGUILayout.LabelField("Weight", GUILayout.Width(40f));
        if (editing)EditorGUILayout.LabelField("", GUILayout.Width(20f));
        EditorGUILayout.EndHorizontal();
    }
    protected override void Option(int index)
    {
        EditorGUILayout.ObjectField(txrScript.objects[index], typeof(Object), true);

        if (editing)
        {
            string trait = EditorGUILayout.TextField(txrScript.nameTraits[index], GUILayout.Width(140f));
            if (trait != txrScript.nameTraits[index])
            {
                Undo.RecordObject(txrScript, "Set Trait Name Value");
                txrScript.nameTraits[index] = trait;
            }
            if (txrScript.setMetallic)
            {
                float metal = EditorGUILayout.FloatField(txrScript.metallicProperty[index], GUILayout.Width(40f));
                if (metal > 1f) metal = 1f;
                if (metal < 0f) metal = 0f;
                if (metal != txrScript.metallicProperty[index])
                {
                    Undo.RecordObject(txrScript, "Set Metal Value");
                    txrScript.metallicProperty[index] = metal;
                }
            }

            if (txrScript.setSmoothness)
            {
                float smooth = EditorGUILayout.FloatField(txrScript.smoothnessProperty[index], GUILayout.Width(40f));
                if (smooth > 1f) smooth = 1f;
                if (smooth < 0f) smooth = 0f;
                if (smooth != txrScript.smoothnessProperty[index])
                {
                    Undo.RecordObject(txrScript, "Set Smooth Value");
                    txrScript.smoothnessProperty[index] = smooth;
                }
            }

            int weight = EditorGUILayout.IntField(txrScript.weights[index], GUILayout.Width(40f));
            if (weight != txrScript.weights[index])
            {
                Undo.RecordObject(txrScript, "Set Weight Value");
                txrScript.weights[index] = weight;
            }
            if (GUILayout.Button("x", GUILayout.Width(20f)))
            {
                Undo.RecordObject(txrScript, "Remove Object");
                txrScript.RemoveAtIndex(index);
            }
        }
        else
        {
            EditorGUILayout.LabelField(txrScript.nameTraits[index], GUILayout.Width(140f));
            if (txrScript.setMetallic) EditorGUILayout.LabelField(txrScript.metallicProperty[index].ToString(), GUILayout.Width(40f));
            if (txrScript.setSmoothness) EditorGUILayout.LabelField(txrScript.smoothnessProperty[index].ToString(), GUILayout.Width(40f));
            EditorGUILayout.LabelField(txrScript.weights[index].ToString(), GUILayout.Width(40f));
        }
    }
}

```
</details>     
        
---    
    
### SuperRules_Editor.cs
##### Component name: ```SuperRules_Editor```
<details>
<summary>Click here to see the details and code</summary>

This script is an editor script for a script called `SuperRules`. It appears to provide a custom editor interface for `SuperRules` in the Unity editor, which allows users to specify rules to be used by a `SuperRules` script at runtime.

The script provides a number of fields and buttons in the inspector for the `SuperRules` script, which allow users to specify the target options that the `SuperRules` script should be aware of, the `RandomObject` that the `SuperRules` script should be listening to, and the action that the SuperRules script should take when certain conditions are met. The script also provides a way for users to specify the name of the trait for each target option.

The script also provides a number of utility methods, such as `ValidateListSize()` and `FetchEnums()`, which are used to ensure that certain lists and arrays used by the `SuperRules` script are properly initialized and populated.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(SuperRules),true)]
public class SuperRules_Editor : Editor
{
    SuperRules myScript;
    List<RandomObject> randomList;

    // for step1
    [SerializeField]
    int curRandomSelected;
    string[] allRandombjects;

    // for step2
    string[] randomObjectOptions;
    int curObject = 0;
    string placeholderSelectObject = "Select Object Option";

    // for step 3
    string[] allActionCaller;
    int curActionSelected = 0;
    string placeholderSelectAction = "Select Action To Disable";
    
    protected virtual void OnEnable()
    {
        myScript = (SuperRules)target;
        ValidateListSize();
        if (myScript.optionsManager != null)
        {
            FetchEnums(myScript.optionsManager);
            if (myScript.randomObject == myScript.optionsManager.mainCharacterOptions)
                curRandomSelected = -2;
            else
                curRandomSelected = GetCurrentSelectedRandom(myScript.randomObject);
            FetchObjectOptions(myScript.randomObject);
        }
    }
    protected virtual void ValidateListSize()
    {
        if (myScript.targetOptions != null)
        {
            if (myScript.optionsTraitName == null)
                myScript.optionsTraitName = new List<string>();

            if (myScript.optionsTraitName.Count != myScript.targetOptions.Count)
            {

                for (int i = myScript.optionsTraitName.Count; i < myScript.targetOptions.Count; i++)
                {
                    myScript.optionsTraitName.Add("");
                }

            }
        }
    }
    public override void OnInspectorGUI()
    {
        if (myScript.optionsManager != null)
        {
            if (GUILayout.Button("Back To Create Rules", GUILayout.Height(30f)))
                Selection.activeGameObject = myScript.optionsManager.gameObject;

            GUILayout.Space(5f);


            // MAKE SURE USER DID NOT SELECT OPTIONS FROM CURRENT RANDOM OBJECTS
            if (myScript.targetOptions?.Count > 0)
                GUI.enabled = false;
            else
                GUILayout.Label("=Instructions= \n\n Select the options this rule be aware of.\n", BoomToolsGUIStyles.CustomLabel(true, false, true));


            if (myScript.randomObject != myScript.optionsManager.mainCharacterOptions)
            {
                if (GUILayout.Button("Choose Main Character", GUILayout.Height(30f)))
                {
                    Undo.RecordObject(myScript, "Change Random Object");
                    Undo.RecordObject(this, "Change Random Object");
                    myScript.randomObject = myScript.optionsManager.mainCharacterOptions;
                    FetchObjectOptions(myScript.randomObject);
                    curRandomSelected = -2;
                }
            }
            else
            {
                if (GUILayout.Button("Choose from Options", GUILayout.Height(30f)))
                {
                    Undo.RecordObject(myScript, "Change Random Object");
                    Undo.RecordObject(this, "Change Random Object");
                    myScript.randomObject = null;
                    FetchObjectOptions(myScript.randomObject);
                    curRandomSelected = -1;
                }
            }
            EditorGUILayout.Space(5f);
            if (myScript.randomObject != myScript.optionsManager.mainCharacterOptions)
            {
                int newSelected = EditorGUILayout.Popup("Target Action", curRandomSelected, allRandombjects);
                if (newSelected != curRandomSelected)
                {
                    RandomObject ro = myScript.optionsManager.randomObjects[newSelected].GetComponent<RandomObject>();
                    Undo.RecordObject(myScript, "Change Random Object");
                    Undo.RecordObject(this, "Change Random Object");
                    myScript.randomObject = ro;
                    FetchObjectOptions(ro);
                    curRandomSelected = newSelected;
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Target Action", GUILayout.Width(120f));
                EditorGUILayout.LabelField("Character", BoomToolsGUIStyles.CustomLabel(false,true,false));
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space(5f);


            GUI.enabled = true;

            if (myScript.targetOptions?.Count > 0)
                GUILayout.Label("(*Remove options below to edit this section again).", BoomToolsGUIStyles.CustomLabel(true, true, true));

            EditorGUILayout.Space(5f);

            // SECOND STEP - Select which options may be chosen from this random object
            if (myScript.randomObject != null)
            {
                EditorGUILayout.LabelField("Now select the options to watch\n", BoomToolsGUIStyles.CustomLabel(true,true,true));



                int newOption = EditorGUILayout.Popup("Add Objects: ", curObject, randomObjectOptions);
                EditorGUILayout.Space(5f);
                if (newOption != curObject)
                {
                    // make sure is (value -1), since we are using the first value as placeholder
                    Object obj = myScript.randomObject.objects[newOption - 1];
                    if (!ExistsInList(obj, ref myScript.targetOptions))
                    {
                        Undo.RecordObject(myScript, "Add Object Option");
                        myScript.AddOption(obj, myScript.randomObject.nameTraits[newOption - 1]);
                    }
                    else
                    {
                        Debug.LogWarning("Object already exists in list, skipping");
                    }
                    curActionSelected = 0;
                }

                if (myScript.targetOptions?.Count>0)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Trait Name",BoomToolsGUIStyles.CustomLabel(false, true, false), GUILayout.Width(120f));
                    EditorGUILayout.LabelField("Chosen Object",BoomToolsGUIStyles.CustomLabel(false, true, false));
                    EditorGUILayout.EndHorizontal();
                    for (int i = 0; i < myScript.targetOptions.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.ObjectField(myScript.optionsTraitName[i], myScript.targetOptions[i], typeof(Object), true);
                        if (GUILayout.Button("X", GUILayout.Width(20f)))
                        {
                            Undo.RecordObject(myScript, "Remove Object Option");
                            myScript.RemoveOption(i);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }


                // THIRD STEP - Indicate what will happen when this options are fullfilled
                EditorGUILayout.Space(15f);

                if (myScript.beChosen)
                {
                    if (GUILayout.Button("Switch to NOT chosen options", GUILayout.Height(30f)))
                        myScript.beChosen = false;
                }
                else
                {
                    if (GUILayout.Button("Switch to ARE chosen options", GUILayout.Height(30f)))
                        myScript.beChosen = true;
                }
                EditorGUILayout.LabelField("If any of the above options " + (myScript.beChosen ? "ARE" : "ARE NOT") + " chosen, the next actions will be DISABLED", BoomToolsGUIStyles.CustomLabel(true, false, true));
                EditorGUILayout.Space(15f);
                int newRandom = EditorGUILayout.Popup("Target Action: ", curActionSelected, allActionCaller);
                if (newRandom != curActionSelected)
                {
                    // make sure is (value -1), since we are using the first value as placeholder
                    ActionCaller ac = myScript.optionsManager.actionCallers[newRandom - 1].GetComponent<ActionCaller>();
                    if (!ExistsInList(ac, ref myScript.disableActions))
                    {
                        Undo.RecordObject(myScript, "Add Action Caller");
                        myScript.AddAction(ref myScript.disableActions, ac);
                    }
                    else
                    {
                        Debug.LogWarning("Action already exsits in list, skipping");
                    }
                    curActionSelected = 0;
                }



                if (myScript.disableActions?.Count > 0)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Action Name", BoomToolsGUIStyles.CustomLabel(false, true, false), GUILayout.Width(120f));
                    EditorGUILayout.LabelField("Chosen Action", BoomToolsGUIStyles.CustomLabel(false, true, false));
                    EditorGUILayout.EndHorizontal();
                    for (int i = 0; i < myScript.disableActions.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        //EditorGUILayout.LabelField(myScript.enableActions[i].gameObject.name);
                        EditorGUILayout.ObjectField(myScript.disableActions[i].gameObject.name, myScript.disableActions[i], typeof(Object), true);
                        if (GUILayout.Button("X", GUILayout.Width(20f)))
                        {
                            Undo.RecordObject(myScript, "Remove Enable Action Caller");
                            myScript.RemoveActionAt(ref myScript.disableActions, i);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }

            }

            //EditorGUILayout.LabelField("=======");
            //base.OnInspectorGUI();
        }

        else
        {

            base.OnInspectorGUI();
        }
    }
    private bool ExistsInList(Object targetAction, ref List<Object> objectList)
    {
        if (objectList == null)
            return false;
        foreach (Object ob in objectList)
        {
            if (targetAction == ob)
                return true;
        }
        return false;
    }
    private bool ExistsInList(ActionCaller targetAction, ref List<ActionCaller> actionList)
    {
        if (actionList == null)
            return false;
        foreach (ActionCaller ac in actionList)
        {
            if (targetAction == ac)
                return true;
        }
        return false;
    }
    private void FetchEnums(OptionsManager optionManager)
    {
        randomList = optionManager.GetRandomObjectOfType(typeof(RandomObject));

        allRandombjects = new string[myScript.optionsManager.randomObjects.Count];
        for (int i = 0; i < myScript.optionsManager.randomObjects.Count; i++)
        {
            allRandombjects[i] = myScript.optionsManager.randomObjects[i] == null ? "null" : myScript.optionsManager.randomObjects[i].name;
        }

        allActionCaller = new string[myScript.optionsManager.actionCallers.Count + 1];
        allActionCaller[0] = placeholderSelectAction;
        for (int i =0; i < myScript.optionsManager.actionCallers.Count; i++)
        {
            allActionCaller[i+1] = myScript.optionsManager.actionCallers[i] == null ? "null": myScript.optionsManager.actionCallers[i].name;
        }
    }
    private void FetchObjectOptions(RandomObject target)
    {
        if (target == null)
        {
            randomObjectOptions = new string[1];
        }
        else
        {
            randomObjectOptions = new string[target.nameTraits.Count + 1];
            randomObjectOptions[0] = placeholderSelectObject;
            for (int i = 0; i < target.nameTraits.Count; i++)
            {
                randomObjectOptions[i + 1] = target.nameTraits[i];
            }
        }

    }
    private int GetCurrentSelectedRandom(RandomObject curRandom)
    {
        if (curRandom == null)
            return -1;
        foreach (RandomObject rand in randomList)
        {
            if (curRandom == rand)
                return randomList.IndexOf(rand);
        }
        return -1;
    }
}

```
</details>     
        
---    
    
### ActionCaller.cs
##### Component name: ```ActionCaller```
<details>
<summary>Click here to see the details and code</summary>

This script defines a class called `ActionCaller`, which is a `MonoBehaviour` that provides some basic functionality for controlling an action in a game. It has fields for an `OptionsManager`, a `RandomObject` called `randomTarget`, and a `string` called `traitName`. It also has `Object` and `string` fields called `selectedObject` and `selectedTrait`, respectively, and a `bool` field called `enableAction`. 

The `ActionCaller` class has several methods, including `SetPreSetup`, `SetPostSetup`, `SetAction`, `DisableByRule`, `SetRandomTrait`, `PreAction`, `PostAction`, and `Action`. The `ActionCaller` class also has several methods that return information about the class or its fields, including `GetRandomObjectValidType`, `IsValidType`, `IsActiveTrait`, `IsValidTrait`, `GetExtraData`, and `GetJsonedObject`.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCaller : MonoBehaviour
{
    [HideInInspector]
    public OptionsManager optionsManager;

    public RandomObject randomTarget;
    public string traitName = "";

    [HideInInspector]
    public Object selectedObject;
    [HideInInspector]
    public string selectedTrait;

    public bool enableAction = true;

    public void SetPreSetup()
    {
        if (randomTarget != null)
            PreAction();
    }
    public void SetPostSetup()
    {
        if (randomTarget != null)
            PostAction();
    }
    public void SetAction()
    {
        if (randomTarget != null)
            Action();
    }
    public void DisableByRule()
    {
        enableAction = false;
    }
    public void SetRandomTrait()
    {
        enableAction = true;
        if (randomTarget != null)
        {
            selectedObject = randomTarget.GetRandomObject();
            selectedTrait = randomTarget.GetObjectTraitName();
        }
        else
        {
            Debug.LogWarning("No random target set in script SetObjectsVisibility in: " + gameObject.name);
        }
    }
    protected virtual void PreAction()
    {
        //override//
    }
    protected virtual void PostAction()
    {
        //override//
    }
    protected virtual void Action()
    {
        //override//
    }

    /// <summary>
    /// Returns the Type of option that can be assigned to this class.
    /// </summary>
    /// <returns></returns>
    public virtual System.Type GetRandomObjectValidType()
    {
        return typeof(RandomObject); //generic
    }
    public virtual bool IsValidType()
    {
        return true;
    }
    /// <summary>
    /// Does this trait is something that is displayed in the scene?
    /// </summary>
    /// <returns></returns>
    protected virtual bool IsActiveTrait()
    {
        return true;
    }
    /// <summary>
    /// Does this trait has the basic setup to work?
    /// </summary>
    /// <returns></returns>
    public virtual bool IsValidTrait()
    {
        if (randomTarget == null)
            return false;
        return true;
    }
    public List<Object> GetExtraData()
    {
        if (IsActiveTrait())
            return FetchExtraData();
        else
            return new List<Object>();
    }
    protected virtual List<Object> FetchExtraData()
    {
        return new List<Object>();
    }
    public string GetJsonedObject(bool addEndComma, int tabulation = 0)
    {
        string tab = "";
        for (int i = 0; i < tabulation; i++)
            tab += "\t";

        if (IsActiveTrait())
        {
            return
                tab + "{\n" +
                tab + "\t\"trait_type\":" + "\"" + traitName + "\",\n" +
                tab + "\t\"value\":" + "\"" + selectedTrait + "\"\n" +
                tab + "}" + (addEndComma ? ",\n":"");
        }
        return "";

    }
}


```
</details>     
        
---    
    
### SetMaterialToMesh.cs
##### Component name: ```SetMaterialToMesh``` 
<details>
<summary>Click here to see the details and code</summary>

This script is for changing the material of a MeshRenderer. It is a derived class of `ActionCaller`, which is a base class for actions that can be called by the `OptionsManager`.

The `SetMaterialToMesh` class has a list of `MeshRenderer`s that it can change the material of, and it has an `Action` function that will change the material of all of the MeshRenderers in the list to the material specified in the `selectedObject` field of the `ActionCaller` class. The `ActionCaller` class has a field called `randomTarget`, which is a reference to a `RandomObject`, and the `selectedObject` field is a reference to one of the objects in the `RandomObject`'s list of options. The `SetMaterialToMesh` class also has several overridden functions from the `ActionCaller` class that are used for checking if the action is valid, such as `IsValidType`, `IsValidTrait`, and `IsActiveTrait`.

The `SetMaterialToMesh` class also has several helper functions for managing the list of MeshRenderers, such as `AddRenderer`, `RemoveRendererAt`, and `HasRenderer`.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterialToMesh : ActionCaller
{
    public List<Renderer> targetRenderers;
    protected override void Action()
    {
        if (randomTarget != null)
            ChangeRendererMaterial(selectedObject as Material);
        else
            Debug.LogWarning("No random target set in script SetTextureToMaterial in: " + gameObject.name);
    }

    private void ChangeRendererMaterial(Material material)
    {
        foreach (Renderer mr in targetRenderers)
            mr.sharedMaterial = material;
    }

    public override System.Type GetRandomObjectValidType()
    {
        return typeof(RandomMaterial);
    }

    public override bool IsValidType()
    {
        if (randomTarget.GetType() != typeof(RandomMaterial))
            return false;
        return base.IsValidType();
    }

    protected override bool IsActiveTrait()
    {
        for (int i = 0; i < targetRenderers?.Count; i++)
        {
            if (targetRenderers[i].gameObject.activeInHierarchy)
                return true;
        }
        return false;
    }

    public override bool IsValidTrait()
    {
        if (targetRenderers == null)
            return false;

        bool valid = false;
        for (int i = 0; i < targetRenderers.Count; i++)
        {
            if (targetRenderers[i] != null)
            {
                valid = true;
                break;
            }
        }
        if (!valid)
            return false;

        return base.IsValidTrait();
    }

    public bool HasRenderer()
    {
        if (targetRenderers != null)
        {
            for (int i = 0; i < targetRenderers.Count; i++)
            {
                if (targetRenderers[i] != null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void AddRenderer(Renderer rend)
    {
        if (rend == null)
            return;
        if (targetRenderers == null)
        {
            targetRenderers = new List<Renderer>();
        }
        foreach (Renderer r in targetRenderers)
        {
            if (r == rend)
            {
                Debug.LogWarning("Already added: " + rend.gameObject.name);
                return;
            }
        }

        targetRenderers.Add(rend);
    }
    public void RemoveRendererAt(int index)
    {
        if (targetRenderers != null)
        {
            targetRenderers.RemoveAt(index);
        }
    }
}

```
</details>    
        
---    
    
### SetObjectsVisibility.cs
##### Component name: ```SetObjectsVisibility```
<details>
<summary>Click here to see the details and code</summary>

This script is for controlling the visibility of a specific object in a scene. It has various features such as the ability to change the parent of the object, set the bones of a humanoid avatar to the object, and set blend shapes on the object. 

It has three methods that are called at different times during the script's execution: 

* `PreAction()`
* `Action()`
* `PostAction()`

* `PreAction()` is called before `Action()` and resets the positions of certain object's parents. 
* `Action()` is called after `PreAction()` and displays or hides the selected object based on the value of the enableAction variable. 
* `PostAction()` is called after `Action()` and handles reparenting the object and setting bones and blend shapes on the object.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;

public class SetObjectsVisibility : ActionCaller
{
    public bool setbonesSkinToVRM = false;

    //new
    public RandomGameObject rootParentOnChosen;
    public string newParentName = "";

    private Transform lastObject = null;
    private Transform lastObjectParent = null;

    public List<Transform> lastChilds;
    public List<Transform> lastChildParents;

    [HideInInspector]
    public List<BlendShapePreset> blendShapes;
    [HideInInspector]
    public List<string> blendShapeNames;
    [HideInInspector]
    public GameObject chosenParent;
    [HideInInspector]
    public List<BlendShapeClip> clips;

    protected override void PreAction()
    {
        ResetParentsPositions();
    }
    protected override void Action()
    {
        if (selectedObject != null)
        {
            if (enableAction)
            {
                DisplayObject(selectedObject as GameObject);
                CreateShapes();
            }
            else
            {
                DisplayObject(null);
            }
        }
    }
    
    protected override void PostAction()
    {
        if (rootParentOnChosen != null)
        {
            GameObject parentObject = rootParentOnChosen.GetSelectedObject() as GameObject;
            GameObject selectedGameObject = selectedObject as GameObject;

            RandomGameObject randomGameObject = randomTarget as RandomGameObject;
            string parentName = randomGameObject.setNewParent ? randomGameObject.parentName[randomGameObject.currentSelected] : newParentName;

            GameObject parent = parentObject;
            if (parentName != "")
            {
                parent = GetObjectByName(parentObject, parentName);
                if (parent == null) parent = parentObject;
            }

            SaveParentPosition(selectedGameObject.transform);
            selectedGameObject.transform.parent = parent.transform;

            if (setbonesSkinToVRM)
            {
                ParentBonesToVRM(parentObject, selectedGameObject);
            }
        }
    }
    private void ParentBonesToVRM(GameObject humanoid, GameObject target)
    {
        Animator anim = humanoid.GetComponent<Animator>();
        if (anim == null) {
            Debug.LogWarning("No animator in target humanoid vrm");
            return; 
        }
        Avatar avatar = anim.avatar;
        if (avatar == null)
        {
            Debug.LogWarning("No avatar in animator in target humanoid vrm");
            return;
        }
        if (!avatar.isHuman || !avatar.isValid)
        {
            Debug.LogWarning("Error in humanoid setup in avatar");
            return;
        }

        // Create a new List that will store the modified parents
        lastChildParents = new List<Transform>();
        lastChilds = new List<Transform>();

        // Get the mapped bones from vrm
        List<GameObject> gameobjectBones = new List<GameObject>();
        Transform[] childs = humanoid.GetComponentsInChildren<Transform>();

        for (int i =0; i < avatar.humanDescription.human.Length; i++)
        {
            HumanBone bone = avatar.humanDescription.human[i];
            for (int j = 0; j < childs.Length; j++)
            {
                if (bone.boneName == childs[j].gameObject.name)
                {
                    gameobjectBones.Add(childs[j].gameObject);
                    //very important break, as it will ONLY take the first found bone, in case another object was reparented before, it will no longer pick the bones from that reparenting
                    break;
                }
            }
        }
        // Parent them to selected object
        Transform[] targetChilds = target.GetComponentsInChildren<Transform>();
        for (int i = 0; i < gameobjectBones.Count; i++)
        {
            Transform  boneParent = gameobjectBones[i].transform;
            for (int j = 0; j < targetChilds.Length; j++)
            {
                if (targetChilds[j].gameObject.name == boneParent.gameObject.name)
                {
                    lastChildParents.Add(targetChilds[j].parent);
                    lastChilds.Add(targetChilds[j]);

                    targetChilds[j].parent = boneParent.transform;
                    // set targetchilds as child of bone parent, but save its value to return it later
                }
            }
        }

    }
    private void DisplayObject(GameObject obj)
    {
        for (int i =0; i < randomTarget.objects.Count;i++)
        {
            GameObject go = randomTarget.objects[i] as GameObject;
            if (go != null)
                go.SetActive(false);
        }
        if (obj != null)
            obj.SetActive(true);
 
    }
    private GameObject GetObjectByName(GameObject root, string name)
    {
        if (root == null)
            return null;

        Transform[] children = root.GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            if (child.name == name)
            {
                return child.gameObject;
            }
        }
        return null;
    }

    private void ResetParentsPositions()
    {
        if (lastObject != null)
        {
            lastObject.SetParent(lastObjectParent);
            lastObject = null;
            lastObjectParent = null;
        }
        if (lastChilds != null)
        {
            for (int i =0; i < lastChilds.Count; i++)
            {
                lastChilds[i].SetParent(lastChildParents[i]);
            }
        }
    }
    private void SaveParentPosition(Transform target)
    {
        lastObject = target;
        lastObjectParent = target.parent;
        //also set the bones
    }

    //private void SetNewParent(GameObject obj)
    //{
        
    //    for (int i =0; i < SetOnNewParentIfActive.Length; i++)
    //    {
    //        if (SetOnNewParentIfActive[i] != null)
    //        {
    //            if (SetOnNewParentIfActive[i].activeInHierarchy)
    //            {
    //                SaveParentPosition(obj.transform);
    //                obj.transform.SetParent(SetOnNewParentIfActive[i].transform);
    //                chosenParent = SetOnNewParentIfActive[i];
    //                break;
    //            }
    //        }

    //    }
    //}
    public override System.Type GetRandomObjectValidType()
    {
        return typeof(RandomGameObject);
    }
    public override bool IsValidType()
    {
        if (randomTarget.GetType() != typeof(RandomGameObject))
            return false;
        return base.IsValidType();
    }
    protected override bool IsActiveTrait()
    {
        GameObject go = (GameObject)selectedObject;
        if (go.activeInHierarchy)
            return true;
        return false;
    }

    protected override List<Object> FetchExtraData()
    {
        List<Object> data = new List<Object>();
        for (int i =0; i < clips.Count;i++)
        {
            data.Add(clips[i] as Object);
        }
        return data;
    }

    // blendshapes section:

    public void CreateShapes()
    {
        if (blendShapes.Count > 0)
        {
            GameObject target = selectedObject as GameObject;
            SkinnedMeshRenderer meshRenderer = target.GetComponentInChildren<SkinnedMeshRenderer>();
            if (meshRenderer != null)
            {
                Mesh mesh = meshRenderer.sharedMesh;

                clips = new List<BlendShapeClip>();
                for (int i = 0; i < mesh.blendShapeCount; i++)
                {
                    int val = IndexCoincidence(mesh.GetBlendShapeName(i));
                    if (val != -1) clips.Add(CreateClip(meshRenderer, blendShapes[val], i, chosenParent));
                }
            }
        }

    }
    private void CreateLists()
    {
        if (blendShapes == null)
            blendShapes = new List<BlendShapePreset>();
        if (blendShapeNames == null)
            blendShapeNames = new List<string>();
    }
    
    
    private int IndexCoincidence(string name)
    {
        for (int i = 0; i < blendShapeNames.Count; i++)
        {
            string compare = (blendShapeNames[i] == "" ? blendShapes[i].ToString(): blendShapeNames[i]);
            if (name.ToLower() == compare.ToLower())
            {
                return i;
            }
        }
        return -1;
    }

    public void _AddBlendShape()
    {
        CreateLists();
        blendShapes.Add(new BlendShapePreset());
        blendShapeNames.Add("");
    }
    public void _RemoveBlendshape(int index)
    {
        blendShapes.RemoveAt(index);
        blendShapeNames.RemoveAt(index);
    }
    private BlendShapeClip CreateClip(SkinnedMeshRenderer targetRenderer, BlendShapePreset preset, int bindIndex, GameObject finalParent = null)
    {
        BlendShapeClip clip = new BlendShapeClip();
        clip.Preset = preset;
        clip.BlendShapeName = preset.ToString();
        Debug.Log(clip.BlendShapeName);

        BlendShapeBinding bind = new BlendShapeBinding();

        bind.Index = bindIndex;
        bind.RelativePath = GetGameObjectPath(finalParent);
        bind.RelativePath += (bind.RelativePath == "" ? targetRenderer.gameObject.name : "/" + targetRenderer.gameObject.name);
        bind.Weight = 100;

        clip.Values = new BlendShapeBinding[1];
        clip.Values[0] = bind;

        return clip;
    }
    private string GetGameObjectPath(GameObject obj)
    {
        if (obj == null)
            return "";
        if (obj.transform.parent == null)
            return "";

        string path = "/" + obj.name;
        while (obj.transform.parent.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = "/" + obj.name + path;
        }
        return path.Substring(1);
    }
}

```
</details>     
        
---    
    
### SetTextureToMaterial.cs
##### Component name: ```SetTextureToMaterial```
<details>
<summary>Click here to see the details and code</summary>

This script is a `MonoBehaviour` that modifies the main texture of one or more renderers in a scene. It does this by changing the `mainTexture` property of the material(s) assigned to the renderer(s). It is also capable of modifying the `_Metallic` and `_Glossiness` properties of the material(s) if desired. The texture to be applied is chosen randomly from a list of textures specified in an instance of the `RandomTexture` class. The script is designed to be easily extensible through the use of virtual functions, allowing subclasses to modify its behavior as needed.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTextureToMaterial : ActionCaller
{
    public List<Renderer> targetRenderers;
    
    protected override void Action()
    {
        if (randomTarget != null)
            ChangeMaterialTexture(selectedObject as Texture2D);
        else
            Debug.LogWarning("No random target set in script SetTextureToMaterial in: " + gameObject.name);
    }
    private void ChangeMaterialTexture(Texture2D texture)
    {
        RandomTexture randomTexture = randomTarget as RandomTexture;
        foreach (Renderer mr in targetRenderers)
        {
            if (mr != null)
            {
                mr.sharedMaterial.mainTexture = texture;
                if (randomTexture.setMetallic)
                    mr.sharedMaterial.SetFloat("_Metallic", randomTexture.metallicProperty[randomTexture.currentSelected]);
                if (randomTexture.setSmoothness)
                    mr.sharedMaterial.SetFloat("_Glossiness", randomTexture.smoothnessProperty[randomTexture.currentSelected]);
            }
        }
    }


    public override System.Type GetRandomObjectValidType()
    {
        return typeof(RandomTexture);
    }
    public override bool IsValidType()
    {
        if (randomTarget.GetType() != typeof(RandomTexture))
            return false;
        return base.IsValidType();
    }
    protected override bool IsActiveTrait()
    {
        for (int i =0; i < targetRenderers?.Count; i++)
        {
            if (targetRenderers[i] != null)
                if (targetRenderers[i].gameObject.activeInHierarchy)
                    return true;
        }
        return false;
    }
    public override bool IsValidTrait()
    {
        if (targetRenderers == null)
            return false;

        bool valid = false;
        for (int i = 0; i < targetRenderers.Count; i++)
        {
            if (targetRenderers[i] != null) {
                valid = true;
                break;
            }
        }
        if (!valid)
            return false;

        return base.IsValidTrait();
    }

    public bool HasRenderer()
    {
        if (targetRenderers != null)
        {
            for (int i =0; i < targetRenderers.Count; i++)
            {
                if (targetRenderers[i] != null)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void AddRenderer(Renderer rend)
    {
        if (rend == null)
            return;
        if (targetRenderers == null)
        {
            targetRenderers = new List<Renderer>();
        }
        foreach(Renderer r in targetRenderers)
        {
            if (r == rend)
            {
                Debug.LogWarning("Already added: " + rend.gameObject.name);
                return;
            }
        }

        targetRenderers.Add(rend);
    }
    public void RemoveRendererAt(int index)
    {
        if (targetRenderers != null)
        {
            targetRenderers.RemoveAt(index);
        }
    }
}

```
</details>     
        
---    
    
### RandomGameObject.cs
##### Component name: ```RandomGameObject```
<details>
<summary>Click here to see the details and code</summary>

This script represents a subclass of `RandomObject`, a class that manages a list of objects of a certain type, and allows selecting one of them at random. `RandomGameObject` is a specific implementation of RandomObject that only allows adding `GameObject` objects to its list, and adds two additional lists to track whether each object is readable and a parent name for each object. The `RandomGameObject` class also overrides a few methods of RandomObject to customize its behavior. When `RandomGameObject.HasCorrectSetup()` is called, it checks whether all the objects in the `isReadable` list are set to `true`, and returns `false` if any of them are not. The `RandomGameObject` class also provides methods for adding and removing objects from its list and updating the corresponding entries in the `parentName` and `isReadable` lists.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGameObject : RandomObject
{
    public bool setNewParent = false;
    public List<string> parentName;
    public List<bool> isReadable;
    public override void SetObjectName()
    {
        objectName = "GameObject";
    }
    public override bool HasCorrectSetup()
    {
        if (isReadable != null)
        {
            foreach(bool bo in isReadable)
            {
                if (!bo)
                    return false;
            }
        }

        return base.HasCorrectSetup();
    }

    public override bool IsValidObjectType(Object obj)
    {
        return obj.GetType() == typeof(GameObject);
    }
    public override void ResetObjects()
    {
        base.ResetObjects();
        parentName = new List<string>();
        isReadable = new List<bool>();
    }
    public override void RemoveAtIndex(int index)
    {
        base.RemoveAtIndex(index);
        parentName.RemoveAt(index);
        isReadable.RemoveAt(index);
    }
    public override void AddObject(Object value)
    {
        base.AddObject(value);
        parentName.Add("");
        isReadable.Add(true);
    }
}

```
</details>     
        
---    
    
### RandomMaterial.cs
##### Component name: ```RandomMaterial```
<details>
<summary>Click here to see the details and code</summary>

This script appears to define a class called `RandomMaterial`, which is a subclass of `RandomObject`.

It looks like the main purpose of this class is to represent a group of `Material` objects that can be randomly selected from and used in some way. The class defines a method called `IsValidObjectType`, which checks if a given object is of type `Material`. It also sets the value of the `objectName` field to `Material`.

It's not clear from this script alone how the `RandomMaterial` class is used in the overall program, but it seems to be intended as a way to manage a group of `Material` objects that can be randomly selected from.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterial : RandomObject
{
    public override void SetObjectName()
    {
        objectName = "Material";
    }

    public override bool IsValidObjectType(Object obj)
    {
        return obj.GetType() == typeof(Material);
    }
}
```
</details>     
        
---    
    
### RandomObject.cs
##### Component name: ```RandomObject```
<details>
<summary>Click here to see the details and code</summary>

This script is a base class for a type of script that randomly selects an object from a list of objects. It has several methods to manage the list of objects, such as adding and removing objects and getting a randomly selected object from the list. It also has a method to check if the object is of a valid type. The object name, weight, and name trait of the currently selected object can also be accessed using this script.

Here are the methods and fields for the RandomObject class:

Fields:

* `optionsManager`: A reference to an OptionsManager object.
* `objects`: A list of objects that can be selected at random.
* `weights`: A list of weights for each object in objects, which determines the probability that an object will be selected.
* `nameTraits`: A list of names for each object in objects.
* `currentSelected`: The index of the currently selected object in objects.
* `objectName`: The name of the type of object that this RandomObject instance is handling (e.g. "Material", "GameObject").

Methods:

* `HasCorrectSetup()`: Returns `true` if this `RandomObject` instance has been set up correctly, false otherwise.
* `GetRandomObject()`: Returns a random `object` from the list of `objects`.
* `GetObjectWeight()`: Returns the weight of the currently selected `object`.
* `GetObjectTraitName()`: Returns the `name` of the currently selected `object`.
* `GetSelectedObject()`: Returns the currently selected `object`.
* `GetObjectAt(int index)`: Returns the `object` at the specified index in objects.
* `IsValidObjectType(Object obj)`: Returns `true` if the specified `object` is of a valid type for this `RandomObject` instance, `false` otherwise.
* `SetObjectName()`: Sets the `objectName` field to the name of the type of object that this `RandomObject` instance is handling.
* `ResetObjects()`: Resets the list of objects, weights, and names.
* `AddObject(Object value)`: Adds the specified object to the list of objects, with a weight of 1 and a name equal to the object's name.
* `GetRandomValue()`: Returns a random integer between 0 and the number of objects in objects (inclusive).
* `RemoveAtIndex(int index)`: Removes the object, weight, and name at the specified index
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObject : MonoBehaviour
{
    public OptionsManager optionsManager;

    public List<Object> objects;
    public List<int> weights;
    public List<string> nameTraits;

    [HideInInspector]
    public int currentSelected = 0;

    public string objectName = "Object";

    public virtual bool HasCorrectSetup()
    {
        if (objects == null)
            return false;
        if (objects.Count == 0)
            return false;
        return true;
    }
    public Object GetRandomObject()
    {
        if (objects.Count == 0)
            return null;

        currentSelected = GetRandomValue();
        if (objects[currentSelected] == null)
        {
            objects.RemoveAt(currentSelected);
            return GetRandomObject();
        }
        return objects[currentSelected];
    }
    public int GetObjectWeight()
    {
        return weights[currentSelected];
    }
    public string GetObjectTraitName()
    {
        return nameTraits[currentSelected];
    }
    public Object GetSelectedObject()
    {
        return objects[currentSelected];
    }
    public Object GetObjectAt(int index)
    {
        return objects[index];
    }
    public virtual bool IsValidObjectType(Object obj)
    {
        return obj.GetType() == typeof(Object);
    }
    public virtual void SetObjectName()
    {
        objectName = "Object";
    }

    public virtual void ResetObjects()
    {
        objects = new List<Object>();
        weights = new List<int>();
        nameTraits = new List<string>();
    }
    public virtual void AddObject(Object value)
    {
        objects.Add(value);
        weights.Add(1);
        nameTraits.Add(value.name);
    }
    
    protected int GetRandomValue()
    {
        return Random.Range(0, objects.Count);
    }
    public virtual void RemoveAtIndex(int index)
    {
        weights.RemoveAt(index);
        nameTraits.RemoveAt(index);
        objects.RemoveAt(index);
        
    }
    public bool ObjectExists(Object obj)
    {
        foreach (Object o in objects)
        {
            if (o == obj)
            {
                return true;
            }
        }
        return false;
    }
}


```
</details>     
        
---    
    
### RandomTexture.cs
##### Component name: ```RandomTexture```
<details>
<summary>Click here to see the details and code</summary>

The `RandomTexture` script is a subclass of the `RandomObject` script. It has a list of smoothness properties, a list of metallic properties, and two boolean values that determine whether the smoothness and metallic properties are set. It has a method called `SetObjectName` which sets the `objectName` field to "Texture". It also has an override of the `IsValidObjectType` method which returns whether the object passed in is of type `Texture2D`. The `ResetObjects` method is overridden to also reset the smoothness and metallic property lists. The `AddObject` method is overridden to also add smoothness and metallic properties to their respective lists. The `RemoveAtIndex` method is overridden to also remove the smoothness and metallic properties at the specified index.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomTexture : RandomObject
{
    public List<float> smoothnessProperty;
    public List<float> metallicProperty;

    public bool setSmoothness = false;
    public bool setMetallic = false;

    public override void SetObjectName()
    {
        objectName = "Texture";
    }
    
    public override bool IsValidObjectType(Object obj)
    {
        return obj.GetType() == typeof(Texture2D);
    }
    public override void ResetObjects()
    {
        base.ResetObjects();
        smoothnessProperty = new List<float>();
        metallicProperty = new List<float>();
    }
    public override void AddObject(Object value)
    {
        base.AddObject(value);
        smoothnessProperty.Add(0);
        metallicProperty.Add(0);
    }

    public override void RemoveAtIndex(int index)
    {
        base.RemoveAtIndex(index);
        smoothnessProperty.RemoveAt(index);
        metallicProperty.RemoveAt(index);
    }

    
}




```
</details>     
        
---    
    
### SuperRules.cs
##### Component name: ```SuperRules```
<details>
<summary>Click here to see the details and code</summary>

Creates superrules so that you can create conditions so layers do not overlap by design, You can setup superrules so when a certain object is enabled, certain items will be disabled.
    
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SuperRule : MonoBehaviour
{
  
   public GameObject[] layerGroupReferences;
    public DNAManager dnaManagerRef;
    public GameObject ifThisObjectAppears;
    public GameObject[] excludeTheseObjects;
    private string whatObjectIsCurrentlyEnabled;
  
    public GameObject[] ifTheseObjectAppear;
    public GameObject includeThisObject;
  
    public GameObject[] ifTheseObjectAlllAppear;
    public GameObject excludeAllTheseObjects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     // check superrule 1 is true of false
    public bool CheckSuperRule1()
    {
        // init local variable to a default value
        bool superRule1 = false;
        // end super rule 1 check
        superRule1 = ifThisObjectAppears.activeSelf;
        return superRule1;
    }


    // chgeck superrule 2 is true of false
    public bool CheckSuperRule2()
    {
        // init local variable to a default value
        bool allAreTrue = false;
       
        // create an array of game objects to test if they are active
        GameObject[] superRule2ArrayObjects = new GameObject[ifTheseObjectAppear.Length];

        // create an array of booleans to store each objects active state
        bool[] superRule2BoolArray = new bool[ifTheseObjectAppear.Length];

        // loop through the objects and assgn their state to the corresponding bool index
        for(int i = 0; i < ifTheseObjectAppear.Length ; i++)
        {
            superRule2BoolArray[i] = ifTheseObjectAppear[i].activeSelf;
            
            // loop thru all bools and check to see if all objects are active
                if (!superRule2BoolArray[i])
                {
                    // if one is false then set superRule2 to false
                    return allAreTrue = false;
                }else{

                    // or else the superrule returns true
                    allAreTrue = true;
                }    
        }

        // return superrule
        return allAreTrue;
    }

    // A check to see what layers are active, it requires the users to pass in the index of the array of objects references, this is used in the superrule logic
    public bool IsLayerActive(int layer)
    {
        // init local variable to a default value
        bool isLayerActive = false;
        // end super rule 1 check
        isLayerActive = layerGroupReferences[layer].activeSelf;
        return isLayerActive; 
    }

}

```
</details>     
        
---    