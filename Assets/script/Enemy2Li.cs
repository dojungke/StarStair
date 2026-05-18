using UnityEngine;
using UnityEngine.UIElements;

public class Enemy2Li : Unit
{
    public override void StairSelect()
    {
        string stair = "SwordAttack";
        Stair newStair = new Stair();
        newStair.stairName = stair;
        newStair.stepSetting();
        newStair.unit = this;
        nowStair = newStair;
        newStair.ascend();
    }
    public override void TurnStart()
    {
        base.TurnStart();
        if (nowStair == null)
        {
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
        }
    }
}
