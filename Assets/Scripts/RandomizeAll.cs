using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VRM;

public class RandomizeAll : MonoBehaviour
{
    public GameObject parentRandomTraitCaller;

    public List<WeightedValue> weightedValues;
    public PrintRandomValue[] randomScriptReferences;
    public BGColorRandomizer randomBGScriptReference;
    public BGColorRandomizer randomBodyTextureScriptReferences;
    public Button buttonReference;
    public DNAManager dnaManagerReference;

    public SetObjectsVisibility exportVRMFromRandomTrait;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = buttonReference.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        RamdomizeAll();

    }

    // Update is called once per frame
    void Update()
    {



    }

    void TaskOnClick()
    {
        //Debug.Log ("You have randomized All!"); 


        if (!dnaManagerReference.DNAList.Contains(dnaManagerReference.DNACode)) {
            RamdomizeAll();
        } else {
            RamdomizeAll();
        }


    }

    public void RamdomizeAll() {
        string newTraits = "";
        if (parentRandomTraitCaller != null)
            newTraits = RandomizeParentGetJsonData();
        randomScriptReferences[0].RandomCheck();
        randomScriptReferences[1].RandomCheck();
        randomScriptReferences[2].RandomCheck();
        randomScriptReferences[3].RandomCheck();
        randomScriptReferences[4].RandomCheck();
        randomScriptReferences[5].RandomCheck();
        randomScriptReferences[6].RandomCheck();
        randomBGScriptReference.RandomCheck();
        randomBodyTextureScriptReferences.RandomCheck();
        dnaManagerReference.ExportJsonToText(newTraits);
    }

    private string RandomizeParentGetJsonData()
    {
        ActionCaller[] traits = parentRandomTraitCaller.GetComponentsInChildren<ActionCaller>(true);
        string result = "";
        List<Object> extraData = new List<Object>();
        foreach (ActionCaller t in traits)
        {
            t.SetRandomTrait();
            result += t.GetJsonedObject(true, 1);
            extraData.AddRange(t.GetExtraData());
        }

        // fetch all blendshape clips data
        List<BlendShapeClip> shapeClips = new List<BlendShapeClip>();
        for (int i =0; i < extraData.Count; i++)
        {
            if (ObjectType(extraData[i], typeof(BlendShapeClip)))
            {
                shapeClips.Add(extraData[i] as BlendShapeClip);
            }
        }
        AttachBlendshapesToAvatar(shapeClips);

        //return json data
        return result.Substring(0, result.Length - 2) + "\n";
    }

    private bool ObjectType(Object obj, System.Type type)
    {
        if (obj.GetType() == type)
            return true;
        return false;
    }

    private void AttachBlendshapesToAvatar(List<BlendShapeClip>clips)
    {
        if (clips.Count > 0)
        {
            if (exportVRMFromRandomTrait != null)
            {
                BlendShapeAvatar avatar = new BlendShapeAvatar
                {
                    Clips = clips
                };
                GameObject root = exportVRMFromRandomTrait.selectedObject as GameObject;
                VRMBlendShapeProxy proxy = root.GetComponent<VRMBlendShapeProxy>();
                if (proxy == null)
                    proxy = root.AddComponent<VRMBlendShapeProxy>();
                proxy.BlendShapeAvatar = avatar;
            }
            else
            {
                Debug.LogError("Blend shapes found, but exportVRMFromRandomTrait as not set");
            }
        }
    }
}
