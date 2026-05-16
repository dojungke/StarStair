using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Scriptable Objects/Effect")]
public class EffectData : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Image;
}