using UnityEngine;

public class Panel : MonoBehaviour
{
    private Canvas canvas = null;
    private MenuManager menuManager = null;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    public void Setup(MenuManager menuManger)
    {
        this.menuManager = menuManager;
        Hide();
    }

    public void Show()
    {
        canvas.enabled = true;

        //int tween or co routine
        // can work for canvas groups to transition here
    }

    public void Hide()
    {
        canvas.enabled = false;
    }
}
