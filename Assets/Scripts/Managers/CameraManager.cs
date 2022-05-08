using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraManager : MonoBehaviour
{

    [SerializeField] Transform[] floorCamPos = new Transform[2];
    [SerializeField][Range(1f, 5f)] float cameraSpeed = 1f;
    [SerializeField] float scrollScale = 1f;
    [SerializeField] Vector2 cameraBoundPos;
    float zoomLevel;
    float cameraOffset = 1f;
    Camera MainCamera;
    Transform activeFloor;
    Vector2 mouseStart;
    bool isMoving = false;
    Vector2 mouseValue;
    Vector3 CameraPos = Vector3.zero;
    float scrollVal;


    void Awake()
    {
        MainCamera = Camera.main;
        CameraPos = Camera.main.transform.position;
        activeFloor = floorCamPos[0];
        zoomLevel = MainCamera.orthographicSize;
    }



    void Update()
    {
        CameraPos = new Vector3(activeFloor.position.x + cameraOffset, Camera.main.transform.position.y, CameraPos.z);

        if (scrollVal != 0)
        {
            zoomLevel -= (Mathf.Clamp(scrollVal, -1, 1) * scrollScale);
            zoomLevel = Mathf.Clamp(zoomLevel, 2, 9);
            if (zoomLevel > 2 || scrollVal < 0f)
            {
                if (zoomLevel < 9 || scrollVal > 0f)
                    cameraSpeed += Mathf.Clamp(scrollVal, -1, 1) / 2;
            }

        }

    

        MainCamera.orthographicSize = Mathf.Lerp(MainCamera.orthographicSize, zoomLevel, Time.deltaTime * 3.5f);
        MoveCamera();
    }

    public void ToggleMove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isMoving = true;
            mouseStart = CursorController.Instance.MousePositionScreen;
        }
        if (context.performed || context.canceled)
        {
            isMoving = false;
            mouseValue = Vector2.zero;
        }
    }

    public void MouseHandler(InputAction.CallbackContext context)
    {
        if (isMoving)
        {
            mouseValue = (mouseStart - CursorController.Instance.MousePositionScreen);
        }
    }

    public void JoyStickHandler(InputAction.CallbackContext context)
    {
        mouseValue = context.ReadValue<Vector2>();
    }


    public void OnScroll(InputAction.CallbackContext context)
    { //Scroll Zoom
        scrollVal = context.ReadValue<Vector2>().y;
    }

    void MoveCamera()
    {
        //Camera Z Axis Movement
        if (CameraPos.z < cameraBoundPos.y && CameraPos.z > cameraBoundPos.x)
        {
            CameraPos.z -= (mouseValue.x - mouseValue.y) < -0.05f || (mouseValue.x - mouseValue.y) > 0.05f ? (mouseValue.x - mouseValue.y) / (cameraSpeed) : 0f;
        }
        else
        {
            CameraPos.z = Mathf.MoveTowards(CameraPos.z, (cameraBoundPos.x + cameraBoundPos.y) / 2, Time.deltaTime * 2f); // If outside boundries move back towards middle
        }

        //Camera X Axis Movement
        if (cameraOffset > -8f && cameraOffset < 15f)
        {
            cameraOffset += (mouseValue.y + mouseValue.x) < -0.05f || (mouseValue.y + mouseValue.x) > 0.05f ? (mouseValue.y + mouseValue.x) / (cameraSpeed) : 0f;
        }
        else
        {
            cameraOffset = Mathf.MoveTowards(cameraOffset, 0, Time.deltaTime * 2f); // If outside boundries move back towards middle
        }

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, CameraPos, Time.deltaTime * 3.5f);
    }

    void FloorChange(int num)
    {
        activeFloor = floorCamPos[num];
        cameraOffset = 0;
    }

    void OnEnable()
    {
        GameManager.OnFloorChanged += FloorChange;

        InputManager.onToggleCameraMove.started += ToggleMove;
        InputManager.onToggleCameraMove.performed += ToggleMove;
        InputManager.onZoom.performed += OnScroll;
        InputManager.onZoom.canceled += OnScroll;
        InputManager.onZoom.started += OnScroll;
        InputManager.onMouseMove.performed += MouseHandler;
        InputManager.onJSMouseMove.started += JoyStickHandler;
        InputManager.onJSMouseMove.canceled += JoyStickHandler;
        InputManager.onJSMouseMove.performed += JoyStickHandler;
    }

    void OnDisable()
    {
        GameManager.OnFloorChanged -= FloorChange;

        InputManager.onToggleCameraMove.started -= ToggleMove;
        InputManager.onToggleCameraMove.performed -= ToggleMove;
        InputManager.onZoom.performed -= OnScroll;
        InputManager.onZoom.canceled -= OnScroll;
        InputManager.onZoom.started -= OnScroll;
        InputManager.onMouseMove.performed -= MouseHandler;
        InputManager.onJSMouseMove.started -= JoyStickHandler;
        InputManager.onJSMouseMove.canceled -= JoyStickHandler;
        InputManager.onJSMouseMove.performed -= JoyStickHandler;
    }
}
