using TMPro;
using UnityEngine;

public class EnemyAmurRatSnake : Enemy
{
    public int AttackCoolTime = 2;
    public float Scales = 10f; // 꼬리 꼬기 마나가드 획득량
    public float AttackDamage = 10f; // 공격 피해량
    public IntentionButton IntentionCoolTimeButton;
    public IntentionButton IntentionAttackButton;
    void Start()
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
        if (EffectDictionary.ContainsKey("Scales") == false)
        {
            AddEffect("Scales", Scales);
        }
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