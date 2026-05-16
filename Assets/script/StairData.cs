using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StairData", menuName = "Scriptable Objects/StairData")]
public class StairData : ScriptableObject
{
    public string stairName;
    public Sprite Image;
    public List<string> steps;
}