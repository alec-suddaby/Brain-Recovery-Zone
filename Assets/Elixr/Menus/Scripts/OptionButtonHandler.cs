using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OptionButtonHandler : MonoBehaviour
{
    public Text Title;
    public Button Button;
    public UnityEvent<int, Button> OnClick;

    private PreferenceOption.Option option;

    public void InitOption(PreferenceOption.Option option)
    {
        this.option = option;
        Title.text = option.name;

        Button.onClick.AddListener(Clicked);
    }

    public void Clicked()
    {
        OnClick.Invoke(option.value, Button);
    }
}
