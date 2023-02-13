using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRM;
using UnityEngine.Animations;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Merge : MonoBehaviour
{

    public Button buttonReference;

    void Start()
    {
        Button btn = buttonReference.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
    }


    private void MergeMeshes()
{
    GameObject[] traitObjects = GameObject.FindGameObjectsWithTag("Traits");

    // Create a dictionary to store the mesh filters and renderers of objects that share a common parent
    Dictionary<Transform, List<MeshFilter>> parentMeshes = new Dictionary<Transform, List<MeshFilter>>();
    Dictionary<Transform, List<Material[]>> parentMaterials = new Dictionary<Transform, List<Material[]>>();

    foreach (GameObject traitObject in traitObjects)
    {
        MeshFilter meshFilter = traitObject.GetComponent<MeshFilter>();
        MeshRenderer renderer = traitObject.GetComponent<MeshRenderer>();

        // If the current object has a mesh filter and renderer
        if (meshFilter != null && renderer != null)
        {
            Transform parentTransform = traitObject.transform.parent;

            // If the parent of the current object is not already in the dictionary
            if (!parentMeshes.ContainsKey(parentTransform))
            {
                // Add a new entry to the dictionary for this parent
                parentMeshes[parentTransform] = new List<MeshFilter>();
                parentMaterials[parentTransform] = new List<Material[]>();
            }

            // Add the mesh filter and materials to the lists of mesh filters and materials for this parent
            parentMeshes[parentTransform].Add(meshFilter);
            parentMaterials[parentTransform].Add(renderer.sharedMaterials);
        }
    }

    // Iterate over all parents in the dictionary
    foreach (Transform parentTransform in parentMeshes.Keys)
    {
        List<MeshFilter> meshFilters = parentMeshes[parentTransform];
        HashSet<Material> uniqueMaterials = new HashSet<Material>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Count];

        // Iterate over all mesh filters for this parent
        for (int i = 0; i < meshFilters.Count; i++)
        {
            Material[] currentMaterials = meshFilters[i].GetComponent<MeshRenderer>().sharedMaterials;

                foreach (Material material in currentMaterials)
                {
                    uniqueMaterials.Add(material);
                }
                
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        // Create a new game object for this parent
                GameObject newObject = new GameObject(parentTransform.name + " (Combined)");
                newObject.transform.parent = parentTransform.parent;
                newObject.transform.localPosition = Vector3.zero;
                newObject.transform.localRotation = Quaternion.identity;
                newObject.transform.localScale = Vector3.one;
                newObject.AddComponent<MeshRenderer>();
                newObject.AddComponent<MeshFilter>();
                MeshFilter newMeshFilter = newObject.GetComponent<MeshFilter>();
                Mesh combinedMesh = new Mesh();
                combinedMesh.CombineMeshes(combine);
                newMeshFilter.mesh = combinedMesh;
            
            MeshRenderer newMeshRenderer = newObject.GetComponent<MeshRenderer>();


            for (int i = 0; i < meshFilters.Count; i++)
            {
                Material[] currentMaterials = meshFilters[i].GetComponent<MeshRenderer>().sharedMaterials;

                foreach (Material material in currentMaterials)
                {
                    uniqueMaterials.Add(material);
                }
            }


            newMeshRenderer.sharedMaterials = uniqueMaterials.ToArray();


            // Destroy the original "Trait" objects
            for (int i = 0; i < meshFilters.Count; i++)
            {
                Destroy(meshFilters[i].gameObject);
            }
        }
    }

    void TaskOnClick()
    {
        MergeMeshes();
        //MergeWeights();
        //CombineMeshes();
    }
}