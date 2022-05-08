using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource interact;
    [SerializeField] AudioSource miss;

    private static SoundManager _instance;
    public static SoundManager Instance { get { return _instance; } }
    private AudioSource source;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

    }

    public void interactSound()
    {
        interact.Play();
    }

    public void missSound()
    {
        miss.Play();
    }
}
