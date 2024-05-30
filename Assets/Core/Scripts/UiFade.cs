using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiFade : MonoBehaviour
{
    public CanvasGroup mask;

    [Range(0,1)]
    public float maxAlpha = 0.62f;
    public float fadeSpeed = 0.1f;
    public bool startActive = false;


    void Awake(){
        mask.alpha = startActive ?  maxAlpha : 0;
        mask.gameObject.SetActive(startActive);
    }

    IEnumerator FadeIn(){
        mask.gameObject.SetActive(true);

        while(mask.alpha < maxAlpha){
            mask.alpha = Mathf.Clamp(mask.alpha + (fadeSpeed * Time.deltaTime), 0, maxAlpha);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator FadeOut(){
        while(mask.alpha > 0){
            mask.alpha = Mathf.Clamp(mask.alpha - (fadeSpeed * Time.deltaTime), 0, maxAlpha);
            yield return new WaitForEndOfFrame();
        }
        
        mask.gameObject.SetActive(false);
    }
}
