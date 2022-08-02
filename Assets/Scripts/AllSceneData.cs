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
