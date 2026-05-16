using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Artifact", menuName = "Scriptable Objects/Artifact")]
public class ArtifactData : ScriptableObject
{
    public string Name;
    public string Description;
    public rarity Rarity;
    public Sprite Image;
    public int Size;
    public int Prize;
}