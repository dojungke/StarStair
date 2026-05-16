using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireLizard : Enemy
{
    public int AttackCoolTime = 2;
    public string GetEffect;
    public float GetEffectNumber = 10f; // 꼬리 꼬기 마나가드 획득량
    public float AttackDamage = 10f; // 공격 피해량
    public IntentionButton IntentionCoolTimeButton;
    public IntentionButton IntentionAttackButton;
    public IntentionButton IntentionHealButton;
    public float HealHp;
    public Action Intention;
    public string GiveEffect;
    public int GiveEffectNumber;
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
        if (EffectDictionary.ContainsKey(GetEffect) == false)
        {
            AddEffect(GetEffect, GetEffectNumber);
        }
        Intention = EffectAttack;
        IntentionSetting();
    }
    public override void HpBarSort()
    {
        if (Hp > MaxHp) { Hp = MaxHp; }
        base.HpBarSort();
        if (Hp > MaxHp / 2) {  }
        else { Intention = Heal; }
        if (battleManager == null) { return; }
        IntentionSetting();
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
        if (GiveEffectNumber > 0) { BattleManager.GetComponent<BattleManager>().AddEffect(GiveEffect, GiveEffectNumber); }
        gameObject.GetComponent<Animator>().Play("Attack");
    }
    public void Heal()
    {
        intentionManager.gameObject.SetActive(false);
        intentionManager.CoolTime = AttackCoolTime;
        Debug.Log($"회복 발동 {HealHp} 회복");
        Intention = EffectAttack;
        Hp += HealHp; HpBarSort();
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
            //setting
            IntentionCoolTimeButton.ImageUpdate();
            IntentionAttackButton.text.text = AttackDamage.ToString();
        }
        else if (Intention == Heal)
        {
            //heal
            IntentionHealButton = Instantiate(battleManager.intentionButtonPrefab, intentionManager.transform).GetComponent<IntentionButton>();
            IntentionHealButton.owner = this;
            IntentionHealButton.type = ImageButtonType.Heal;
            intentionManager.intentionButtonList.Add(IntentionHealButton);
            IntentionHealButton.ImageUpdate();
            IntentionHealButton.text.text = HealHp.ToString();
        }
        IntentionCoolTimeButton.ImageUpdate();
        destroyflag = false;
    }
}