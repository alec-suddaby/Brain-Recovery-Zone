using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IconHighlighter : MonoBehaviour
{
    public List<RawImage> images;

    public float highlightTime;
    public float fadeTime;
    public Color normalColour;
    public Color highlightColour;

    private bool inProgress = false;

    public RawImage indicatorIcon;

    public void ToggleHighlighter(){
        if(inProgress){
            StopHighlighter();
        }else{
            StartHighlighter();
        }

        inProgress = !inProgress;

        if(indicatorIcon != null){
            indicatorIcon.color = inProgress ? highlightColour : normalColour;
        }
    }

    public void StartHighlighter(){
        StartCoroutine("Highlight");
    }

    IEnumerator Highlight(){
        while(true){
            foreach(RawImage image in images){
                float timeElapsed = 0f;
                while(timeElapsed < fadeTime){
                    timeElapsed += Time.deltaTime;

                    image.color = Color.Lerp(normalColour, highlightColour, timeElapsed/fadeTime);

                    yield return new WaitForSeconds(Time.deltaTime);
                }

                yield return new WaitForSeconds(highlightTime);

                timeElapsed = 0f;
                while(timeElapsed < fadeTime){
                    timeElapsed += Time.deltaTime;

                    image.color = Color.Lerp(highlightColour, normalColour, timeElapsed/fadeTime);

                    yield return new WaitForSeconds(Time.deltaTime);
                }

                image.color = normalColour;
            }
        }
    }

    public void StopHighlighter(){
        StopCoroutine("Highlight");

        foreach(RawImage image in images){
            image.color = normalColour;
        }
    }
}
