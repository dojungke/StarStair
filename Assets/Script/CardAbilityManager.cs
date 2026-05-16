using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardAbilityManager : MonoBehaviour
{
    public GameObject SkillCostViewerManager;
    public GameObject BattleManager;
    public GameObject HandManager;
    public ArtifactManager artifactManager;
    public SkillCostViewerManager skillCoolTimeViewerSetter;
    BattleManager battleManager;
    Hand handManager;
    public Enemy Targetenemy;
    public GameObject AttackEffectPrefab;
    public GraphicRaycaster graphicRaycaster;
    void Start()
    {
        skillCoolTimeViewerSetter = SkillCostViewerManager.GetComponent<SkillCostViewerManager>();
        battleManager = BattleManager.GetComponent<BattleManager>();
        handManager = HandManager.GetComponent<Hand>();

    }

    public void CardActive(GameObject Target, string CardName, int CanUseItNow, GameObject cardObject) //카드 사용
    {
        string CardColor = CardName.Split('/')[1]; //카드 색깔
        int CardNumber = int.Parse(CardName.Split('/')[2]); //카드 숫자
        CardName = CardName.Split('/')[0]; //카드 이름만 가져옴
        Targetenemy = Target.GetComponent<Enemy>();
        CardData ThisCard = Resources.Load<CardData>("DeckOfAll/" + CardName);
        if (ThisCard == null)
        {
            Debug.Log("카드 검색 실패");
        }
        else
        {
            Debug.Log("카드검색 성공:" + ThisCard.Name);
        }
        List<AbilityValues> Ability = cardObject.GetComponent<Card>().cardAbilityList;
        artifactManager.ArtifactActiveOnCardActive($"{CardName}", CanUseItNow);
        /*if (ThisCard.coolTime > 1)
        {
            if (skillCoolTimeViewerSetter.SkillCoolTimeAdd(CardName))
            {
                cardObject.GetComponent<CardAbility>().CardAbilityActive(Ability, CardColor, CardNumber);
                if (CanUseItNow >= 2)
                {
                    if (skillCoolTimeViewerSetter.SkillCoolTimeAdd(CardName))
                    {
                        cardObject.GetComponent<CardAbility>().CardAbilityActive(Ability, CardColor, CardNumber);
                    }
                }
                else
                {
                    Debug.Log("코스트 부족");
                }
            }
            else
            {
                Debug.Log("코스트 부족");

                if (CanUseItNow >= 2)
                {
                    if (skillCoolTimeViewerSetter.SkillCoolTimeAdd(CardName))
                    {
                        cardObject.GetComponent<CardAbility>().CardAbilityActive(Ability, CardColor, CardNumber);
                    }
                }
            }
        }
        else
        {
            cardObject.GetComponent<CardAbility>().CardAbilityActive(Ability, CardColor, CardNumber);
        }*/
        cardObject.GetComponent<CardAbility>().CardAbilityActive(Ability, CardColor, CardNumber);
        skillCoolTimeViewerSetter.SkillCoolTimeAdd(CardName,-ThisCard.timeCost);
        cardObject.GetComponent<CardAbility>().BattleCardActive(cardObject, cardObject.GetComponent<Card>().Name, cardObject.GetComponent<Card>().CanUseItNow);
        battleManager.TurnEnd(ThisCard.timeCost);
    }
    public float Attack(AbilityValues CardAbilities) //공격
    {
        float hitDamage = 0;
        if (CardAbilities.Target == TargetTypes.Single)
        {
            GameObject AttackEffect = Instantiate(AttackEffectPrefab, Targetenemy.transform);
            if (CardAbilities.Content == "HpAttack")
            {
                Debug.Log("마나가드 무시 공격" + Targetenemy.Name + CardAbilities.Value + "데미지");
                hitDamage += Targetenemy.HpDamage(CardAbilities.Value);
            }
            else
            {
                Debug.Log("공격" + Targetenemy.Name + CardAbilities.Value + "데미지");
                hitDamage += Targetenemy.Hit(CardAbilities.Value);
            }
            if (CardAbilities.cardInfo != "NonCard")
            {
                artifactManager.ArtifactActiveOnCardAttack(CardAbilities.cardInfo, Targetenemy, hitDamage);
            }
        }
        else
        {
            if (CardAbilities.Content == "HpAttack")
            {
                Debug.Log("마나가드 무시 공격" + CardAbilities.Target + CardAbilities.Value + "데미지");
                List<GameObject> targets = new List<GameObject>(battleManager.EnemyObjectList);
                foreach (GameObject targetenemy in targets)
                {
                    GameObject AttackEffect = Instantiate(AttackEffectPrefab, targetenemy.transform);
                    hitDamage += targetenemy.GetComponent<Enemy>().HpDamage(CardAbilities.Value);
                    if (CardAbilities.cardInfo != "NonCard")
                    {
                        artifactManager.ArtifactActiveOnCardAttack(CardAbilities.cardInfo, targetenemy.GetComponent<Enemy>(), hitDamage);
                    }
                }
            }
            else
            {
                Debug.Log("공격" + CardAbilities.Target + CardAbilities.Value + "데미지");
                List<GameObject> targets = new List<GameObject>(battleManager.EnemyObjectList);
                foreach (GameObject targetenemy in targets)
                {
                    GameObject AttackEffect = Instantiate(AttackEffectPrefab, targetenemy.transform);
                    hitDamage += targetenemy.GetComponent<Enemy>().Hit(CardAbilities.Value);
                    if (CardAbilities.cardInfo != "NonCard")
                    {
                        artifactManager.ArtifactActiveOnCardAttack(CardAbilities.cardInfo, targetenemy.GetComponent<Enemy>(), hitDamage);
                    }
                }
            }
        }
        return hitDamage;
    }
    public void GiveEffect(AbilityValues abilityValues)    //효과부여
    {
        Debug.Log("이빽토" + abilityValues.Content + abilityValues.Target + abilityValues.Value);
        if (abilityValues.Target == TargetTypes.Multi)
        {
            foreach (GameObject Enemy in BattleManager.GetComponent<BattleManager>().EnemyObjectList)
            {
                if (abilityValues.Content == "Bleeding")
                {
                    int i = 0;
                    while (Enemy.GetComponent<Enemy>().EffectDictionary.ContainsKey($"Bleeding/{i}"))
                    {
                        i += 1;
                    }
                    Enemy.GetComponent<Enemy>().AddEffect($"Bleeding/{i}", abilityValues.Value);
                }
                else
                {
                    Enemy.GetComponent<Enemy>().AddEffect(abilityValues.Content, abilityValues.Value);
                }
            }
        }
        else
        {
            if (abilityValues.Content == "Bleeding")
            {
                int i = 0;
                while (Targetenemy.GetComponent<Enemy>().EffectDictionary.ContainsKey($"Bleeding/{i}"))
                {
                    i += 1;
                }
                Targetenemy.GetComponent<Enemy>().AddEffect($"Bleeding/{i}", abilityValues.Value);
            }
            else
            {
                Targetenemy.GetComponent<Enemy>().AddEffect(abilityValues.Content, abilityValues.Value);
            }
        }
    }
    public void GiveManaGuard(AbilityValues CardAbilities) //마나가드 부여
    {
        BattleManager.GetComponent<BattleManager>().ManaGuard += CardAbilities.Value;
        Debug.Log("플레이어 마나가드 " + CardAbilities.Value + "획득");
        battleManager.HpBarSort();
    }
    public void GetStat(AbilityValues CardAbilities) //스탯 획득
    {
        Debug.Log("플레이어가" + CardAbilities.Content + " 스탯을 " + CardAbilities.Value + " 만큼 얻음");
        switch (CardAbilities.Content)
        {
            case "Hp":
                BattleManager.GetComponent<BattleManager>().Hp += CardAbilities.Value;
                battleManager.HpBarSort(); break;
            case "Gold":
                artifactManager.Gold += (int)CardAbilities.Value; battleManager.rewardManager.shopManager.CardGoldUpdate();
                artifactManager.ArtifactActiveOnGoldChanged(artifactManager.Gold); break;
        }
        ;
    }
}