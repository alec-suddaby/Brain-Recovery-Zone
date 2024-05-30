using UnityEngine;

namespace Elixr.PlayerPreferences
{
    [CreateAssetMenu(fileName = "FloatPref", menuName = "Elixr/Player Preference/Float")]
    public class PlayerPrefFloat : PlayerPrefDefinition<float>
    {
        public override float Read()
        {
            return PlayerPrefs.GetFloat(playerPrefName, defaultValue);
        }

        public override void Write(float value)
        {
            PlayerPrefs.SetFloat(playerPrefName, value);
            PlayerPrefs.Save();
        }
    }
}
