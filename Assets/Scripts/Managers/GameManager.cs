using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    public static event Action<int> OnFloorChanged;
    public static event Action<bool> OnTutorialToggle;
    [SerializeField]
    int Floor = 0;
    InputAction c_FloorChange;
    [SerializeField] GameObject firstFloor;
    [SerializeField] GameObject secondFloor;
    public static bool isTutorial;
    float idleTimer;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        
        InputManager.onFloorChange.performed += C_switchLevel;
        InputManager.onMouseMove.started += ResetIdle;
        InputManager.onMouseMove.performed += ResetIdle;
        InputManager.onJSMouseMove.started += ResetIdle;
        InputManager.onActivate.started += ResetIdle;
        InputManager.onQuit.started += QuitGame;
    }


    public void UpdateLevel(int level)
    {
        Floor = level;
        Invoke("HideFloor", 0.2f);

        OnFloorChanged?.Invoke(Floor);
    }

    void HideFloor()
    {
        switch (Floor)
        {
            case 0:
                firstFloor.SetActive(true);
                secondFloor.SetActive(false);
                break;
            case 1:
                firstFloor.SetActive(false);
                secondFloor.SetActive(true);
                break;
            default:
                break;
        }
    }

    public static void setTutorialEnabled(bool tutorial)
    {
        isTutorial = tutorial;
        OnTutorialToggle?.Invoke(isTutorial);
    }

    public int GetLevel()
    {
        return Floor;
    }

    public void t_switchLevel()
    {
        UpdateLevel(Floor == 0 ? 1 : 0);
    }

    private void C_switchLevel(InputAction.CallbackContext context)
    {
        if (!QuitUI.Instance.QuitStatus())
        {
            t_switchLevel();
        }
    }

    private void ResetIdle(InputAction.CallbackContext context)
    {
        idleTimer = 0f;
    }

    void QuitGame(InputAction.CallbackContext context)
    {
        QuitApp();
    }

    public void QuitApp()
    {
        QuitUI.Instance.ToggleQuit();
    }

    void Update()
    {
        if (!isTutorial)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > 30f)
            {
                setTutorialEnabled(true);
            }
        }
        
    }

    private void OnDisable()
    {
        InputManager.onFloorChange.performed -= C_switchLevel;
        InputManager.onMouseMove.started -= ResetIdle;
        InputManager.onMouseMove.performed -= ResetIdle;
        InputManager.onJSMouseMove.started -= ResetIdle;
        InputManager.onActivate.started -= ResetIdle;
        InputManager.onQuit.started -= QuitGame;
    }
}

