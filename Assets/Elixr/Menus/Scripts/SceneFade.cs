using System;
using System.Collections;
using UnityEngine;

public class SceneFade : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public GameObject fadeObject;
    public float fadeTime = 2.5f;

    public bool transitioning = false;

    public void Start()
    {
        Color colour = meshRenderer.material.color;
        colour.a = 1;
        meshRenderer.material.color = colour;

        FadeIn();
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(0, false));
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(1, true));
    }

    private IEnumerator Fade(float targetAlpha, bool endActive)
    {
        fadeObject.SetActive(true);
        float startAlpha = meshRenderer.material.color.a;
        float difference = startAlpha - targetAlpha;

        float currentFadeTime = 0f;
        float step = difference * Time.fixedDeltaTime / ((Mathf.Abs(difference) / 1f) * fadeTime);

        float currentAlpha = startAlpha;

        while (currentFadeTime < (Mathf.Abs(difference) / 1f) * fadeTime)
        {
            currentFadeTime += Time.fixedDeltaTime;

            Color colour = meshRenderer.material.color;
            currentAlpha = Mathf.Clamp(currentAlpha - step, 0, 1);
            colour.a = currentAlpha / 1f;
            meshRenderer.material.color = colour;

            yield return new WaitForFixedUpdate();
        }
        fadeObject.SetActive(endActive);
    }

    public IEnumerator FadeTransition(Action onFadeOut, Action onComplete)
    {
        if (transitioning)
        {
            yield return null;
        }

        transitioning = true;

        StartCoroutine(Fade(1, true));
        yield return new WaitForSeconds(fadeTime);
        onFadeOut.Invoke();
        StartCoroutine(Fade(0, false));
        yield return new WaitForSeconds(fadeTime);
        onComplete.Invoke();

        transitioning = false;
    }
}
