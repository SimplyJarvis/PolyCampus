using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSprite : MonoBehaviour
{
    Animator animator;
    [SerializeField] GameObject dustPrefab;
    private static ClickSprite instance;
    public static ClickSprite Instance { get { return instance; } }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void OnEnable()
    {
        CursorController.OnClickHit += Dust;
        animator = GetComponent<Animator>();
    }

    void OnDisable()
    {
        CursorController.OnClickHit -= Dust;
    }


    public void Play(Vector3 pos)
    {
        transform.position = pos;
        transform.LookAt(Camera.main.transform.position);
        animator.SetTrigger("Click");
    }

    void Dust(Vector3 pos)
    {
        GameObject dust = Instantiate(dustPrefab, pos, Quaternion.identity);
        Destroy(dust, 1.5f);
    }

}
