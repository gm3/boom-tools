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
