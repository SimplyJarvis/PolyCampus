using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIHelpers
{
    public class UI_Tools
    {
        public static IEnumerator FadeText(float targetAlpha, CanvasGroup cg, float fadeSpeed)
        {
            while (cg.alpha != targetAlpha)
            {
                cg.alpha = Mathf.Lerp(cg.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
                if (Mathf.Abs(cg.alpha - targetAlpha) < 0.02f) cg.alpha = targetAlpha;
                yield return null;
            }
            yield return new WaitForSeconds(2f);
        }
    }
}

