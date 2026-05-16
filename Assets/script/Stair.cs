using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    public Unit unit;
    public List<Step> steps;
    public int nowStep;
    public bool ascend()
    {
        steps[nowStep].ascend();
        nowStep += 1;
        if (nowStep >= steps.Count - 1) return true; //계단 종료
        return false;
    }
}