 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DebugAvatar : MonoBehaviour
{
    private void OnEnable()
    {
        Animator anim = GetComponent<Animator>();
        if (anim!= null)
        {
            //Debug.Log(anim.avatar.isHuman);
            //Debug.Log(gameObject.name);
            Debug.Log(anim.avatar.humanDescription.human.Length);
            //Debug.Log(anim.avatar.humanDescription.human[0].boneName);
            //Debug.Log(anim.avatar.humanDescription.human[0].humanName);
            Debug.LogWarning("starts here");
            for (int i =0; i< anim.avatar.humanDescription.human.Length; i++)
            {
                HumanBone bone = anim.avatar.humanDescription.human[i]; 
                Debug.Log(bone.humanName + "_" + i.ToString());
            }
        }
    }
}
