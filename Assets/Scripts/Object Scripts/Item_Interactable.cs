using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item_Interactable : MonoBehaviour
{
    public event Action onTrigger;

    public void Triggered()
    {
        onTrigger?.Invoke();
    }
}
