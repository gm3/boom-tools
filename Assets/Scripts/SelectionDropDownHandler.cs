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
