using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;

public class Step
{
    public Stair stair;
    public StepInfo content;
    public IEnumerator doActions() 
    {
        stair.nowStep += 1;
        foreach (StepAction action in content.actions)
        {
            switch (action.actionType) 
            {
                case ActionType.Dummy:
                    yield return new WaitForSeconds(float.Parse(action.value));
                    break;
                case ActionType.SingleAttack:
                    stair.target.Hit(float.Parse(action.value), stair.unit);
                    break;
                case ActionType.PlayAnimation:
                    stair.unit.GetComponent<Animator>().Play(action.value);
                    break;
                case ActionType.Move:
                    stair.unit.GetComponent<Rigidbody2D>().AddForce(new Vector2(float.Parse(action.value.Split("/")[0]) * stair.unit.team, float.Parse(action.value.Split("/")[1])));
                    break;
            }
        }
    }
    public virtual void ascend() 
    {
        BattleManager.Instance.StartCoroutine(doActions());
    }
    public void Quick(string condition)
    {
        string[] contents = condition.Split("/");

        switch (contents[0])
        {
            case "STR":
                if (stair.unit.STRength > int.Parse(contents[1]))
                    stair.unit.quickMoveCount += 10;
                    break;

            case "HP":
                if (stair.unit.CONstitution > int.Parse(contents[1]))
                    stair.unit.quickMoveCount += 10;
                break;

            case "Dex":
                if (stair.unit.DEXterity > int.Parse(contents[1]))
                    stair.unit.quickMoveCount += 10;
                break;

            case "INT":
                if (stair.unit.INTelligence > int.Parse(contents[1]))
                    stair.unit.quickMoveCount += 10;
                break;
        }
    }
}