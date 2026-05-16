using System.Collections.Generic;
using UnityEngine;

public class CardNomal : CardAbility
{
    public override void CardAbilityActive(List<AbilityValues> Ability, string Color, int Number)
    {
        if (gameObject.GetComponent<Card>().CanUseItNow == 2 && gameObject.GetComponent<Card>().ThisCard.coolTime <= 1)
        {
            base.CardAbilityActive(Ability, Color, Number);
            base.CardAbilityActive(Ability, Color, Number);
        }
        else
        {
            base.CardAbilityActive(Ability, Color, Number);
        }
    }
    public override void BattleCardActive(GameObject target, string cardName, int canUseItNow)
    {
        if (canUseItNow > 0)
        {
            // 카드 사용 가능
            Debug.Log($"카드 {cardName} 사용됨");
            //gameObject.GetComponent<Card>().UsedCardRemove(); // 카드 사용 불가 시 카드 제거
        }
        else
        {
            Debug.Log($"카드 {cardName} 사용 불가");
            //gameObject.GetComponent<Card>().UsedCardRemove(); // 카드 사용 불가 시 카드 제거
        }
    }
}