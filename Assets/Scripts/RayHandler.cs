using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RayHandler : MonoBehaviour
{

    [SerializeField]
    Material fadeMaterial;
    [SerializeField]
    Material glassFadeMaterial;
    [SerializeField]
    float SphereRadius;
    [SerializeField]
    float SphereGrowSpeed;
    bool toggleSphere;
    RaycastHit hit;
    Ray ray;
    int layerMask;
    bool isMouse = true;

    //Controls
    InputAction c_toggleSphere;


    void Start()
    {
        layerMask = LayerMask.GetMask("Default");
        fadeMaterial.SetFloat("_Radius", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (toggleSphere)
        {
            
            fadeMaterial.SetVector("_Position", CursorController.Instance.MouseRayHit.point);
            glassFadeMaterial.SetVector("_Position", CursorController.Instance.MouseRayHit.point);
            transform.position = CursorController.Instance.MouseRayHit.point;

        }
    }

    public void ToggleSphere(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            StopAllCoroutines();
            toggleSphere = true;
            StartCoroutine(GrowSphere(true));
        }
        if (context.performed)
        {
            StopAllCoroutines();
            toggleSphere = false;
            StartCoroutine(GrowSphere(false));
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fadeMaterial.GetFloat("_Radius"));
    }



    IEnumerator GrowSphere(bool direction)
    {

        if (direction)
        {
            while (fadeMaterial.GetFloat("_Radius") < SphereRadius)
            {
                fadeMaterial.SetFloat("_Radius", Mathf.Lerp(fadeMaterial.GetFloat("_Radius"), SphereRadius, Time.deltaTime * SphereGrowSpeed));
                yield return null;
            }
        }
        else
        {
            while (fadeMaterial.GetFloat("_Radius") > 0)
            {
                fadeMaterial.SetFloat("_Radius", Mathf.Lerp(fadeMaterial.GetFloat("_Radius"), 0, Time.deltaTime * SphereGrowSpeed));
                if (fadeMaterial.GetFloat("_Radius") < 0.4) fadeMaterial.SetFloat("_Radius", 0);
                yield return null;
            }
        }

    }


    void OnEnable()
    {
        c_toggleSphere = InputManager.Instance.inputActionMap.FindAction("SphericalFade");
        Debug.Log(c_toggleSphere);
        c_toggleSphere.started += ToggleSphere;
        c_toggleSphere.performed += ToggleSphere;
    }

    void OnDisable()
    {
        c_toggleSphere.started -= ToggleSphere;
        c_toggleSphere.performed -= ToggleSphere;
    }



}
