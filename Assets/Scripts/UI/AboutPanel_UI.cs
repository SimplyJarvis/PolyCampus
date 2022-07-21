using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UIHelpers;

public class AboutPanel_UI : MonoBehaviour
{
    [SerializeField] CanvasGroup cg;
    [SerializeField] float fadeSpeed;
    void Awake()
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
            StartCoroutine(UI_Tools.FadeText(1f, cg, 2.5f));
        }
        else if (context.canceled)
        {
            StartCoroutine(UI_Tools.FadeText(0f, cg, 2.5f));
        }
    }

}
