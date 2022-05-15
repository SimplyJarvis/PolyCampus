using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;


public class MouseMoveUI : MonoBehaviour
{

    public bool isActive;

    [SerializeField] DrawLineUI lineGraphic;

    

    void StartMouse(InputAction.CallbackContext context)
    {
        lineGraphic.SetStart(CursorController.Instance.MousePositionScreenRaw);
    }

    void DrawLine(InputAction.CallbackContext context)
    {
        if (isActive)
        {
            lineGraphic.setMousePos(CursorController.Instance.MousePositionScreenRaw);
        }

    }


    void ToggleCameraMove(InputAction.CallbackContext context)
    {
        isActive = !isActive;
        if (!isActive) lineGraphic.setMousePos(Vector2.zero);
        //Debug.Log(isActive);
    }

    void OnEnable()
    {
        InputManager.onMouseMove.performed += DrawLine;

        InputManager.onToggleCameraMove.started += StartMouse;
        InputManager.onToggleCameraMove.started += ToggleCameraMove;
        InputManager.onToggleCameraMove.performed += ToggleCameraMove;
    }

    void OnDisable()
    {
        InputManager.onMouseMove.performed -= DrawLine;

        InputManager.onToggleCameraMove.started -= StartMouse;
        InputManager.onToggleCameraMove.started -= ToggleCameraMove;
        InputManager.onToggleCameraMove.performed -= ToggleCameraMove;
    }
}
