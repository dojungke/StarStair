using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    public List<string> stairHand = new List<string>();
    public List<string> usedStair = new List<string>();
    public List<string> unUsedStair = new List<string>();
    public override void TurnStart()
    {
        base.TurnStart();
        if (nowStair == null)
        {
            HandDrow();
            StairSelect();
        }
        else
        {
            nowStair.ascend();
            while (quickMoveCount >= 10)
            {
                nowStair.ascend();
                quickMoveCount -= 10;
            }
            //TurnStart();
        }
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
    public int maxHP; //체력: 수치*10 만큼 최대체력 제공
    public int STRength; //근력: 수치*10% 만큼 피해량 제공
    public int DEXterity; //민첩: 턴마다 수치*0.1 만큼 추가 행동 제공
    public int INTelligence; //지능: 수치*0.5 만큼 계단 패 제공
    public float hp; //현제체력
    public int quickMoveCount = 0;
    public Stair nowStair = null;
    private void Awake()
    {
        hp = maxHP * 10;
    }
    public virtual void TurnStart()
    {
        quickMoveCount += DEXterity; //임시로 그냥 10넘으면 10깍고 행동하게 해놈 나중에 버프 형태로 수정
    }
    public abstract void StairSelect();
}