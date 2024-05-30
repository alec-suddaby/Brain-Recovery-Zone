using UnityEngine;

namespace Elixr.MenuSystem
{
    [CreateAssetMenu(menuName = "Elixr/Menus/Menu Position Tracker")]
    public class MenuPositionTracker : ScriptableObject
    {
        public string lastMenuPositionGuid = "";

        private void OnEnable()
        {
            lastMenuPositionGuid = "";
            hideFlags = HideFlags.DontUnloadUnusedAsset;
        }
    }
}