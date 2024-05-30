using Elixr.MenuSystem;
using System;
using UnityEngine;

[System.Serializable]
public class UnlockableLoadLevel : LoadLevelNode
{
    public UnlockLevelValue LevelRequired;
    public UnlockLevelValue StageRequired;
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

                if (LevelRequired.MinimumValue < LevelRequired.PrefValue.Read())
                {
                    return true;
                }

                if (LevelRequired.MinimumValue == LevelRequired.PrefValue.Read())
                {
                    if (StageRequired.PrefValue == null)
                    {
                        return true;
                    }

                    if (StageRequired.PrefValue.Read() >= StageRequired.MinimumValue)
                    {
                        return true;
                    }
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
