using System.Collections.Generic;
using UnityEngine;

public abstract class CardAbility : MonoBehaviour
{
    public GameObject SkillCostViewerManager; // 스킬 코스트 뷰어 매니저
    public GameObject BattleManager; // 배틀 매니저
    public GameObject HandManager; // 핸드 매니저
    protected SkillCostViewerManager skillCostViewerManager;
    protected BattleManager battleManager;
    protected Hand handManager;
    public Card card; // 카드 데이터
    public CardAbilityManager cardAbilityManager; // 카드 능력 매니저
    protected virtual void Start()
    {
        skillCostViewerManager = SkillCostViewerManager.GetComponent<SkillCostViewerManager>();
        battleManager = BattleManager.GetComponent<BattleManager>();
        handManager = HandManager.GetComponent<Hand>();
    }
    public abstract void BattleCardActive(GameObject target, string cardName, int canUseItNow); // 카드 사용
    public virtual void CardAbilityActive(List<AbilityValues> Ability, string Color, int Number)
    {
        Debug.Log(Ability[0].AbilityType);
        Enemy Targetenemy = cardAbilityManager.Targetenemy;
        foreach (AbilityValues CardAbilities in Ability)
        {
            string cardAbilitieType = CardAbilities.AbilityType;
            float CardAbilitieValue = CardAbilities.Value;
            switch (CardAbilities.AbilityType)
            {
                case "Attack":
                    cardAbilityManager.Attack(CardAbilities); break;
                case "CardAttack":
                    cardAbilityManager.Attack(CardAbilities); break;
                case "GiveEffect":
                    cardAbilityManager.GiveEffect(CardAbilities); break;
                case "GiveManaGuard":
                    cardAbilityManager.GiveManaGuard(CardAbilities); break;
                case "GetStat":
                    cardAbilityManager.GetStat(CardAbilities); break;
                case "Honkai":
                    if (Targetenemy.ManaGuard > 0)
                    {
                        CardAbilities.Value = CardAbilities.Value * 2;
                        Debug.Log(CardAbilities.Value);
                        cardAbilityManager.Attack(CardAbilities); break;
                    }
                    else
                    {
                        cardAbilityManager.Attack(CardAbilities); break;
                    }
                case "Drow":
                    handManager.Drow((int)CardAbilities.Value, CardAbilities.Content); break;
                case "MagicArrow":
                    switch (Color)
                    {
                        case "R":
                            AbilityValues GiveFire = new AbilityValues
                            {
                                cardInfo = CardAbilities.cardInfo,
                                AbilityType = "GiveEffect",
                                Target = TargetTypes.Single,
                                Content = "Fire",
                                Value = 6
                            };
                            cardAbilityManager.Attack(CardAbilities); cardAbilityManager.GiveEffect(GiveFire); break;
                        case "B":
                            CardAbilities.Content = "HpAttack";
                            cardAbilityManager.Attack(CardAbilities); break;
                        case "G":
                            AbilityValues GetHp = new AbilityValues
                            {
                                AbilityType = "GetStat",
                                Target = TargetTypes.Player,
                                Content = "Hp",
                                Value = cardAbilityManager.Attack(CardAbilities) / 2
                            };
                            cardAbilityManager.GetStat(GetHp); break;
                        case "S":
                            AbilityValues MultiAttack = new AbilityValues
                            {
                                cardInfo = CardAbilities.cardInfo,
                                AbilityType = "Attack",
                                Target = TargetTypes.Multi,
                                Content = "Attack",
                                Value = 4
                            };
                            cardAbilityManager.Attack(CardAbilities); cardAbilityManager.Attack(MultiAttack); break;
                        case "T":
                            cardAbilityManager.Attack(CardAbilities); handManager.Drow(1, "마법화살 하양"); break; //드로우는 카드가 아니더라도 하는 경우가 많으므로 어빌리티벨류스로 안함
                    }
                    break;
            }
        }
    }
}
