using System.Collections;
using UnityEngine;

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
        newStair.target = BattleManager.Instance.PlayerUnitList[0];

        StairInfoButton.gameObject.SetActive(true);
        StairInfoButton.stairName = nowStair.stairName;
        StairInfoButton.nowStep = 0;
        StairInfoButton.ButtonSetting();
    }
    public override void TurnStart()
    {
        base.TurnStart();
        StartCoroutine(Walk());
    }
}
