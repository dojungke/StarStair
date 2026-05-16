using UnityEngine;

public class EnemyFire2LiAI : Enemy
{
    public float FireAttackDamage = 6f; // 출혈 피해량
    public float BiteDamage = 10f; // 물기 피해량
    public override void TurnStart()
    {
        EffectActive(); // 효과 적용
        if (Hp < 0)
        {
            //StartCoroutine(EndTurnAfterDelay(0.5f));        //히아 내가 2초나 멈출수 있다!!!
            return;
        }
        ManaGuard = (int)Mathf.Floor(ManaGuard / 2f); // 턴 시작시 마나가드 감소, 소수점 버림
        if (BattleManager.GetComponent<BattleManager>().ManaGuard > 0)
        {
            Bite();
        }
        else
        {
            FireAttack();
        }
        //히아 내가 2초나 멈출수 있다!!!
        StartCoroutine(EndTurnAfterDelay(2f));
    }
    private System.Collections.IEnumerator EndTurnAfterDelay(float delaySeconds)    //시간을 멈춰라 마이 월드야!!!!!
    {
        yield return new WaitForSeconds(delaySeconds);
        TurnEnd();
    }
    public void FireAttack()
    {
        Debug.Log("화상 공격 발동");
        BattleManager.GetComponent<BattleManager>().Hit(FireAttackDamage);
        BattleManager.GetComponent<BattleManager>().AddEffect("Fire", 8);
        gameObject.GetComponent<Animator>().Play("Attack");
    }
    public void Bite()
    {
        Debug.Log("물기 발동");
        BattleManager.GetComponent<BattleManager>().Hit(BiteDamage);
        gameObject.GetComponent<Animator>().Play("Attack");
    }
}