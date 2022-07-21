using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Item_Interactable))]
public class Item_Animation : MonoBehaviour
{
    public Animator[] anim;
    void OnEnable()
    {
        GetComponent<Item_Interactable>().onTrigger += Activate;
    }

    void OnDisable()
    {
        GetComponent<Item_Interactable>().onTrigger -= Activate;
    }

    void Activate()
    {
        foreach (Animator item in anim)
        {
            item.SetTrigger("play");
        }
    }
}
