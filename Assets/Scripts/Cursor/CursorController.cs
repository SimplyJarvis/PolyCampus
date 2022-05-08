using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    private static CursorController instance;
    public static CursorController Instance { get { return instance; } }
    private Vector2 screenMousePos;
    public Vector2 MousePositionScreen { get { return screenMousePos; } } // Used for Camera movement
    private bool isMouse = true;
    public RaycastHit MouseRayHit { get { return rayHit; } }
    public static event Action<Vector3> OnItemClicked; //Clicked on valid interactable object
    public static event Action<Vector3> OnClickHit; //Clicked on other objects
    private int interactableLayer;

    //Raycast Stuff
    Ray ray;
    RaycastHit rayHit;
    RaycastHit rayHitInteractable;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        interactableLayer = 1 << LayerMask.NameToLayer("InteractableObject"); //Create bitmask, This gets only objects on Layer 7 which is our interactable objects layer
        Debug.Log(LayerMask.NameToLayer("InteractableObject"));
    }

    private void MouseHandler(InputAction.CallbackContext context)
    {
        if (isMouse)
        {
            screenMousePos = Camera.main.ScreenToViewportPoint(context.ReadValue<Vector2>()) - new Vector3(0.5f, 0.5f, 0);
            MouseRayCast();
        }
    }

    void Update()
    {
        if (!isMouse)
        {
            MouseRayCast(); //As the camera is smoothed, it can continue moving after player input, this allows the sphere to continue moving with the camera after player input stops
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
            ray = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0));
        }
        if (InputManager.Instance.isSphereActive)
        {
            Physics.Raycast(ray, out rayHitInteractable, Mathf.Infinity, interactableLayer);
        }
        Physics.Raycast(ray, out rayHit);
        
    }

    private void ActivateObject(InputAction.CallbackContext context)
    {
        if (InputManager.Instance.isSphereActive)
        {
            if (rayHitInteractable.collider && rayHitInteractable.collider.GetComponent<Item_Info>()) //If clicked on an interactiable object
            {
                rayHitInteractable.collider.GetComponent<Item_Info>().Activate();
                OnItemClicked?.Invoke(rayHitInteractable.point);
            }
            else // If just clicked on something with a collider
            {
                OnClickHit?.Invoke(rayHit.point);
                SoundManager.Instance.missSound();
            }
        }
        else if (rayHit.collider)
        {
            if (rayHit.collider.GetComponent<Item_Info>()) //If clicked on an interactiable object
            {
                rayHit.collider.GetComponent<Item_Info>().Activate();
                OnItemClicked?.Invoke(rayHit.point);
            }
            else // If just clicked on something with a collider
            {
                OnClickHit?.Invoke(rayHit.point);
                SoundManager.Instance.missSound();
            }
        }
    }

    void HandleControlChange(PlayerInput input)
    {
        if (input.currentControlScheme == "GamePad")
        {
            isMouse = false;
            screenMousePos = Camera.main.ScreenToViewportPoint(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0)); //Center the "mouse pos"
        }
        else
        {
            isMouse = true;
        }
    }

    void OnEnable()
    {
        InputManager.onActivate.performed += ActivateObject;
        InputManager.onJSMouseMove.performed += MouseHandler;
        InputManager.onJSMouseMove.started += MouseHandler;
        InputManager.onJSMouseMove.canceled += MouseHandler;
        InputManager.onMouseMove.performed += MouseHandler;
        InputManager.onControlsChanged += HandleControlChange;
    }

    void OnDisable()
    {
        InputManager.onActivate.performed -= ActivateObject;
        InputManager.onJSMouseMove.performed -= MouseHandler;
        InputManager.onJSMouseMove.canceled -= MouseHandler;
        InputManager.onJSMouseMove.started -= MouseHandler;
        InputManager.onMouseMove.performed -= MouseHandler;
        InputManager.onControlsChanged -= HandleControlChange;
    }

}
