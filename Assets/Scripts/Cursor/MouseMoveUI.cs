using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;


public class MouseMoveUI : MonoBehaviour
{
    Vector2 startPos;
    bool isActive;
    [SerializeField] Vector2[] linePoints = new Vector2[2];
    [SerializeField] GameObject linePanel;
    RectTransform rTransform;

    void Start()
    {
        rTransform = linePanel.GetComponent<RectTransform>();
    }

    void StartMouse(InputAction.CallbackContext context)
    {
        startPos = Camera.main.ScreenToViewportPoint(context.ReadValue<Vector2>());
    }

    void DrawLine(InputAction.CallbackContext context)
    {
        if (isActive)
        {
            linePoints[0] = startPos;
            linePoints[1] = Camera.main.ScreenToViewportPoint(context.ReadValue<Vector2>());

            rTransform.sizeDelta = new Vector2(rTransform.sizeDelta.x, Mathf.Abs(Vector2.Distance(linePoints[0], linePoints[1])));
            rTransform.position = new Vector3(startPos.x, startPos.y, rTransform.position.z);
        }

    }

    void ToggleCameraMove(InputAction.CallbackContext context)
    {
        isActive = !isActive;
        Debug.Log(isActive);
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
