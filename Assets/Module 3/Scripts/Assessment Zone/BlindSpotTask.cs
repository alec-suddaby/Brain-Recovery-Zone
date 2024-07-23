using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlindSpotTask
{
    public BlindSpotAssessment.TargetPosition targetPosition;
    public EyePatch.Eye eye;
    public bool isBlindSpot;
}
