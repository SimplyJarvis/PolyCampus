using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item_Interactable))]
[RequireComponent(typeof(AudioSource))]
public class Item_Sound : MonoBehaviour
{
    AudioSource aSource;
    [SerializeField] float playTime;

    private void Start()
    {
        aSource = GetComponent<AudioSource>();        
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
        StopAllCoroutines();
        StartCoroutine(PlaySound());
    }

    IEnumerator PlaySound()
    {
        aSource.Play();
        yield return new WaitForSeconds(playTime);
        aSource.Stop();
    }

}
