using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;



public class FIleUtilities : MonoBehaviour
{

    public string[] paths;
    public Button clearButtonReference;

    void Start(){
        Button clearbtn = clearButtonReference.GetComponent<Button>();
            clearbtn.onClick.AddListener(ClearClick); 
    }

    public static void EmptyDirectory(string[] paths)
{
    foreach (string path in paths)
    {
        DirectoryInfo directory = new DirectoryInfo(path);

        foreach (FileInfo file in directory.GetFiles())
        {
            file.Delete(); 
        }

        foreach (DirectoryInfo subDirectory in directory.GetDirectories())
        {
            subDirectory.Delete(true);
        }
    }
}

void ClearClick()
{
    EmptyDirectory(paths);
}

    
}


