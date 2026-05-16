using UnityEngine;

public class PlayerUnit : Unit
{
    public float hp = 100;
    public Stair nowStair;
    BattleManager battleManager = BattleManager.Instance;
    public override void TurnStart() 
    {
        if (nowStair.ascend()) 
        {
            StairSelect();
            battleManager.TimeStop();
        }
    }
    public void StairSelect() 
    {
        //¹Ì¿Ï
        battleManager.TimeFlow();
    }
}
public abstract class Unit : MonoBehaviour
{
    public abstract void TurnStart();
}