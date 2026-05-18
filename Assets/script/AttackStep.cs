using UnityEngine;

public class AttackStep : Step
{
    Rigidbody2D unitRigidbody;
    public override void ascend()
    {
        unitRigidbody = stair.unit.GetComponent<Rigidbody2D>();
        stair.nowStep += 1;
        unitRigidbody.AddForce(new Vector2(1000 * stair.unit.team, 50));
    }
}
