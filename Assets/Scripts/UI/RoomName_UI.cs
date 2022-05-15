using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class RoomName_UI : MonoBehaviour
{
    TMP_Text RoomText;
    [SerializeField] float fadeSpeed;

    void Start()
    {
        RoomText = GetComponent<TMP_Text>();
    }

    public void Display(string name)
    {
        StopAllCoroutines();
        RoomText.text = name;
        StartCoroutine(FadeText(1f));
    }

    public string GetCurrentName()
    {
        return RoomText.text;
    }

    IEnumerator FadeText(float targetAlpha)
    {
        while (RoomText.alpha != targetAlpha)
        {
            RoomText.alpha = Mathf.Lerp(RoomText.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
            if (Mathf.Abs(RoomText.alpha - targetAlpha) < 0.02f) RoomText.alpha = targetAlpha;
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine(FadeText(0f));
    }
}
