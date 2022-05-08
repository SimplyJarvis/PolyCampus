using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSprite : MonoBehaviour
{
    Animator animator;
    [SerializeField] GameObject dustPrefab;

    void OnEnable()
    {
        CursorController.OnItemClicked += Play;
        CursorController.OnClickHit += Dust;
        animator = GetComponent<Animator>();
    }

    void OnDisable()
    {
        CursorController.OnItemClicked -= Play;
        CursorController.OnClickHit -= Dust;
    }


    void Play(Vector3 pos)
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
