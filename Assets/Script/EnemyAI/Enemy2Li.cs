using UnityEngine;

public class Enemy2Li : Enemy
{
    public int AttackCoolTime;
    public float AttackDamage = 15f; // 공격 피해량
    public IntentionButton IntentionCoolTimeButton;
    public IntentionButton IntentionAttackButton;
    public IntentionButton IntentionEffectButton;
    public string EffectName = "Bleeding"; // 부여할 효과
    public float EffectNumber = 2; //효과 부여량
    public EffectData effectData;
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
        IntentionAttackButton.ImageUpdate();
        //effect
        IntentionEffectButton = Instantiate(battleManager.intentionButtonPrefab, intentionManager.transform).GetComponent<IntentionButton>();
        IntentionEffectButton.owner = this;
        IntentionEffectButton.type = ImageButtonType.Effect;
        IntentionEffectButton.effectData = effectData;
        intentionManager.intentionButtonList.Add(IntentionAttackButton);
        IntentionEffectButton.ImageUpdate();
        //other
        IntentionSetting();
        intentionManager.CoolTime = AttackCoolTime;
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
            EffectAttack();
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
        Debug.Log($"효과부여 공격 발동 {AttackDamage} 피해 {EffectName} {EffectNumber} 부여");
        BattleManager.GetComponent<BattleManager>().Hit(AttackDamage);
        if (EffectName == "Bleeding")
        {
            int i = 0;
            while (BattleManager.GetComponent<BattleManager>().EffectDictionary.ContainsKey($"Bleeding/{i}"))
            {
                i += 1;
            }
            BattleManager.GetComponent<BattleManager>().AddEffect($"Bleeding/{i}", EffectNumber);
        }
        else
        {
            BattleManager.GetComponent<BattleManager>().AddEffect(EffectName, EffectNumber);
        }
        gameObject.GetComponent<Animator>().Play("Attack");
    }
    public void IntentionSetting()
    {
        intentionManager.gameObject.SetActive(true);
        IntentionCoolTimeButton.ImageUpdate();
        IntentionAttackButton.text.text = AttackDamage.ToString();
        IntentionEffectButton.text.text = EffectNumber.ToString();
        IntentionAttackButton.ImageUpdate();
    }
}