using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLostOfBeast : Enemy
{
    public int AttackCoolTime = 2;
    public string GetEffect;
    public float GetEffectNumber = 10f; // 꼬리 꼬기 마나가드 획득량
    public float AttackDamage = 10f; // 공격 피해량
    public IntentionButton IntentionCoolTimeButton;
    public IntentionButton IntentionAttackButton;
    public IntentionButton IntentionSummonButton;
    public IntentionButton IntentionEffectButton;
    public float HealHp;
    public Action Intention;
    public string GiveEffect;
    public int GiveEffectNumber;
    public string SummonEnemy;
    public EffectData effectData;
    bool destroyflag = false;
    void Start()
    {
        //cooltime
        IntentionCoolTimeButton = Instantiate(battleManager.intentionButtonPrefab, intentionManager.transform).GetComponent<IntentionButton>();
        IntentionCoolTimeButton.owner = this;
        IntentionCoolTimeButton.type = ImageButtonType.IntentionCooltime;
        intentionManager.intentionButtonList.Add(IntentionCoolTimeButton);
        IntentionCoolTimeButton.ImageUpdate();
        //others
        intentionManager.CoolTime = AttackCoolTime;
        Intention = EffectAttack;
        IntentionSetting();
        if (EffectDictionary.ContainsKey(GetEffect) == false)
        {
            AddEffect(GetEffect, GetEffectNumber);
        }
    }
    public override void HpBarSort()
    {
        if (Hp > MaxHp) { Hp = MaxHp; }
        base.HpBarSort();
    }
    public override void TurnStart()
    {
        EffectActive(); // 효과 적용
        if (Hp <= 0)
        {
            //StartCoroutine(EndTurnAfterDelay(0.5f));        //히아 내가 2초나 멈출수 있다!!!
            return;
        }
        intentionManager.CoolTime -= 1;
        IntentionSetting();
        if (intentionManager.CoolTime <= 0)
        {
            Intention();
            if (BattleManager.GetComponent<BattleManager>().EnemyObjectList.Count < 2) { Intention = Summon; }
            else { Intention = EffectAttack; }
            StartCoroutine(EndTurnAfterDelay(2f));
        }
        else
        {
            StartCoroutine(EndTurnAfterDelay(0.5f));        //히아 내가 2초나 멈출수 있다!!!
        }
    }
    private System.Collections.IEnumerator EndTurnAfterDelay(float delaySeconds)    //시간을 멈춰라 마이 월드야!!!!!
    {
        yield return new WaitForSeconds(delaySeconds);
        IntentionSetting();
        TurnEnd();
    }
    public void EffectAttack()
    {
        intentionManager.gameObject.SetActive(false);
        intentionManager.CoolTime = AttackCoolTime;
        Debug.Log($"일반 공격 발동 {AttackDamage} 피해");
        BattleManager.GetComponent<BattleManager>().Hit(AttackDamage);
        BattleManager.GetComponent<BattleManager>().AddEffect(GiveEffect, GiveEffectNumber);
        gameObject.GetComponent<Animator>().Play("Attack");
    }
    public void Summon()
    {
        intentionManager.gameObject.SetActive(false);
        intentionManager.CoolTime = AttackCoolTime;
        Debug.Log($"소환 발동 {SummonEnemy} 소환!");
        BattleManager.GetComponent<BattleManager>().EnemyManager.GetComponent<EnemyManger>().EnemyAdd(SummonEnemy);
        gameObject.GetComponent<Animator>().Play("Attack");
    }
    public void IntentionSetting()
    {
        if (destroyflag == false)
        {
            destroyflag = true;
            for (int i = 1; i < intentionManager.intentionButtonList.Count; i++) { intentionManager.intentionButtonList[i].DestroySelf(); }
        }
        intentionManager.gameObject.SetActive(true);
        List<IntentionButton> tempList = new(intentionManager.intentionButtonList);
        if (Intention == EffectAttack)
        {
            //attack
            IntentionAttackButton = Instantiate(battleManager.intentionButtonPrefab, intentionManager.transform).GetComponent<IntentionButton>();
            IntentionAttackButton.owner = this;
            IntentionAttackButton.type = ImageButtonType.Attack;
            intentionManager.intentionButtonList.Add(IntentionAttackButton);
            IntentionAttackButton.ImageUpdate();
            //effect
            IntentionEffectButton = Instantiate(battleManager.intentionButtonPrefab, intentionManager.transform).GetComponent<IntentionButton>();
            IntentionEffectButton.owner = this;
            IntentionEffectButton.type = ImageButtonType.Effect;
            IntentionEffectButton.effectData = effectData;
            intentionManager.intentionButtonList.Add(IntentionEffectButton);
            IntentionEffectButton.text.text = GiveEffectNumber.ToString();
            IntentionEffectButton.ImageUpdate();
            //setting
            IntentionCoolTimeButton.ImageUpdate();
            IntentionAttackButton.text.text = AttackDamage.ToString();
        }
        else if (Intention == Summon)
        {
            //heal
            IntentionSummonButton = Instantiate(battleManager.intentionButtonPrefab, intentionManager.transform).GetComponent<IntentionButton>();
            IntentionSummonButton.owner = this;
            IntentionSummonButton.type = ImageButtonType.Summon;
            intentionManager.intentionButtonList.Add(IntentionSummonButton);
            IntentionSummonButton.ImageUpdate();
            //setting
            IntentionSummonButton.ImageUpdate();
            IntentionSummonButton.text.text = SummonEnemy;
        }
        destroyflag = false;
        IntentionCoolTimeButton.ImageUpdate();
    }
}