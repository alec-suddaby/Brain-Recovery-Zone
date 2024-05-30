using UnityEngine;

namespace Elixr.PlayerPreferences
{

    [CreateAssetMenu(fileName = "IntPref", menuName = "Elixr/Player Preference/Integer")]
    public class PlayerPrefInt : PlayerPrefDefinition<int>
    {
        public override int Read()
        {
            return PlayerPrefs.GetInt(playerPrefName, defaultValue);
        }

        public override void Write(int value)
        {
            PlayerPrefs.SetInt(playerPrefName, value);
            PlayerPrefs.Save();
        }
    }
}
