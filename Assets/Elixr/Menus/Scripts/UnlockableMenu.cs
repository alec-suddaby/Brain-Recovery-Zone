using System;
using UnityEngine;

namespace Elixr.MenuSystem
{
    [System.Serializable]
    public class UnlockableMenu : Menu
    {
        public UnlockLevelValue LevelRequired;

        public override bool isEnabled
        {
            get
            {
                try
                {
                    if (LevelRequired.PrefValue == null)
                    {
                        return true;
                    }

                    if (LevelRequired.MinimumValue <= LevelRequired.PrefValue.Read())
                    {
                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    Debug.Log($"{ex.Message}. {ex.StackTrace}");
                }
                return true;
            }
        }
    }
}
