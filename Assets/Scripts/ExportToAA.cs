using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class ExportToAA : MonoBehaviour
{
    public Button buttonReference;
    [TextArea(10,10)]
    public string webaJSONOutputPreview;
    public TextMeshProUGUI WebaJSONBody;
    private int exportNumber;

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
       
        

    }

}
