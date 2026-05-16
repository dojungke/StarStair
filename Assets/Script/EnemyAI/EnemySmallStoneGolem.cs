using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class EnemySmallStoneGolem : Enemy
{
    public int AttackCoolTime = 2;
    public string GetEffect;
    public float GetEffectNumber = 10f; // 꼬리 꼬기 마나가드 획득량
    public float AttackDamage = 10f; // 공격 피해량
    public float GetManaGuardCount;
    public IntentionButton IntentionCoolTimeButton;
    public IntentionButton IntentionAttackButton;
    public IntentionButton IntentionEffectButton;
    public IntentionButton IntentionGetManaGuradButton;
    public Action Intention;
    public string GiveEffect;
    public int GiveEffectNumber;
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
        Intention = GetManaGuard;
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
            if (ManaGuard > 50) { Intention = EffectAttack; }
            else { Intention = GetManaGuard; }
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
    public void GetManaGuard()
    {
        intentionManager.gameObject.SetActive(false);
        intentionManager.CoolTime = AttackCoolTime;
        Debug.Log($"보호막 획득 발동 {GetManaGuardCount} 획득");
        ManaGuard += GetManaGuardCount; HpBarSort();
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
        else if (Intention == GetManaGuard)
        {
            //heal
            IntentionGetManaGuradButton = Instantiate(battleManager.intentionButtonPrefab, intentionManager.transform).GetComponent<IntentionButton>();
            IntentionGetManaGuradButton.owner = this;
            IntentionGetManaGuradButton.type = ImageButtonType.GetManaGuard;
            intentionManager.intentionButtonList.Add(IntentionGetManaGuradButton);
            IntentionGetManaGuradButton.ImageUpdate();
            //setting
            IntentionGetManaGuradButton.ImageUpdate();
            IntentionGetManaGuradButton.text.text = GetManaGuardCount.ToString();
        }
        destroyflag = false;
        IntentionCoolTimeButton.ImageUpdate();
    }
}