using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }
    public List<Unit> UnitList;
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
    public IEnumerator TimeStop() 
    {
        while (Time.timeScale > 0) 
        {
            Time.timeScale -= 0.1f;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
    public void TimeFlow() 
    {
        Time.timeScale = 1;
    }

}
