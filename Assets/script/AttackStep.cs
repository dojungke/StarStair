using UnityEngine;

public class AttackStep : Step
{
    Rigidbody2D unitRigidbody;
    public void Awake()
    {
        unitRigidbody = stair.unit.GetComponent <Rigidbody2D>();
    }
    public override void ascend()
    {
        unitRigidbody.AddForce(new Vector2(1000, 50));
    }
}
