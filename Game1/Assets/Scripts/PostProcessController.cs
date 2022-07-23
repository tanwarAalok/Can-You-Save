using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
public class PostProcessController : MonoBehaviour
{
    public static PostProcessController instance;
    Volume postProcessVolume;
    Vignette vignette;
    private void Awake()
    {
        postProcessVolume = GetComponent<Volume>();
        instance = this;
    }
    public void VignetteColor()
    {
       if(postProcessVolume.profile.TryGet<Vignette>(out vignette))
       {
            vignette.color.value = new Color(1, 0, 0);
            StartCoroutine(OrignalVignetteColor());
       }
    }
    IEnumerator OrignalVignetteColor()
    {
        yield return new WaitForSeconds(0.5f);
        vignette.color.value = new Color(0, 0, 0);
    }
    public void VignetteIntensity(float intensity)
    {
        vignette.intensity.value = Mathf.Min(intensity, 0.9f);
    }
}
