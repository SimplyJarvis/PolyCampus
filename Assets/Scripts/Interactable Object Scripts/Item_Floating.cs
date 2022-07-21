using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Floating : MonoBehaviour
{
    [SerializeField]
    float amplitude = 0.2f;
    Vector3 startPos;
    [SerializeField]
    float speed = 1f;
    [SerializeField]
    bool isFloating = false;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        
        if (isFloating) StartCoroutine(MovePopUp());
    }

    void OnBecameVisible()
    {
        StopAllCoroutines();
        if (isFloating) StartCoroutine(MovePopUp());
    }

    void OnBecameInvisible()
    {
        StopAllCoroutines();
    }

    IEnumerator MovePopUp()
    {
        isFloating = true;
        float elapsedTime = Random.Range(0f, 1f);
        while (isFloating)
        {
            float y = amplitude * (Mathf.Sin((speed * elapsedTime)) + 1 );
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, startPos.y, transform.position.z);
        

    }
}
