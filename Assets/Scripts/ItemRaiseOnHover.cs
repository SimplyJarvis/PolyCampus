using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRaiseOnHover : MonoBehaviour
{
    [SerializeField]
    float raiseHeight = 0.2f;
    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        StopAllCoroutines();
        StartCoroutine(MovePopUp(startPos.y + raiseHeight));
    }

    void OnMouseExit()
    {
        StopAllCoroutines();
        StartCoroutine(MovePopUp(startPos.y));
    }

    IEnumerator MovePopUp(float newHeight)
    {
        while (Mathf.Abs(transform.position.y - newHeight) > 0.01f)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, newHeight, Time.deltaTime * 2f), transform.position.z);
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
    }
}
