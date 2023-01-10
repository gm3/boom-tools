using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    public GameObject DropDownReference;
    public TMP_Text TextBox;

    void Start()
    {
        currentGameMode = GameMode.Random;
        modeValueTextReference.text = currentGameMode.ToString();


        var dropdown = DropDownReference.transform.GetComponent<TMP_Dropdown>();

        List<string> items = new List<string>();

        items.Add("Random");
        items.Add("Import");

        // fill dropdown with items
        foreach(var item in items){
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = item});
        }

        DropdownItemSelected(dropdown);

        dropdown.onValueChanged.AddListener(delegate {DropdownItemSelected(dropdown); } );
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

    void DropdownItemSelected(TMP_Dropdown dropdown)
    {
    int index = dropdown.value;
        TextBox.text = dropdown.options[index].text;

        if (dropdown.options[index].text == "Random")
        {
            currentGameMode = GameMode.Random;
        }
        else if (dropdown.options[index].text == "Import")
        {
            currentGameMode = GameMode.Import;
        }
    }
}
