using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameManager myScript = (GameManager)target;
        if(GUILayout.Button("Switch Level"))
        {
            GameManager.Instance.t_switchLevel();
        }
    }
}
