using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Item_Interactable : MonoBehaviour, IPointerEnterHandler
{
    public event Action onTrigger;
    public event Action OnHover;
    public bool isHover;

    public void Triggered(Vector3 pos)
    {
        if (!isHover)
        {
            onTrigger?.Invoke();
            ClickSprite.Instance.Play(pos);
        }
    }

     public void OnPointerEnter(PointerEventData eventData)
     {
         if (isHover) OnHover?.Invoke();
     }

     public void OnControllerEnter()
     {
         if (isHover) OnHover?.Invoke();
     }
 
   
}
