using UnityEngine;

public abstract class Step : MonoBehaviour
{
    public Stair stair;
    public abstract void ascend();
}