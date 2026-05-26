using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public enum ActionType { Quick, SingleAttack, Dummy, PlayAnimation, Move }
[Serializable]
public struct StepInfo
{
    public string stepType;
    public string stepName;
    public string stepDescription;
    public List<StepAction> actions;
}
[Serializable]
public struct StepAction
{
    public ActionType actionType;
    public string value;
}

[CreateAssetMenu(fileName = "StairData", menuName = "Scriptable Objects/StairData")]
public class StairData : ScriptableObject
{
    public string stairName;
    public Sprite Image;
    public List<StepInfo> StepInfo;
}