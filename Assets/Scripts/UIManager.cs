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
    }

    public void ActivateInfoPopUp(string title, string content)
    {
        popUpContent.SetText(content);
        popUpTitle.SetText(title);
        float height = (popUpContainer.sizeDelta.y * popUpContainer.localScale.y) + popUpHeightOffset;
        Debug.Log(popUpContainer.sizeDelta);
        StartCoroutine(MovePopUp(height, false));
        StartCoroutine(MovePopUp(hidePosition.y, true));
    }

    IEnumerator MovePopUp(float newHight, bool isHiding)
    {
        if (isHiding) { yield return new WaitForSeconds(popUpDisplayTime); }
        else popUpContainer.position = hidePosition;
        float curveTime = 0;
        while (popUpContainer.position.y != newHight)
        {
            curveTime += Time.deltaTime;
            popUpContainer.position = Vector3.MoveTowards(popUpContainer.position, new Vector3(popUpContainer.position.x, newHight, popUpContainer.position.z), popUpCurve.Evaluate(curveTime) * popUpSpeed);

            yield return null;
        }

    }




}
