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

