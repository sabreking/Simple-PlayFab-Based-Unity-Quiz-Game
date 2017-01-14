using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : UnitySingleton<GlobalSettings>
{
    [Header("In Seconds")]
    public float timeAllowancePerQuestion = 5;
    public float scoreAwardForEasyQuestions = 10f;
    public float scoreMultiplierForMediumQuestions = 1.2f;
    public float scoreMultiplierForHardQuestions = 1.5f;
}
