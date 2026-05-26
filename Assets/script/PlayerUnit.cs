using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class PlayerUnit : Unit
{
    public List<string> stairDeck = new List<string>();
    public List<string> stairHand = new List<string>();
    public List<string> usedStair = new List<string>();
    public List<string> unUsedStair = new List<string>();
    public override void TurnStart()
    {
        base.TurnStart();
        HandDrow();
        StartCoroutine(Walk());
    }
    public IEnumerator Walk() 
    {
        if (nowStair == null)
        {
            StairSelect();
        }
        else
        {
            nowStair.ascend();
            while (quickMoveCount >= 10)
            {
                quickMoveCount -= 10;
                if (nowStair == null) { StairSelect(); break; }
                nowStair.ascend();
                BattleManager.Instance.StartCoroutine(BattleManager.Instance.TimeAction(1));
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private object WaitForSeconds(float v)
    {
        throw new System.NotImplementedException();
    }

    public override void StairSelect()
    {
        BattleManager.Instance.TimeStop();
        StairHandManager.Instance.HandSetting(this);
    }
    public void HandDrow()
    {
        while(stairHand.Count < INTelligence / 2) 
        {
            int unUsedStairCount = unUsedStair.Count;
            if (unUsedStairCount > 0) { stairHand.Add(unUsedStair[Random.Range(0, unUsedStairCount)]); }
            else if (HandSuffle())
            {
                stairHand.Add(unUsedStair[Random.Range(0, unUsedStairCount)]);
            }
            else { Debug.Log("모든 스킬 사용가능"); break; }
        }
    }
    public bool HandSuffle()
    {
        if (usedStair.Count > 0)
        {
            unUsedStair = new List<string>(usedStair);
            usedStair.Clear();
            return true;
        }
        return false;
    }
}
public abstract class Unit : MonoBehaviour
{
    public int team = 1;
    public int maxHP; //체력: 수치*10 만큼 최대체력 제공
    public int STRength; //근력: 수치*10% 만큼 피해량 제공
    public int DEXterity; //민첩: 턴마다 수치*0.1 만큼 추가 행동 제공
    public int INTelligence; //지능: 수치*0.5 만큼 계단 패 제공
    public float hp; //현제체력
    public int quickMoveCount = 0;
    public Stair nowStair = null;
    public HpBar hpBar;
    private void Awake()
    {
        hp = maxHP;
        hpBar.unit = this;
        hpBar.HpBarSetting();
    }
    public virtual void TurnStart()
    {
        quickMoveCount += DEXterity; //임시로 그냥 10넘으면 10깍고 행동하게 해놈 나중에 버프 형태로 수정
    }
    public abstract void StairSelect();
    private void OnMouseDown()
    {
        if (StairHandManager.Instance.target == this)
        {
            StairHandManager.Instance.StairSellected();
        }
        StairHandManager.Instance.TargetSelect(this);
        if (GameManager.Instance.quickBattle) 
        {
            StairHandManager.Instance.StairSellected();
        }
    }
    public void Hit(float damage) 
    {
        hp -= damage;
        hpBar.HpBarSetting();
    }
}