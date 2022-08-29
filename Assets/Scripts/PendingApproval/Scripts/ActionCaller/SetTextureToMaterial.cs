using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTextureToMaterial : ActionCaller
{
    public Renderer[] targetRenderers;
    
    protected override void Action()
    {
        if (randomTarget != null)
            ChangeMaterialTexture(selectedObject as Texture2D);
        else
            Debug.LogWarning("No random target set in script SetTextureToMaterial in: " + gameObject.name);
    }
    private void ChangeMaterialTexture(Texture2D texture)
    {
        foreach (Renderer mr in targetRenderers)
            mr.sharedMaterial.mainTexture = texture;
    }
    
    public override bool IsValidType()
    {
        if (randomTarget.GetType() != typeof(RandomTexture))
            return false;
        return base.IsValidType();
    }
    protected override bool IsValidTrait()
    {
        for (int i =0; i < targetRenderers.Length; i++)
        {
            if (targetRenderers[i].gameObject.activeInHierarchy)
                return true;
        }
        return false;
    }
}
