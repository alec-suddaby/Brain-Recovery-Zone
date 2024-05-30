using UnityEngine;

namespace Elixr.PlayerPreferences
{
    [CreateAssetMenu(fileName = "StringPref", menuName = "Elixr/Player Preference/String")]
    public class PlayerPrefString : PlayerPrefDefinition<string>
    {
        public override string Read()
        {
            return PlayerPrefs.GetString(playerPrefName, defaultValue);
        }

        public override void Write(string value)
        {
            PlayerPrefs.SetString(playerPrefName, value);
            PlayerPrefs.Save();
        }
    }
}
