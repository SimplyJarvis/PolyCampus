using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AboutPanel_UI : MonoBehaviour
{
    [SerializeField] CanvasGroup cg;
    [SerializeField] float fadeSpeed;
    void OnEnable()
    {
        InputManager.onSecondary.started += ToggleAbout;
        InputManager.onSecondary.canceled += ToggleAbout;
    }
    void OnDisable()
    {
        InputManager.onSecondary.started -= ToggleAbout;
        InputManager.onSecondary.canceled -= ToggleAbout;
    }

    void ToggleAbout(InputAction.CallbackContext context)
    {
        
        StopAllCoroutines();
        if (context.started)
        {
            StartCoroutine(FadeText(1f));
        }
        else if (context.canceled)
        {
            StartCoroutine(FadeText(0f));
        }
    }

    IEnumerator FadeText(float targetAlpha)
    {
        while (cg.alpha != targetAlpha)
        {
            cg.alpha = Mathf.Lerp(cg.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
            if (Mathf.Abs(cg.alpha - targetAlpha) < 0.02f) cg.alpha = targetAlpha;
            yield return null;
        }
        yield return new WaitForSeconds(2f);
    }
}
