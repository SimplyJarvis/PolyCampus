using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UIHelpers;

public class QuitUI : Singleton<QuitUI>
{
    [SerializeField] CanvasGroup cg;
    [SerializeField] float fadeSpeed;
    bool isQuitting = false;

    public void ToggleQuit()
    {
        isQuitting = !isQuitting;
        StopAllCoroutines();

        StartCoroutine(UI_Tools.FadeText(isQuitting ? 1f : 0f, cg, fadeSpeed));
        cg.blocksRaycasts = isQuitting ? true : false;

        AttachControls();
    }

    public void ConfirmQuit()
    {
        Application.Quit();
    }

    public void DismissQuit()
    {
        ToggleQuit();
    }

    public void HandleConfirmInput(InputAction.CallbackContext context)
    {
            if (context.started)
            {
                ConfirmQuit();
            }
    }

    public void HandleDismissInput(InputAction.CallbackContext context)
    {
            if (context.started)
            {
                DismissQuit();
            }
    }

    public bool QuitStatus()
    {
        return isQuitting;
    }

    void AttachControls()
    {
        if (isQuitting)
        {
            InputManager.onFloorChange.started += HandleConfirmInput;
            InputManager.onSecondary.started += HandleDismissInput;
        }
        else
        {
            InputManager.onFloorChange.started -= HandleConfirmInput;
            InputManager.onSecondary.started -= HandleDismissInput;
        }
        
    }

}
