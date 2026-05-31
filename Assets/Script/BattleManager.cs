using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }
    public List<Unit> UnitList;
    public List<Unit> PlayerUnitList;
    public Unit target;
    private int actionTime = 0;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void TurnSet(Unit unit)
    {
        unit.TurnStart();
    }
    public void NextTurn()
    {
        foreach (Unit unit in UnitList)
        {
            TurnSet(unit);
        }
    }
    public void TimeStop()
    {
        Time.timeScale = 0f;
    }
    public void TimeFlow()
    {
        Time.timeScale = 1;
    }
    public void BattleStart() 
    {
        StartCoroutine(WalkStart());
    }
    public IEnumerator TimeAction(int time) 
    {
        float timeSacle = Time.timeScale;
        actionTime += time;
        Time.timeScale *= 2;
        yield return new WaitForSeconds(time);
        Time.timeScale = timeSacle;
        
    }
    IEnumerator WalkStart()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (actionTime <= 0) { NextTurn(); Debug.Log("TimeFlow"); }
            actionTime -= 1;
        }
    }

    private object WaitForSeconds(float v)
    {
        throw new NotImplementedException();
    }

    private object WaitForSecondsRealtime(float v)
    {
        throw new NotImplementedException();
    }
}
