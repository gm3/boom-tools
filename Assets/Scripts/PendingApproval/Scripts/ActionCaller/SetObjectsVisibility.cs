using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;

public class SetObjectsVisibility : ActionCaller
{
    public GameObject[] SetOnNewParentIfActive;
    private Transform lastObject = null;
    private Transform lastObjectParent = null;

    [HideInInspector]
    public List<BlendShapePreset> blendShapes;
    [HideInInspector]
    public List<string> blendShapeNames;
    [HideInInspector]
    public GameObject chosenParent;
    [HideInInspector]
    public List<BlendShapeClip> clips;

    protected override void Action()
    {
        if (selectedObject != null)
        {
            DisplayObject(selectedObject as GameObject);
            CreateShapes();
        }

    }
    private void DisplayObject(GameObject obj)
    {
        ResetParentPosition();
        foreach (GameObject go in randomTarget.objects)
        {
            go.SetActive(false);
        }
        obj.SetActive(true);
        if (SetOnNewParentIfActive.Length != 0)
            SetNewParent(obj);
 
    }
    private void ResetParentPosition()
    {
        if (lastObject != null)
        {
            lastObject.SetParent(lastObjectParent);
            lastObject = null;
            lastObjectParent = null;
        }
    }
    private void SaveParentPosition(Transform target)
    {
        lastObject = target;
        lastObjectParent = target.parent;
    }
    private void SetNewParent(GameObject obj)
    {
        
        for (int i =0; i < SetOnNewParentIfActive.Length; i++)
        {
            if (SetOnNewParentIfActive[i] != null)
            {
                if (SetOnNewParentIfActive[i].activeInHierarchy)
                {
                    SaveParentPosition(obj.transform);
                    obj.transform.SetParent(SetOnNewParentIfActive[i].transform);
                    chosenParent = SetOnNewParentIfActive[i];
                    break;
                }
            }

        }
    }
    public override bool IsValidType()
    {
        if (randomTarget.GetType() != typeof(RandomGameObject))
            return false;
        return base.IsValidType();
    }
    protected override bool IsValidTrait()
    {
        GameObject go = (GameObject)selectedObject;
        if (go.activeInHierarchy)
            return true;
        return false;
    }

    protected override List<Object> FetchExtraData()
    {
        List<Object> data = new List<Object>();
        for (int i =0; i < clips.Count;i++)
        {
            data.Add(clips[i] as Object);
        }
        return data;
    }

    // blendshapes section:

    public void CreateShapes()
    {
        if (blendShapes.Count > 0)
        {
            GameObject target = selectedObject as GameObject;
            SkinnedMeshRenderer meshRenderer = target.GetComponentInChildren<SkinnedMeshRenderer>();
            if (meshRenderer != null)
            {
                Mesh mesh = meshRenderer.sharedMesh;

                clips = new List<BlendShapeClip>();
                for (int i = 0; i < mesh.blendShapeCount; i++)
                {
                    int val = IndexCoincidence(mesh.GetBlendShapeName(i));
                    if (val != -1) clips.Add(CreateClip(meshRenderer, blendShapes[val], i, chosenParent));
                }
            }
        }

    }
    private void CreateLists()
    {
        if (blendShapes == null)
            blendShapes = new List<BlendShapePreset>();
        if (blendShapeNames == null)
            blendShapeNames = new List<string>();
    }
    
    
    private int IndexCoincidence(string name)
    {
        for (int i = 0; i < blendShapeNames.Count; i++)
        {
            string compare = (blendShapeNames[i] == "" ? blendShapes[i].ToString(): blendShapeNames[i]);
            if (name.ToLower() == compare.ToLower())
            {
                return i;
            }
        }
        return -1;
    }

    public void _AddBlendShape()
    {
        CreateLists();
        blendShapes.Add(new BlendShapePreset());
        blendShapeNames.Add("");
    }
    public void _RemoveBlendshape(int index)
    {
        blendShapes.RemoveAt(index);
        blendShapeNames.RemoveAt(index);
    }
    private BlendShapeClip CreateClip(SkinnedMeshRenderer targetRenderer, BlendShapePreset preset, int bindIndex, GameObject finalParent = null)
    {
        BlendShapeClip clip = new BlendShapeClip();
        clip.Preset = preset;
        clip.BlendShapeName = preset.ToString();
        Debug.Log(clip.BlendShapeName);

        BlendShapeBinding bind = new BlendShapeBinding();

        bind.Index = bindIndex;
        bind.RelativePath = GetGameObjectPath(finalParent);
        bind.RelativePath += (bind.RelativePath == "" ? targetRenderer.gameObject.name : "/" + targetRenderer.gameObject.name);
        bind.Weight = 100;

        clip.Values = new BlendShapeBinding[1];
        clip.Values[0] = bind;

        return clip;
    }
    private string GetGameObjectPath(GameObject obj)
    {
        if (obj == null)
            return "";
        if (obj.transform.parent == null)
            return "";

        string path = "/" + obj.name;
        while (obj.transform.parent.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = "/" + obj.name + path;
        }
        return path.Substring(1);
    }
}
