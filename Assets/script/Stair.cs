using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Stair
{
    public string stairName;
    public StairData data;
    public Unit unit;
    public List<Step> steps = new List<Step>();
    public int nowStep;
    public void ascend()
    {
        Debug.Log(steps[0].content);
        Debug.Log(steps[1].content);
        steps[nowStep].ascend();
        if (nowStep >= steps.Count) //계단 종료
        {
            Debug.Log("계단종료");
            unit.nowStair = null;
            steps.Clear();
            steps = null;
        }
    }
    public void stepSetting() 
    {
        data = (StairData)Resources.Load($"StairData/{stairName}");
        foreach (string step in data.steps) 
        {
            Type stepType = Type.GetType(step.Split("/")[0]);
            Step newStep = (Step)Activator.CreateInstance(stepType);
            newStep.stair = this;
            newStep.content = step;
            steps.Add(newStep);
        }
    }
}