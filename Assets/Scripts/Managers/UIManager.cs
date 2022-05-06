using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }


    [SerializeField]
    TMP_Text popUpTitle;
    [SerializeField]
    TMP_Text popUpContent;
    [SerializeField]
    RectTransform popUpContainer;
    [SerializeField]
    AnimationCurve popUpCurve;
    [SerializeField]
    float popUpSpeed;
    [SerializeField]
    float popUpDisplayTime = 8f;
    [Range(0, 100f)]
    [SerializeField]
    float popUpHeightOffset = 0f;
    [SerializeField]
    RectTransform canvas;
    Vector3 hidePosition;

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
        hidePosition = popUpContainer.position;

        Debug.Log(hidePosition);
    }

    public void ActivateInfoPopUp(string title, string content)
    {
        popUpContent.SetText(content);
        popUpTitle.SetText(title);
        StopAllCoroutines();
        StartCoroutine(MovePopUp());
    }

  
    IEnumerator MovePopUp()
    {
        popUpContainer.position = new Vector3(popUpContainer.position.x, 0f, popUpContainer.position.z);
        float curveTime = 0;
        bool isMoving = true;
        
        while (isMoving)
        {
            yield return 0; //Wait one frame to allow OnGUI to redraw the text container with the new content
            float newHeight = ((popUpContainer.sizeDelta.y * popUpContainer.localScale.y) * canvas.localScale.y) + popUpHeightOffset;

            while (Mathf.Abs(popUpContainer.position.y - newHeight) > 0.1f)
            {
                curveTime += Time.deltaTime;
                popUpContainer.position = Vector3.Lerp(popUpContainer.position, new Vector3(popUpContainer.position.x, newHeight, popUpContainer.position.z), popUpCurve.Evaluate(curveTime) * popUpSpeed);
                yield return null;
            }

            yield return new WaitForSeconds(popUpDisplayTime);

            while (popUpContainer.position.y > 0.1f)
            {
                curveTime += Time.deltaTime;
                popUpContainer.position = Vector3.Lerp(popUpContainer.position, new Vector3(popUpContainer.position.x, 0, popUpContainer.position.z), popUpCurve.Evaluate(curveTime) * popUpSpeed);
                yield return null;
            }
            isMoving = false;
        }


    }




}
