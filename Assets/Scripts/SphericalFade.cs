using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SphericalFade : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Default");
        fadeMaterial.SetFloat("_Radius", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMouse){
            ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        }
        else 
        {
            ray = Camera.main.ScreenPointToRay(Vector3.zero);
        }
        
        if (toggleSphere)
        {
            if (Physics.Raycast(ray, out hit))
            {
                fadeMaterial.SetVector("_Position", hit.point);
                glassFadeMaterial.SetVector("_Position", hit.point);
                transform.position = hit.point;
            }
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



}
