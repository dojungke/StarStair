using TMPro;
using UnityEngine;

public class EnemyRat : Enemy
{
    public int AttackCoolTime;
    public float AttackDamage = 15f; // 공격 피해량
    public IntentionButton IntentionCoolTimeButton;
    public IntentionButton IntentionAttackButton;
    private void Start()
    {
        //cooltime
        IntentionCoolTimeButton = Instantiate(battleManager.intentionButtonPrefab, intentionManager.transform).GetComponent<IntentionButton>();
        IntentionCoolTimeButton.owner = this;
        IntentionCoolTimeButton.type = ImageButtonType.IntentionCooltime;
        intentionManager.intentionButtonList.Add(IntentionCoolTimeButton);
        IntentionCoolTimeButton.ImageUpdate();
        //attack
        IntentionAttackButton = Instantiate(battleManager.intentionButtonPrefab, intentionManager.transform).GetComponent<IntentionButton>();
        IntentionAttackButton.owner = this;
        IntentionAttackButton.type = ImageButtonType.Attack;
        intentionManager.intentionButtonList.Add(IntentionAttackButton);
        intentionManager.CoolTime = AttackCoolTime;
        IntentionAttackButton.ImageUpdate();
        //other
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
            Attack();
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
    public void Attack()
    {
        intentionManager.gameObject.SetActive(false);
        intentionManager.CoolTime = AttackCoolTime;
        Debug.Log($"일반 공격 발동 {AttackDamage} 피해");
        BattleManager.GetComponent<BattleManager>().Hit(AttackDamage);
        gameObject.GetComponent<Animator>().Play("Attack");
    }
    public void IntentionSetting()
    {
        intentionManager.gameObject.SetActive(true);
        IntentionCoolTimeButton.ImageUpdate();
        IntentionAttackButton.text.text = AttackDamage.ToString();
        IntentionAttackButton.ImageUpdate();
    }
}