using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent (typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance {get {return instance;}}
    public PlayerInput ControlInput;
    [SerializeField]
    private InputActionAsset inputActionAsset;
    public InputActionMap inputActionMap;

    void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
        Debug.Log(instance);
        ControlInput = GetComponent<PlayerInput>();
        inputActionMap = inputActionAsset.FindActionMap("Main");
    }

}
