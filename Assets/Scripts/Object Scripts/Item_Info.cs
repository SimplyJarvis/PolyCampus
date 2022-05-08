using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Info : MonoBehaviour 
{

    public infoPoint itemInfo;
    [SerializeField]
    float spriteSize;
    [SerializeField]
    GameObject playerCamera;
    [SerializeField]
    Vector3 initRotation;
    [SerializeField]
    bool isUI = true;

    // Start is called before the first frame update
    void Start()
    {
        if (isUI)
        {
            transform.rotation = Quaternion.Euler(initRotation);
            transform.localScale = Vector3.zero;
        }

    }

    void OnBecameVisible()
    {
        if (isUI)
        {
            StartCoroutine(ScaleUp());
        }
        
    }

    void OnBecameInvisible()
    {
        if (isUI)
        {
            StartCoroutine(ScaleDown());
        }
        
    }

    public void Activate ()
     {
         UIManager.Instance.ActivateInfoPopUp(itemInfo.itemName, itemInfo.itemDesc);
         SoundManager.Instance.interactSound();
     }

    IEnumerator ScaleUp()
    {
        yield return new WaitForSeconds(0.5f);
        while (transform.localScale.x < spriteSize)
        {
            transform.localScale += Vector3.one * 0.05f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator ScaleDown()
    {
        yield return new WaitForSeconds(0.5f);
        while (transform.localScale.x > 0)
        {
            transform.localScale -= Vector3.one * 0.1f;
            if (transform.localScale.x < 0)
            {
                transform.localScale = Vector3.zero;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void SetInfoPoint(infoPoint data)
    {
        itemInfo = data;
        Debug.Log("Data Set");
    }
}
