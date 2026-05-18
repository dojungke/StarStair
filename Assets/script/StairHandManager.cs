using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class StairHandManager : MonoBehaviour
{
    public static StairHandManager Instance { get; private set; }
    public GameObject stairButton;
    public List<StairButton> stairButtons = new List<StairButton>();
    public Unit caller;
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
    public void HandSetting(PlayerUnit unit)
    {
        HandReset();
        BattleManager.Instance.TimeStop();
        caller = unit;
        foreach(var stair in unit.stairHand) 
        {
            StairButton newStairButton = GameObject.Instantiate(stairButton, gameObject.transform).GetComponent<StairButton>();
            newStairButton.stairName = stair;
            stairButtons.Add(newStairButton);
            newStairButton.buttonSetting();
        }
    }
    public void HandReset()
    {
        for(int i=0; i<stairButtons.Count;i++)
        {
            Destroy(stairButtons[i].gameObject);
        }
        stairButtons.Clear();
    }
    public void StairSellected(string stair) 
    {
        BattleManager.Instance.TimeFlow();
        HandReset();
        Stair newStair = new Stair();
        newStair.stairName = stair;
        newStair.stepSetting();
        newStair.unit = caller;
        caller.nowStair = newStair;
        newStair.ascend();
    }
}
