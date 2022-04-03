using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    public static GameManager Instance {get {return _instance;}}
    public static event Action<int> OnFloorChanged;
    [SerializeField]
    int Floor = 0;
   
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
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
}

