using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFallenStar : CardAbility
{
    public string FallenStarDamage = "8"; // Fallen Star 카드의 기본 데미지 값
    public override void BattleCardActive(GameObject target, string cardName, int canUseItNow)
    {
        // 카드 사용 로직 구현
        if (canUseItNow > 0)
        {
            // 카드 사용 가능
            Debug.Log($"카드 {cardName} 사용됨");
            // 여기에 카드 사용에 따른 추가 로직을 작성할 수 있습니다.
        }
        else
        {
            Debug.Log($"카드 {cardName} 사용 불가");
        }
    }
    public override void CardAbilityActive(List<AbilityValues> Ability, string Color, int Number)
    {
        // 카드 능력 활성화 로직

        if (gameObject.GetComponent<Card>().CanUseItNow >= 2)
        {
            gameObject.GetComponent<Card>().CanUseItNow = 0; // 카드 사용 후 사용 불가 상태로 변경
            StartCoroutine(StarFall(gameObject.GetComponent<Card>().cardAbilityList[1], Color, Number));
        }
        else
        {
            gameObject.GetComponent<Card>().CanUseItNow = 0; // 카드 사용 후 사용 불가 상태로 변경
            StartCoroutine(StarFall(gameObject.GetComponent<Card>().cardAbilityList[0], Color, Number));
        }
    }
    IEnumerator StarFall(AbilityValues abilityValue, string Color, int Number)
    {
        // 카드 능력 활성화 로직 구현
        Debug.Log($"카드 능력 활성화:  FallenStar - 색상: {Color}, 번호: {Number}");
        List<GameObject> cards = new List<GameObject>(handManager.HandCard);
        cards.Remove(this.gameObject);
        cardAbilityManager.Attack(abilityValue);
        yield return new WaitForSeconds(0.2f); // 카드 사용 후 잠시 대기
        foreach (GameObject card in cards)
        {
            cardAbilityManager.Attack(abilityValue);
            //card.GetComponent<Card>().UsedCardRemove();
            handManager.SortHand(0);
            yield return new WaitForSeconds(0.2f); // 카드 사용 후 잠시 대기
        }
        //gameObject.GetComponent<Card>().UsedCardRemove();
    }
}