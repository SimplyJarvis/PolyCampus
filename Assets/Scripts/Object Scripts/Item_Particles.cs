using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Item_Interactable))]
public class Item_Particles : MonoBehaviour
{
    
    ParticleSystem particles;

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

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
        particles.Play();
    }
}
