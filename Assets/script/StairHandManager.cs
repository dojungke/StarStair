using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class StairHandManager : MonoBehaviour
{
    public static StairHandManager Instance { get; private set; }
    public GameObject stairButton;
    public GameObject targetMakerPrefab;
    public List<StairButton> stairButtons = new List<StairButton>();
    public PlayerUnit caller;
    public Unit target;
    public string selectedStair = "";
    public TextMeshProUGUI targetSelectAnounceText;
    private GameObject targetMaker;
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
    public void StairSelect(string stair) 
    {
        selectedStair = stair;
        HandReset();
        targetSelectAnounceText.gameObject.SetActive(true);
    }
    public void TargetSelect(Unit unit) 
    {
        if (selectedStair == "")
        {
            Debug.Log("지정된 사다리 없음");
            return;
        }
        target = unit;
        targetMaker = Instantiate(targetMakerPrefab, target.transform);
    }
    public void StairSellected() 
    {
        if (selectedStair == "") 
        { 
            Debug.Log("지정된 사다리 없음");
            return;
        }
        Debug.Log(selectedStair);
        BattleManager.Instance.TimeFlow();
        HandReset();
        Stair newStair = new Stair();
        newStair.stairName = selectedStair;
        newStair.target = target;
        newStair.stepSetting();
        newStair.unit = caller;
        caller.nowStair = newStair;
        caller.StartCoroutine(caller.Walk());
        caller.stairDeck.Remove(selectedStair);
        selectedStair = "";
        Destroy(targetMaker);
        target = null;
        targetSelectAnounceText.gameObject.SetActive(false);
    }
}
