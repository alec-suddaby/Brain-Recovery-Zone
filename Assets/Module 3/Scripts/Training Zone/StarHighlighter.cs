using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarHighlighter : MonoBehaviour
{
    public Image image;
    [Range(0f,0.5f)]
    public float targetAlpha = 0.5f;
    public float fadeSpeed = 2f;
    private bool fadingOut = false;

    void Awake(){
        StartCoroutine("FadeIn");
    }

    IEnumerator FadeIn(){
        Color matColour = image.color;
        float alpha = 0f;

        while(alpha < targetAlpha){
            alpha += fadeSpeed * Time.fixedDeltaTime;
            alpha = Mathf.Clamp01(alpha);
            matColour.a = alpha;
            image.color = matColour;
            yield return new WaitForFixedUpdate();
        }
        
    }

    IEnumerator FadeOut(){
        Color matColour = image.color;
        float alpha = matColour.a;

        while(alpha > 0f){
            alpha -= fadeSpeed * Time.fixedDeltaTime;
            alpha = Mathf.Clamp01(alpha);
            matColour.a = alpha;
            image.color = matColour;
            yield return new WaitForFixedUpdate();
        }
        
        Destroy(gameObject);
    }

    public void StartFadeOut(){
        if(fadingOut){
            return;
        }

        fadingOut = true;
        StartCoroutine("FadeOut");
    }
}
