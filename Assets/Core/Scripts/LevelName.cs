using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelName
{
    public int moduleNumber;
    public int levelNumber;
    public int exerciseNumber;
    public LevelType levelType;

    public enum LevelType{
        WarmUp,
        Training
    }

    public string GetLevelName{
        get{
            string levelTypeString = "";
            switch(levelType){
                case LevelType.WarmUp:
                    levelTypeString="W";
                    break;
                case LevelType.Training:
                    levelTypeString="T";
                    break;
            }
            return "M" + moduleNumber.ToString() + levelTypeString + levelNumber.ToString() + "E" + exerciseNumber;
        }
        
    }
}
