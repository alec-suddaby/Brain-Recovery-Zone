using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreferenceHandler : MonoBehaviour
{
    public Text Title;
    public Text Description;

    public OptionButtonHandler OptionButtonPrefab;
    public Transform OptionButtonParent;

    public Color deselectedColour;
    public Color selectedColour;

    private PreferenceOption preferenceOption;

    private List<Button> buttons = new List<Button>();

    public void Init(PreferenceOption preferenceOption)
    {
        this.preferenceOption = preferenceOption;

        int prefValue = PlayerPrefs.GetInt(preferenceOption.PreferenceName, preferenceOption.DefaultValue);

        Title.text = preferenceOption.Title;
        Description.text = preferenceOption.Description;

        foreach (PreferenceOption.Option option in preferenceOption.Options)
        {
            OptionButtonHandler optionButton = Instantiate(OptionButtonPrefab, OptionButtonParent).GetComponent<OptionButtonHandler>();
            optionButton.InitOption(option);
            Button button = optionButton.GetComponent<Button>();
            buttons.Add(button);
            button.image.color = option.value == prefValue ? selectedColour : deselectedColour;
            optionButton.OnClick.AddListener(OptionSelected);
        }
    }

    private void OptionSelected(int value, Button selectedButton)
    {
        foreach (Button button in buttons)
        {
            button.image.color = deselectedColour;
        }

        selectedButton.image.color = selectedColour;

        PlayerPrefs.SetInt(preferenceOption.PreferenceName, value);
        PlayerPrefs.Save();
    }
}
