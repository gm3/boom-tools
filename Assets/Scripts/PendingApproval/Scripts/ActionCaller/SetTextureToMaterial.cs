using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTextureToMaterial : ActionCaller
{
    public List<Renderer> targetRenderers;
    
    protected override void Action()
    {
        if (randomTarget != null)
            ChangeMaterialTexture(selectedObject as Texture2D);
        else
            Debug.LogWarning("No random target set in script SetTextureToMaterial in: " + gameObject.name);
    }
    private void ChangeMaterialTexture(Texture2D texture)
    {
        RandomTexture randomTexture = randomTarget as RandomTexture;
        foreach (Renderer mr in targetRenderers)
        {
            if (mr != null)
            {
                mr.sharedMaterial.mainTexture = texture;
                if (randomTexture.setMetallic)
                    mr.sharedMaterial.SetFloat("_Metallic", randomTexture.metallicProperty[randomTexture.currentSelected]);
                if (randomTexture.setSmoothness)
                    mr.sharedMaterial.SetFloat("_Glossiness", randomTexture.smoothnessProperty[randomTexture.currentSelected]);
            }
        }
    }


    public override System.Type GetRandomObjectValidType()
    {
        return typeof(RandomTexture);
    }
    public override bool IsValidType()
    {
        if (randomTarget.GetType() != typeof(RandomTexture))
            return false;
        return base.IsValidType();
    }
    protected override bool IsActiveTrait()
    {
        for (int i =0; i < targetRenderers?.Count; i++)
        {
            if (targetRenderers[i] != null)
                if (targetRenderers[i].gameObject.activeInHierarchy)
                    return true;
        }
        return false;
    }
    public override bool IsValidTrait()
    {
        if (targetRenderers == null)
            return false;

        bool valid = false;
        for (int i = 0; i < targetRenderers.Count; i++)
        {
            if (targetRenderers[i] != null) {
                valid = true;
                break;
            }
        }
        if (!valid)
            return false;

        return base.IsValidTrait();
    }

    public bool HasRenderer()
    {
        if (targetRenderers != null)
        {
            for (int i =0; i < targetRenderers.Count; i++)
            {
                if (targetRenderers[i] != null)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void AddRenderer(Renderer rend)
    {
        if (rend == null)
            return;
        if (targetRenderers == null)
        {
            targetRenderers = new List<Renderer>();
        }
        foreach(Renderer r in targetRenderers)
        {
            if (r == rend)
            {
                Debug.LogWarning("Already added: " + rend.gameObject.name);
                return;
            }
        }

        targetRenderers.Add(rend);
    }
    public void RemoveRendererAt(int index)
    {
        if (targetRenderers != null)
        {
            targetRenderers.RemoveAt(index);
        }
    }
}
