using System.Collections.Generic;
using UnityEngine;
public enum CardAbilityType
{
    Attack,
    GetStat,
    GiveEffect,
    GiveManaGuard,
}
public enum TargetTypes
{
    Single,
    Multi,
    Player,
}

[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Objects/Card")]
public class CardData : ScriptableObject
{
    public string Name;
    public string Description;
    public int Rare;
    public Sprite Image;
    public int timeCost;
    public string[] cardColor;
    public int coolTime;
    public string ActiveType;
    public List<EffectData> relatedEffect;
    public string AbilityScript;
    public List<AbilityValues> AbilityList;
}
[System.Serializable]
public class AbilityValues
{
    public string cardInfo = "NonCard";
    public string AbilityType;
    public TargetTypes Target;
    public string Content;
    public float Value;
}
public class CardReinforce
{
    public CardAbilityType reinforceType;
    public float reinforceRate; //카드 강화 정도 % 곱하기
}