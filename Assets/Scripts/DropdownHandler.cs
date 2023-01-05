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
