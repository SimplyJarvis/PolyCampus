using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyFade : MonoBehaviour
{

    [SerializeField] Color[] FloorColors;
    [SerializeField] Material SkyMaterial;

    void OnEnable()
    {
        GameManager.OnFloorChanged += ChangeSky;
    }

    void OnDisable()
    {
        GameManager.OnFloorChanged -= ChangeSky;
    }

    void ChangeSky(int num)
    {
        StopAllCoroutines();
        StartCoroutine(FadeSky(FloorColors[num]));
    }

    IEnumerator FadeSky(Color col)
    {
        while(SkyMaterial.GetColor("_Tint") != col)
        {
            SkyMaterial.SetColor("_Tint", Color.Lerp(SkyMaterial.GetColor("_Tint"), col, Time.deltaTime * 2.5f));
            yield return null;
        }
       // Debug.Log(SkyMaterial.GetColor("_Tint") + " " + col);
    }
}
