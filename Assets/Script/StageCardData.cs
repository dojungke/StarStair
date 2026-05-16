using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageCard", menuName = "Scriptable Objects/StageCards")]
public class StageCardData : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Image;
}