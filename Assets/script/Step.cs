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
                case ActionType.Quick:
                    //BattleManager.Instance.StartCoroutine(BattleManager.Instance.TimeAction(1));
                    //yield return new WaitForSeconds(1f);
                    //stair.unit.nowStair.ascend();
                    break;
                case ActionType.SingleAttack:
                    stair.target.Hit(float.Parse(action.value));
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
}
public class DummyStep : Step 
{
    public override void ascend()
    {
        stair.nowStep += 1;
        string[] contents = content.actions[0].value.Split("/");
        switch (contents[1]) 
        {
            case "None":
                break;
            case "STR":
                if (stair.unit.STRength > int.Parse(contents[2]))
                    stair.ascend();
                break;
            case "HP":
                if (stair.unit.maxHP > int.Parse(contents[2]))
                    stair.ascend();
                break;
            case "Dex":
                if (stair.unit.DEXterity > int.Parse(contents[2]))
                    stair.ascend();
                break;
            case "INT":
                if(stair.unit.INTelligence > int.Parse(contents[2]))
                    stair.ascend();
                break;
        }
    }
}