using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PreferenceOption
{
    [System.Serializable]
    public class Option
    {
        public string name;
        public int value;
    }

    public string PreferenceName;
    public string Title;
    [TextArea] public string Description;
    public int DefaultValue = 0;

    public List<Option> Options;
}
