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
