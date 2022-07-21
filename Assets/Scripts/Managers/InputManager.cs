using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance { get { return instance; } }
    public PlayerInput ControlInput;
    [SerializeField]
    private InputActionAsset inputActionAsset;
    public InputActionMap inputActionMap;
    public static event Action<bool> SphereActive;
    public static event Action<Controller_Enum> onControlsChanged;
    private bool _isSphereActive = false;
    public bool isSphereActive { get { return _isSphereActive; } }
    Controller_Enum activeController;
    InputAction onToggleSphere;

    public static InputAction onAnalogueSphere; //Used for controller sphere radius
    public static InputAction onFloorChange; //Change Floors
    public static InputAction onPlaceSphere; //Placing sphere with mouse
    public static InputAction onMouseMove; //Whenever the mouse is moved
    public static InputAction onJSMouseMove; //Joystick Camera move
    public static InputAction onActivate; //The main interaction (Object interaction)
    public static InputAction onSecondary; //Secondary Input (Currently used for About UI)
    public static InputAction onToggleCameraMove; //Triggered when button for moving camera is pushed (mouse only)
    public static InputAction onZoom; //Zooming Camera In and Out
    public static InputAction onQuit; //Quitting The Game


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
        Debug.Log(instance);
        ControlInput = GetComponent<PlayerInput>();
        inputActionMap = inputActionAsset.FindActionMap("Main");
        ControlInput.onControlsChanged += ControlChange;
        AssignControls();
    }

    void Start()
    {
        ControlChange(ControlInput);   
    }

    public void SetSphere(bool isActive)
    {
        _isSphereActive = isActive;
        SphereActive?.Invoke(_isSphereActive);
    }

    void ControlChange(PlayerInput input)
    {
        if (input.currentControlScheme == "GamePad")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            activeController = Controller_Enum.Xbox;
            onControlsChanged?.Invoke(activeController);
            return;
        }
        else 
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            activeController = Controller_Enum.Keyboard;
            onControlsChanged?.Invoke(activeController);
        }

        
    }

    void AssignControls()
    {
        onFloorChange = inputActionMap.FindAction("FloorChange");
        onActivate = inputActionMap.FindAction("ActivateObject");
        onMouseMove = inputActionMap.FindAction("MousePos");
        onJSMouseMove = inputActionMap.FindAction("JS_Camera");
        onToggleCameraMove = inputActionMap.FindAction("ToggleCameraMove");
        onZoom = inputActionMap.FindAction("Zoom");
        onPlaceSphere = inputActionMap.FindAction("ToggleSphereFade");
        onAnalogueSphere = inputActionMap.FindAction("AnalogueSphereFade");
        onSecondary = inputActionMap.FindAction("Secondary");
        onQuit = inputActionMap.FindAction("Quit");
    }

    void OnEnable()
    {
        onActivate.started += HideTutorial;
    }

    void OnDisable()
    {
        onActivate.started -= HideTutorial;
    }

    void HideTutorial(InputAction.CallbackContext context)
    {
        if (GameManager.isTutorial)
        {
            GameManager.setTutorialEnabled(false);
        }
    }

    public Controller_Enum GetActiveController()
    {
        return activeController;
    }

}
