using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField]
    Transform[] floorCamPos = new Transform[2];
    [SerializeField]
    GameObject CameraObject;
    [SerializeField]
    float cameraOffset = 1f;
    [SerializeField]
    [Range(1f, 5f)]
    float cameraSpeed = 1f;
    [SerializeField]
    float scrollScale = 1f;
    float zoomLevel;
    Vector2 cameraMousePos;
    Camera MainCamera;
    Transform activeFloor;
    Vector2 mouseStart;

    Vector3 CameraPos = Vector3.zero;

    void Awake()
    {
        GameManager.OnFloorChanged += FloorChange;
        MainCamera = CameraObject.GetComponent<Camera>();
        CameraPos = CameraObject.transform.position;
        activeFloor = floorCamPos[0];
        zoomLevel = MainCamera.orthographicSize;
    }

    void OnDestroy()
    {
        GameManager.OnFloorChanged -= FloorChange;
    }

    void Update()
    {

        CameraPos = new Vector3(activeFloor.position.x + cameraOffset, CameraObject.transform.position.y, CameraPos.z);



        cameraMousePos = MainCamera.ScreenToViewportPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f, 0);

        if (Input.GetMouseButtonDown(1))
        {
            mouseStart = MainCamera.ScreenToViewportPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f, 0);
        }

        if (Input.GetMouseButton(1))
        {
            Vector2 currentMousePos = MainCamera.ScreenToViewportPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f, 0);
            Vector2 mouseDifference = (mouseStart - currentMousePos);
            CameraPos.z -= (mouseDifference.x - mouseDifference.y) < -0.05f || (mouseDifference.x - mouseDifference.y) > 0.05f ? (mouseDifference.x - mouseDifference.y) / (cameraSpeed) : 0f;
            //Debug.Log(cameraOffset);
            if (cameraOffset > -8f && cameraOffset < 15f)
            {
                cameraOffset += (mouseDifference.y + mouseDifference.x) < -0.05f || (mouseDifference.y + mouseDifference.x) > 0.05f ? (mouseDifference.y + mouseDifference.x) / (cameraSpeed) : 0f;
            }
            else 
            {
                cameraOffset = Mathf.MoveTowards(cameraOffset, 0, Time.deltaTime * 2f);
            }
            
        }


        CameraObject.transform.position = Vector3.Lerp(CameraObject.transform.position, CameraPos, Time.deltaTime * 3.5f);
        MainCamera.orthographicSize = Mathf.Lerp(MainCamera.orthographicSize, zoomLevel, Time.deltaTime * 3.5f);

    }

    void OnGUI()
    {
        if (zoomLevel > 2 || Input.mouseScrollDelta.y < 0f)
        {
            if (zoomLevel < 9 || Input.mouseScrollDelta.y > 0f)
            {
                zoomLevel -= Input.mouseScrollDelta.y * scrollScale;
                cameraSpeed += Input.mouseScrollDelta.y / 2;
            }
        }
    }


    void FloorChange(int num)
    {
        activeFloor = floorCamPos[num];
        cameraOffset = 0;
    }
}
