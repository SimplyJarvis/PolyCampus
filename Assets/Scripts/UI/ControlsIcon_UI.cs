using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class ControlsIcon_UI : MonoBehaviour
{
    public GameObject keyboardUI;
    public GameObject gamePadUI;

    void IconChange(Controller_Enum controller)
    {
        Debug.Log("Control Change to: " + controller);
        switch (controller)
        {
            case Controller_Enum.Xbox:
                keyboardUI.SetActive(false);
                gamePadUI.SetActive(true);
                break;
            case Controller_Enum.Keyboard:
                keyboardUI.SetActive(true);
                gamePadUI.SetActive(false);
                break;
            case Controller_Enum.Playstation:
                break;
            default:
                break;
        }
    }



    void OnEnable()
    {
        InputManager.onControlsChanged += IconChange;
    }

    void OnDisable()
    {
        InputManager.onControlsChanged -= IconChange;
    }
}
