using Elixr.MenuSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LikertScale : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;

    [SerializeField] private Slider likertSlider;

    [SerializeField] private Slider previousValueSlider = null;

    private UnityEvent<float> onComplete = new ();

    private Elixr.MenuSystem.MenuManager menuManager;

    private float fadeDuration = 1f;

    public void Init(Elixr.MenuSystem.MenuManager menuManager, int initialValue = 5, bool active = false)
    {
        this.menuManager = menuManager;

        if(!active)
            StartCoroutine(menuManager.Fade(0, 0, false, panel, 0));
        else
            StartCoroutine(menuManager.Fade(1, 0, true, panel, 0));

        SetValue(initialValue);
    }

    public void Display(float fadeDuration, bool display, UnityAction<float> eventOnComplete = null, float? previousValue = null, float? fadeDelay = 0)
    {
        this.fadeDuration = fadeDuration;

        StartCoroutine(menuManager.Fade(display ? 1 : 0, fadeDuration, display, panel, (float)fadeDelay));

        onComplete = new();

        if(eventOnComplete != null)
            onComplete.AddListener(eventOnComplete);

        if(previousValueSlider != null && previousValue != null)
        {
            previousValueSlider.gameObject.SetActive(true);
            previousValueSlider.value = (float)previousValue;
        }

        if(!display && previousValueSlider != null)
        {
            previousValueSlider.gameObject.SetActive(false);
        }
    }

    public void AddListener(UnityAction<float> eventListener)
    {
        onComplete.AddListener(eventListener);
    }

    public void SetValue(float value)
    {
        likertSlider.value = value;
    }

    public void Complete()
    {
        onComplete.Invoke(likertSlider.value);
        Display(menuManager.transitionTime, false, fadeDelay: 0);
    }
}
