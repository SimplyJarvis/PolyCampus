using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalFade : MonoBehaviour
{

    [SerializeField]
    Material fadeMaterial;
    [SerializeField]
    Material glassFadeMaterial;
    RaycastHit hit;
    Ray ray;
    int layerMask;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Default");
    }

    // Update is called once per frame
    void Update()
    {
        
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)){
            fadeMaterial.SetVector("_Position", hit.point);
            glassFadeMaterial.SetVector("_Position", hit.point);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fadeMaterial.GetFloat("_Radius"));
    }
}
