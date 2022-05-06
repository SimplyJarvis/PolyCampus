using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    public static GameManager Instance {get {return _instance;}}
    public static event Action<int> OnFloorChanged;
    [SerializeField]
    int Floor = 0;
    InputAction c_FloorChange;
   
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        c_FloorChange = InputManager.Instance.inputActionMap.FindAction("FloorChange");
        c_FloorChange.performed += C_switchLevel;
    }


   public void UpdateLevel(int level){
        Floor = level;

        switch(Floor){
            case 0:
            break;
            case 1:
            break;
            default:
            break;
        }

        OnFloorChanged?.Invoke(Floor);
    }

    public int GetLevel()
    {
        return Floor;
    }

    public void t_switchLevel()
    {
        UpdateLevel(Floor == 0 ?  1 : 0);
    }

    private void C_switchLevel(InputAction.CallbackContext context)
    {
        t_switchLevel();
    }

    private void OnDisable()
    {
        c_FloorChange.performed -= C_switchLevel;
    }
}

