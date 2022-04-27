using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    private static CursorController instance;
    public static CursorController Instance {get {return instance;}}
    private Vector2 currentMousePos;
    public Vector2 MousePositionInWorld {get {return currentMousePos;}}
    private Vector2 screenMousePos;
    public Vector2 MousePositionScreen {get {return screenMousePos;}}
    private InputAction c_mousePos;
    private InputAction c_jsMouse;
    private InputAction c_activate;
    private bool isMouse = true;
    public bool getIsMouse {get {return isMouse;}}
    public RaycastHit MouseRayHit {get {return rayHit;}}

    //Raycast Stuff
    Ray ray;
    RaycastHit rayHit;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    private void MouseHandler(InputAction.CallbackContext context)
    {
        if (isMouse)
        {
            screenMousePos = Camera.main.ScreenToViewportPoint(context.ReadValue<Vector2>()) - new Vector3(0.5f, 0.5f, 0);
            MouseRayCast();
        }
        else
        {
            screenMousePos = Camera.main.ScreenToViewportPoint(new Vector3 (Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0));
        }
        
    }

    void Update()
    {
        if (!isMouse)
        {
            MouseRayCast(); //As the camera is smoothed, it can continue moving after player input, this allows it to keep up
        }
    }

    private void MouseRayCast()
    {
        if (isMouse)
        {
            ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        }
        else
        {
            ray = Camera.main.ScreenPointToRay(new Vector3 (Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0));
        }

        Physics.Raycast(ray, out rayHit);
    }

    private void ActivateObject(InputAction.CallbackContext context)
    {
        //Debug.Log("hello");
        if (rayHit.collider.GetComponent<PopUpItem>())
        {
            rayHit.collider.GetComponent<PopUpItem>().Activate();
        }
    }

    void ControlChange(PlayerInput input)
    {
        if (input.currentControlScheme == "GamePad")
        {
            isMouse = false;
        }
        else {
            isMouse = true;
        }
    }

     void OnEnable()
    {
        c_mousePos = InputManager.Instance.inputActionMap.FindAction("MousePos");
        c_jsMouse = InputManager.Instance.inputActionMap.FindAction("JS_Camera");
        c_activate = InputManager.Instance.inputActionMap.FindAction("ActivateObject");
        c_activate.performed += ActivateObject;
        c_jsMouse.performed += MouseHandler;
        c_jsMouse.started += MouseHandler;
        c_jsMouse.canceled += MouseHandler;
        c_mousePos.performed += MouseHandler;
        InputManager.Instance.ControlInput.onControlsChanged += ControlChange;
    }

    void OnDisable()
    {
        c_activate.performed -= ActivateObject;
        c_jsMouse.performed -= MouseHandler;
        c_jsMouse.canceled -= MouseHandler;
        c_jsMouse.started -= MouseHandler;
        c_mousePos.performed -= MouseHandler;
        InputManager.Instance.ControlInput.onControlsChanged -= ControlChange;
    }

}
