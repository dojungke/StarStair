using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopCard", menuName = "Scriptable Objects/ShopCards")]
public class ShopCardData : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Image;
}