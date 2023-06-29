using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CutsceneStep
{
    public string text;
    public bool doesCharacterExit;
    public bool doesCharacterEnter;
    public SpeakerController speaker;
    public bool customSpeed;
    public float textSpeed;
}

[System.Serializable]
public enum StepType
{
    DIALOG,
    EXIT,
    ENTER
}