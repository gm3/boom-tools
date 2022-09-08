using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;

public class SetObjectsVisibility : ActionCaller
{
    public bool setbonesSkinToVRM = false;

    //new
    public RandomGameObject rootParentOnChosen;
    public string newParentName = "";

    private Transform lastObject = null;
    private Transform lastObjectParent = null;

    public List<Transform> lastChilds;
    public List<Transform> lastChildParents;

    [HideInInspector]
    public List<BlendShapePreset> blendShapes;
    [HideInInspector]
    public List<string> blendShapeNames;
    [HideInInspector]
    public GameObject chosenParent;
    [HideInInspector]
    public List<BlendShapeClip> clips;

    protected override void PreAction()
    {
        ResetParentsPositions();
    }
    protected override void Action()
    {
        if (selectedObject != null)
        {
            DisplayObject(selectedObject as GameObject);
            CreateShapes();
        }
    }
    
    protected override void PostAction()
    {
        if (rootParentOnChosen != null)
        {
            GameObject parentObject = rootParentOnChosen.GetSelectedObject() as GameObject;
            GameObject selectedGameObject = selectedObject as GameObject;

            RandomGameObject randomGameObject = randomTarget as RandomGameObject;
            string parentName = randomGameObject.setNewParent ? randomGameObject.parentName[randomGameObject.currentSelected] : newParentName;
            Debug.Log(parentName);
            if (parentName != "")
            {
                GameObject parent = GetObjectByName(parentObject, parentName);
                Debug.Log(parent);
                if (parent != null)
                {
                    
                    SaveParentPosition(selectedGameObject.transform);
                    selectedGameObject.transform.parent = parent.transform;
                }
            }
            if (setbonesSkinToVRM)
            {
                ParentBonesToVRM(parentObject, selectedGameObject);
            }
        }
    }
    private void ParentBonesToVRM(GameObject humanoid, GameObject target)
    {
        Animator anim = humanoid.GetComponent<Animator>();
        if (anim == null) {
            Debug.LogWarning("No animator in target humanoid vrm");
            return; 
        }
        Avatar avatar = anim.avatar;
        if (avatar == null)
        {
            Debug.LogWarning("No avatar in animator in target humanoid vrm");
            return;
        }
        if (!avatar.isHuman || !avatar.isValid)
        {
            Debug.LogWarning("Error in humanoid setup in avatar");
            return;
        }

        // Create a new List that will store the modified parents
        lastChildParents = new List<Transform>();
        lastChilds = new List<Transform>();

        // Get the mapped bones from vrm
        List<GameObject> gameobjectBones = new List<GameObject>();
        Transform[] childs = humanoid.GetComponentsInChildren<Transform>();

        for (int i =0; i < avatar.humanDescription.human.Length; i++)
        {
            HumanBone bone = avatar.humanDescription.human[i];
            for (int j = 0; j < childs.Length; j++)
            {
                if (bone.boneName == childs[j].gameObject.name)
                {
                    gameobjectBones.Add(childs[j].gameObject);
                    //very important break, as it will ONLY take the first found bone, in case another object was reparented before, it will no longer pick the bones from that reparenting
                    break;
                }
            }
        }
        // Parent them to selected object
        Transform[] targetChilds = target.GetComponentsInChildren<Transform>();
        for (int i = 0; i < gameobjectBones.Count; i++)
        {
            Transform  boneParent = gameobjectBones[i].transform;
            for (int j = 0; j < targetChilds.Length; j++)
            {
                if (targetChilds[j].gameObject.name == boneParent.gameObject.name)
                {
                    lastChildParents.Add(targetChilds[j].parent);
                    lastChilds.Add(targetChilds[j]);

                    targetChilds[j].parent = boneParent.transform;
                    // set targetchilds as child of bone parent, but save its value to return it later
                }
            }
        }

    }
    private void DisplayObject(GameObject obj)
    {
        for (int i =0; i < randomTarget.objects.Count;i++)
        {
            GameObject go = randomTarget.objects[i] as GameObject;
            if (go != null)
                go.SetActive(false);
        }
        obj.SetActive(true);
 
    }
    private GameObject GetObjectByName(GameObject root, string name)
    {
        if (root == null)
            return null;

        Transform[] children = root.GetComponentsInChildren<Transform>();
        Debug.Log(children.Length);
        foreach (var child in children)
        {
            Debug.Log(child.name);
            if (child.name == name)
            {
                return child.gameObject;
            }
        }
        return null;
    }

    private void ResetParentsPositions()
    {
        if (lastObject != null)
        {
            lastObject.SetParent(lastObjectParent);
            lastObject = null;
            lastObjectParent = null;
        }
        if (lastChilds != null)
        {
            Debug.Log(gameObject.name);
            //Debug.Log(lastChilds.Count);
            Debug.Log("CHECK HERE");
            for (int i =0; i < lastChilds.Count; i++)
            {
                Debug.Log("===");
                Debug.Log("old parent: " + lastChilds[i].parent.name);
                lastChilds[i].SetParent(lastChildParents[i]);
                Debug.Log("new parent: " + lastChilds[i].parent.name);
            }
            //lastChilds = null;
            //lastChildParents = null;
        }
    }
    private void SaveParentPosition(Transform target)
    {
        lastObject = target;
        lastObjectParent = target.parent;
        //also set the bones
    }

    //private void SetNewParent(GameObject obj)
    //{
        
    //    for (int i =0; i < SetOnNewParentIfActive.Length; i++)
    //    {
    //        if (SetOnNewParentIfActive[i] != null)
    //        {
    //            if (SetOnNewParentIfActive[i].activeInHierarchy)
    //            {
    //                SaveParentPosition(obj.transform);
    //                obj.transform.SetParent(SetOnNewParentIfActive[i].transform);
    //                chosenParent = SetOnNewParentIfActive[i];
    //                break;
    //            }
    //        }

    //    }
    //}
    public override System.Type GetRandomObjectValidType()
    {
        return typeof(RandomGameObject);
    }
    public override bool IsValidType()
    {
        if (randomTarget.GetType() != typeof(RandomGameObject))
            return false;
        return base.IsValidType();
    }
    protected override bool IsActiveTrait()
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
