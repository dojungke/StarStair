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
    public Unit target;
    public void ascend()
    {
        if (nowStep >= steps.Count) //계단 종료
        {
            unit.StairEnd();
            Debug.Log("계단종료");
            unit.nowStair = null;
            steps.Clear();
            steps = null;
            return;
        }
        steps[nowStep].Quick(steps[nowStep].content.quick);
        Debug.Log($"{steps[nowStep].content.stepName}, {unit.name} -> {target.name}");
        steps[nowStep].ascend();
        unit.StairInfoButton.nowStep = nowStep;
        unit.StairInfoButton.StepInfo(0);
    }
    public void stepSetting() 
    {
        data = (StairData)Resources.Load($"StairData/{stairName}");
        foreach (StepInfo step in data.StepInfo) 
        {
            Type stepType = Type.GetType("Step");
            Step newStep = (Step)Activator.CreateInstance(stepType);
            newStep.stair = this;
            newStep.content = step;
            steps.Add(newStep);
        }
    }
}