using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : Unit
{
    public GameObject HpBarSlider;
    public GameObject HpBarText;
    public GameObject ManaGuardImage;
    public GameObject ManaGuardText;
    public GameObject NameText;
    public GameObject EffectViewManager;
    public GameObject BattleManager; //BattleManager를 찾지 못했을 때를 대비하여 Start에서 찾음
    public BattleManager battleManager;
    public IntentionManager intentionManager;
    public bool FallingDown = false; //적이 쓰러졌는지 여부
    public abstract void TurnStart();

    private void OnDestroy()
    {
        if(battleManager.turnObject == gameObject) { NextTurn(); }
    }
    public override void Awake()
    {
        base.Awake();
        base.Name = Name;
        HpBarSort();
        if (BattleManager == null)
        {
            BattleManager = GameObject.Find("BattleManager");
        }
        battleManager = BattleManager.GetComponent<BattleManager>();
        BattleManager.GetComponent<BattleManager>().EnemyAdd(this.gameObject);
        NameText.GetComponent<TextMeshProUGUI>().text = Name;
        BattleManager.GetComponent<BattleManager>().EnemyManager.GetComponent<EnemyManger>().EnemySort();
    }
    public override void HpBarSort()
    {
        if (Hp <= 0)
        {
            if (Hp > MaxHp) { Hp = MaxHp; }
            if (FallingDown) return; //이미 쓰러진 상태라면 중복 실행 방지
            FallingDown = true;
            Debug.Log("적 " + Name + " 쓰러짐");
            HpBarText.GetComponent<TextMeshProUGUI>().text = "쓰러짐";
            ManaGuardImage.SetActive(false);
            BattleManager.GetComponent<BattleManager>().EnemyManager.GetComponent<EnemyManger>().RemoveEnemy(this.gameObject);
        }
        else
        {
            HpBarSlider.GetComponent<Slider>().value = Hp / MaxHp;
            HpBarText.GetComponent<TextMeshProUGUI>().text = Hp + "/" + MaxHp;
            if (ManaGuard > 0)
            {
                ManaGuardImage.SetActive(true);
                ManaGuardText.GetComponent<TextMeshProUGUI>().text = ManaGuard.ToString();
                HpBarSlider.GetComponent<Slider>().fillRect.GetComponent<Image>().color = new Color32(90, 215, 200, 255);
            }
            else
            {
                ManaGuardImage.SetActive(false);
                HpBarSlider.GetComponent<Slider>().fillRect.GetComponent<Image>().color = new Color32(240, 36, 36, 255);
            }
        }
    }
    public void NextTurn()
    {

    }
    public override void TurnEnd(int times = 1)
    {
        BattleManager.GetComponent<BattleManager>().NextTurn();
    }
    public override void AddEffect(string Effect, float Count)
    {
        if (EffectDictionary.ContainsKey(Effect))
        {
            EffectDictionary[Effect] += Count;
            EffectViewManager.GetComponent<EnemyEffectViewManager>().AddEffectView(Effect, Count);
        }
        else
        {
            EffectDictionary.Add(Effect, Count);
            EffectViewManager.GetComponent<EnemyEffectViewManager>().AddEffectView(Effect, Count);
        }
    }
    public override void RemoveEffect(string Effect)
    {
        if (EffectDictionary.ContainsKey(Effect))
        {
            EffectDictionary.Remove(Effect);
            EffectViewManager.GetComponent<EnemyEffectViewManager>().RemoveEffectView(Effect);
        }
        else
        {
            Debug.Log("에러: 제거할 효과가 없음");
        }
    }
}