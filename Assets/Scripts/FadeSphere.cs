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
    float targetSphereRadius;


    void Start()
    {
        fadeMaterial.SetFloat("_Radius", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null)
        {
            fadeMaterial.SetVector("_Position", transform.position);
            glassFadeMaterial.SetVector("_Position", transform.position);
        }

    }

    public void PlaceSphere(InputAction.CallbackContext context)
    {
        StopAllCoroutines();
        SetPosition();
        targetSphereRadius = SphereRadius;
        StartCoroutine(PlaceGrowSphere(true));
    }

    public void AnalogueSphere(InputAction.CallbackContext context) // This can be used for analogue inputs, such as the Xbox Controller Triggers
    {
        if (context.started)
        {
            SetPosition();
            ToggleParent();
            StopAllCoroutines();
            targetSphereRadius = SphereRadius * context.ReadValue<float>();
            StartCoroutine(PlaceGrowSphere(false));
        }
        if (context.performed || context.canceled)
        {
            targetSphereRadius = SphereRadius * context.ReadValue<float>();
            if (context.ReadValue<float>() == 0)
            {
                ToggleParent();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fadeMaterial.GetFloat("_Radius"));
    }

//This controls the growth of the sphere, isPlaced true = Grow, Wait for 2 seconds, Shrink. isPlaced False = Continus size using targetSphereRadius.
    IEnumerator PlaceGrowSphere(bool isPlaced)
    {
        bool isActive = true;
        InputManager.Instance.SetSphere(isActive);
        while (isActive)
        {

            if (targetSphereRadius == 0 && fadeMaterial.GetFloat("_Radius") <= 0.01f)
            {
                fadeMaterial.SetFloat("_Radius", 0f);
                glassFadeMaterial.SetFloat("_Radius", 0f);
                isActive = false;
            }

            if (Mathf.Abs(fadeMaterial.GetFloat("_Radius") - targetSphereRadius) < 0.1f && targetSphereRadius != 0f && isPlaced) //Wait once we reach the target size, then set the target back to 0 and carry on
            {
                targetSphereRadius = 0f;
                Debug.Log(Mathf.Abs(fadeMaterial.GetFloat("_Radius") - targetSphereRadius));
                yield return new WaitForSeconds(2f);
            }

            fadeMaterial.SetFloat("_Radius", Mathf.Lerp(fadeMaterial.GetFloat("_Radius"), targetSphereRadius, Time.deltaTime * SphereGrowSpeed));
            glassFadeMaterial.SetFloat("_Radius", Mathf.Lerp(fadeMaterial.GetFloat("_Radius"), targetSphereRadius, Time.deltaTime * SphereGrowSpeed));
            yield return null;
        }
        InputManager.Instance.SetSphere(isActive);
    }

    void SetPosition()
    {
        transform.position = new Vector3(CursorController.Instance.MouseRayHit.point.x, 1.5f, CursorController.Instance.MouseRayHit.point.z);
        fadeMaterial.SetVector("_Position", transform.position);
        glassFadeMaterial.SetVector("_Position", transform.position);
    }

    void ToggleParent() //parent to camera for following movement
    {
        transform.parent = transform.parent == null ? Camera.main.gameObject.transform : null;
    }

    void TimedShrink() // Use this to delay invoke the shrink with mouse toggle
    {
        targetSphereRadius = 0;
    }


    void OnEnable()
    {
        InputManager.onPlaceSphere.started += PlaceSphere;
        InputManager.onAnalogueSphere.started += AnalogueSphere;
        InputManager.onAnalogueSphere.performed += AnalogueSphere;
        InputManager.onAnalogueSphere.canceled += AnalogueSphere;
    }

    void OnDisable()
    {
        InputManager.onPlaceSphere.started -= PlaceSphere;
        InputManager.onAnalogueSphere.started -= AnalogueSphere;
        InputManager.onAnalogueSphere.performed -= AnalogueSphere;
        InputManager.onAnalogueSphere.canceled -= AnalogueSphere;
    }



}
