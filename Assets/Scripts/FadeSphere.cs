using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FadeSphere : MonoBehaviour
{


    [SerializeField] Material fadeMaterial;
    [SerializeField] Material glassFadeMaterial;
    [SerializeField] float SphereRadius;
    [SerializeField] float SphereGrowSpeed;
    bool toggleSphere;
    RaycastHit hit;
    Ray ray;
    int layerMask;
    bool isMouse = true;
    float targetSphereRadius;

    //Controls
    InputAction c_toggleSphere;
    InputAction c_analogueSphere;


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
            targetSphereRadius = SphereRadius;
            StartCoroutine(GrowSphere());
        }
        if (context.performed || context.canceled)
        {
            toggleSphere = false;
            targetSphereRadius = 0;
        }
    }

    public void AnalogueSphere(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            toggleSphere = true;
            StopAllCoroutines();
            targetSphereRadius = SphereRadius * context.ReadValue<float>();
            StartCoroutine(GrowSphere());
        }
        if (context.performed || context.canceled)
        {
            targetSphereRadius = SphereRadius * context.ReadValue<float>();
            if (context.ReadValue<float>() == 0) toggleSphere = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fadeMaterial.GetFloat("_Radius"));
    }



    IEnumerator GrowSphere()
    {
        bool isActive = true;
        while (isActive)
        {
            if (targetSphereRadius == 0 && fadeMaterial.GetFloat("_Radius") <= 0.1f)
            {
                fadeMaterial.SetFloat("_Radius", 0f);
                isActive = false;
            }
            fadeMaterial.SetFloat("_Radius", Mathf.Lerp(fadeMaterial.GetFloat("_Radius"), targetSphereRadius, Time.deltaTime * SphereGrowSpeed));
            yield return null;
        }

    }


    void OnEnable()
    {
        c_toggleSphere = InputManager.Instance.inputActionMap.FindAction("ToggleSphereFade");
        c_analogueSphere = InputManager.Instance.inputActionMap.FindAction("AnalogueSphereFade");
        c_toggleSphere.started += ToggleSphere;
        c_toggleSphere.performed += ToggleSphere;
        c_toggleSphere.canceled += ToggleSphere;

        c_analogueSphere.started += AnalogueSphere;
        c_analogueSphere.performed += AnalogueSphere;
        c_analogueSphere.canceled += AnalogueSphere;
    }

    void OnDisable()
    {
        c_toggleSphere.started -= ToggleSphere;
        c_toggleSphere.performed -= ToggleSphere;
        c_toggleSphere.canceled -= ToggleSphere;

        c_analogueSphere.started -= AnalogueSphere;
        c_analogueSphere.performed -= AnalogueSphere;
        c_analogueSphere.canceled -= AnalogueSphere;
    }



}
