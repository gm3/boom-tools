using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
//using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.UI;
//using UnityEngine.JsonUtility;

public class CodeConstrutorGPT : MonoBehaviour
{
    //private Button button;
    private string[] files;
    private Dictionary<string, Dictionary<string, List<string>>> codebase;
    private int headingLevel = 2;

    private void Start()
    {
        // Get a reference to the button and add a listener for clicks
        //button = GetComponent<Button>();
        //button.onClick.AddListener(TaskOnClick);

        // Get a list of all .cs files in the Application.dataPath directory
        files = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);

        // Create a new directory in the Application.streamingAssetsPath directory
        Directory.CreateDirectory(Application.streamingAssetsPath + "/Code_Output/");

        TaskOnClick();
    }

    private void SearchAllScripts()
    {
    codebase = new Dictionary<string, Dictionary<string, List<string>>>();
    var classNameRegex = new Regex(@"^\s*(public|private|protected)\s+class\s+(\w+)");
    var methodRegex = new Regex(@"^\s*(public|private|protected)\s+(.*)\s+(\w+)\s*\(");
    var fieldRegex = new Regex(@"^\s*(public|private|protected)\s+([^\s]+)\s+(\w+);");

    Parallel.ForEach(files, file =>
    {
        var lines = File.ReadAllLines(file);
        for (int i = 0; i < lines.Length; i++)
        {
            var match = classNameRegex.Match(lines[i]);
            if (match.Success)
            {
                var className = match.Groups[2].Value;
                lock (codebase)
                {
                    if (!codebase.ContainsKey(className))
                    {
                        codebase.Add(className, new Dictionary<string, List<string>>());
                    }
                }
                for (int j = i + 1; j < lines.Length; j++)
                {   
                    var matchMethod = methodRegex.Match(lines[j]);
                    if (matchMethod.Success)
                    {
                        var methodName = matchMethod.Groups[3].Value;
                        var methodType = matchMethod.Groups[1].Value;
                        var methodReturnType = matchMethod.Groups[2].Value;
                        lock (codebase[className])
                        {
                            if (!codebase[className].ContainsKey(methodName))
                            {
                                codebase[className].Add(methodName, new List<string>());
                            }
                            codebase[className][methodName].Add(methodType + " " + methodReturnType);
                        }

                        // new RegEx for private and public fields
                        //var fieldRegex = new Regex(@"^\s*(public|private)\s+(.*)\s+(.+);");
                        var matchField = fieldRegex.Match(lines[j]);
                        if (matchField.Success)
                        {
                            var fieldName = matchField.Groups[3].Value;
                            var fieldType = matchField.Groups[1].Value;
                            var fieldReturnType = matchField.Groups[2].Value;
                            lock (codebase[className])
                            {
                                if (!codebase[className].ContainsKey(fieldName))
                                {
                                    codebase[className].Add(fieldName, new List<string>());
                                }
                                codebase[className][fieldName].Add(fieldType + " " + fieldReturnType);
                            }
                        }
                    }
                
                }
            }
        }
    });
    }
        
    

    
    private string GenerateMarkdown()
{
    var markdown = new StringBuilder();
    foreach (var className in codebase.Keys)
    {
        markdown.Append(new string('#', headingLevel) + " " + className + "\n");
        foreach (var fieldName in codebase[className].Keys)
        {
            var fieldType = codebase[className][fieldName][0];
            if (fieldType == "public" || fieldType == "private")
            {
                markdown.Append("- " + fieldType + " " + fieldName + ";\n");
            }
            else
            {
                markdown.Append("- " + string.Join(",", codebase[className][fieldName]) + " " + fieldName + "\n");
            }
        }
    }
    return markdown.ToString();
}

    private void CreateTextFile(string fileName, string content)
    {
        try
        {
            File.WriteAllText(Application.streamingAssetsPath + "/Code_Output/" + fileName, content);
        }
        catch (Exception e)
        {
            Debug.LogError("Error writing file: " + e.Message);
        }
    }

    private void LogCodebase()
    {
        foreach (var className in codebase.Keys)
        {
            Debug.Log("Class: " + className);
            foreach (var methodName in codebase[className].Keys)
            {
                Debug.Log("Method: " + methodName);
                foreach (var method in codebase[className][methodName])
                {
                    Debug.Log("Signature: " + method);
                }
            }
        }
    }

    private void TaskOnClick()
    {
        SearchAllScripts();
        LogCodebase();
        Debug.Log(codebase);
        string json = JsonUtility.ToJson(codebase);
        //Debug.Log(json);
        File.WriteAllText(Application.streamingAssetsPath + "/Code_Output/codebase.json", json);
        var markdown = GenerateMarkdown();
        CreateTextFile("codebase.md", markdown);
    }
}