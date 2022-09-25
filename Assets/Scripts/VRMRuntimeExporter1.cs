﻿using System.IO;
using UnityEngine;
using VRMShaders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using VRM.RuntimeExporterSample;
using VRM;


    public class VRMRuntimeExporter1 : MonoBehaviour
    {
        
        public DNAManager dnaManagerReference; 

        [SerializeField]
        public bool UseNormalize = true;

        public GameObject m_model;
    public SetObjectsVisibility exportVRMFromRandomTrait;
    public GameObject parentRandomTraitCaller;
        public Button buttonReference;

        public CameraCapture cameraCaptureReference;
    public ToTextFile toTxtFileRef;
        

        /* void OnGUI()
        {
            if (GUILayout.Button("Load"))
            {
                Load();
            }

            GUI.enabled = m_model != null;

            if (GUILayout.Button("Add custom blend shape"))
            {
                AddBlendShapeClip(m_model);
            }

            if (GUILayout.Button("Export"))
            {
                Export(m_model, UseNormalize, dnaManagerReference.genID.ToString());
            }
        } */

        void Start(){
          
    
        
            // Reference to Button
            Button btn = buttonReference.GetComponent<Button>();
		    btn.onClick.AddListener(TaskOnClick);
        }

    private string updateTraits()
    {
        string result = "";
        ActionCaller[] traits = parentRandomTraitCaller.GetComponentsInChildren<ActionCaller>(true);
        List<Object> extraData = new List<Object>();
        foreach (ActionCaller t in traits)
        {
            t.SetAction();
            result += t.GetJsonedObject(true, 1);
            extraData.AddRange(t.GetExtraData());
        }
        foreach (ActionCaller t in traits)
        {
            t.SetPostSetup();
        }


        // fetch all blendshape clips data
        List<BlendShapeClip> shapeClips = new List<BlendShapeClip>();
        for (int i = 0; i < extraData.Count; i++)
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

    private void AttachBlendshapesToAvatar(List<BlendShapeClip> clips)
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

    void TaskOnClick(){
        if (dnaManagerReference.optionsManager != null)
            dnaManagerReference.optionsManager.AttachDataToDNA(dnaManagerReference);
        Debug.Log ("You have exported a VRM file");

        if (exportVRMFromRandomTrait != null)
            m_model = exportVRMFromRandomTrait.selectedObject as GameObject;

        // export when click button
        string json = updateTraits();
        dnaManagerReference.ExportJsonToText(json);

        Export(m_model, true, dnaManagerReference.genID.ToString());
        cameraCaptureReference.Capture();
       
        toTxtFileRef.CreateTextFile();
                  
	}

        async void Load()
        {
#if UNITY_STANDALONE_WIN
            var path = FileDialogForWindows.FileDialog("open VRM", ".vrm");
#elif UNITY_EDITOR
            var path = UnityEditor.EditorUtility.OpenFilePanel("Open VRM", "", "vrm");
#else
        var path = Application.dataPath + "/StreamingAssets/VRM/default.vrm";
#endif
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var loaded = await VrmUtility.LoadAsync(path);
            loaded.ShowMeshes();
            loaded.EnableUpdateWhenOffscreen();

            if (m_model != null)
            {
                GameObject.Destroy(m_model.gameObject);
            }

            m_model = loaded.gameObject;
        }

        static void AddBlendShapeClip(GameObject go)
        {
            // get or create blendshape proxy
            var proxy = go.GetComponent<VRMBlendShapeProxy>();
            if (proxy == null)
            {
                proxy = go.AddComponent<VRMBlendShapeProxy>();
            }

            // get or create blendshapeavatar
            var avatar = proxy.BlendShapeAvatar;
            if (avatar == null)
            {
                avatar = ScriptableObject.CreateInstance<BlendShapeAvatar>();
                proxy.BlendShapeAvatar = avatar;
            }

            // add blendshape clip to avatar.Clips
            var clip = ScriptableObject.CreateInstance<BlendShapeClip>();
            var name = $"custom#{avatar.Clips.Count}";
            Debug.Log($"Add {name}");
            // unity asset name
            clip.name = name;
            // vrm export name
            clip.BlendShapeName = name;
            clip.Preset = BlendShapePreset.Unknown;

            clip.IsBinary = false;
            clip.Values = new BlendShapeBinding[]
            {
                new BlendShapeBinding
                {
                    RelativePath = "mesh/face", // target Renderer relative path from root 
                    Index = 0, // BlendShapeIndex in SkinnedMeshRenderer
                    Weight = 75f // BlendShape weight, range is [0-100]
                },
            };
            clip.MaterialValues = new MaterialValueBinding[]
            {
                new MaterialValueBinding
                {
                    MaterialName = "Alicia_body", // target_material_name
                    ValueName = "_Color", // target_material_property_name,
                    BaseValue = new Vector4(1, 1, 1, 1), // Target value when the Weight value of BlendShapeClip is 0
                    TargetValue = new Vector4(0, 0, 0, 1), // Target value when the Weight value of BlendShapeClip is 1
                },
            };
            avatar.Clips.Add(clip);

            // done
        }


        public void Export(GameObject model, bool useNormalize, string edition)
        {
            //#if UNITY_STANDALONE_WIN
#if false
        var path = FileDialogForWindows.SaveDialog("save VRM", Application.dataPath + "/export.vrm");
#else
            var path = Application.dataPath + "/StreamingAssets/VRM/export" + edition + ".vrm"; // i edited this line
#endif
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var bytes = useNormalize ? ExportCustom(model) : ExportSimple(model);

            File.WriteAllBytes(path, bytes);
            Debug.LogFormat("export to {0}", path);
        }

        static byte[] ExportSimple(GameObject model)
        {
            var vrm = VRMExporter.Export(new UniGLTF.GltfExportSettings(), model, new RuntimeTextureSerializer());
            var bytes = vrm.ToGlbBytes();
            return bytes;
        }

        static byte[] ExportCustom(GameObject exportRoot, bool forceTPose = false)
        {
            // normalize
            var target = VRMBoneNormalizer.Execute(exportRoot, forceTPose);

            try
            {
                return ExportSimple(target);
            }
            finally
            {
                // cleanup
                GameObject.Destroy(target);
            }
        }

        void OnExported(UniGLTF.glTF vrm)
        {
            Debug.LogFormat("exported");
        }
    }

