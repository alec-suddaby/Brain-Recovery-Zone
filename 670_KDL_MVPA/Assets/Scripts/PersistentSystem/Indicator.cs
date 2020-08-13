using UnityEngine;

public class Indicator : MonoBehaviour
{

    private void Awake()
    {

    }

    private void Start()
    {
        Deactivate();
    }

    public void Show()
    {
        Debug.Log("############### SHOW");
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        Debug.Log("############### HIDE");
        gameObject.SetActive(false);
    }

    public void Deactivate()
    {
        Debug.Log("############### DEACTIVEATED");
        gameObject.SetActive(false);
    }
}
