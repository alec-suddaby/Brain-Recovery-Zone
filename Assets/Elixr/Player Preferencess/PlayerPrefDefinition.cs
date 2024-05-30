using UnityEngine;

namespace Elixr.PlayerPreferences
{
    public abstract class PlayerPrefDefinition<T> : ScriptableObject
    {
        public string playerPrefName;
        public T defaultValue;

        public abstract T Read();
        public abstract void Write(T value);
    }
}
