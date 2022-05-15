using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item_Interactable))]
public class Room_Hover : MonoBehaviour
{
    [SerializeField] string Name;
    void OnEnable()
    {
        GetComponent<Item_Interactable>().OnHover += DisplayName;
    }

    void OnDisable()
    {
        GetComponent<Item_Interactable>().OnHover -= DisplayName;
    }

    void DisplayName()
    {
        UIManager.Instance.RoomNameDisplay(Name);
    }
}
