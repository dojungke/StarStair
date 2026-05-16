using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static StageManager;
public class BattleManager : Unit
{
    public int Gold;
    public List<GameObject> EnemyObjectList;
    public GameObject TurnObject;
    public GameObject HandManager;
    public Hand handManager;
    public Enemy turnObject;
    public List<GameObject> BattleUnitList;
    public Vector4 Turn;
    public GameObject HpBarSlider;
    public GameObject HpBarText;
    public GameObject ManaGuardImage;
    public GameObject ManaGuardText;
    public GameObject EffectViewManager;
    public GameObject EnemyManager;
    public StageManager stageManager;
    public GameObject BattleCanvas;
    public SkillCostViewerManager skillCostViewerManager;
    public RewardManager rewardManager;
    public GameObject HitPrefab;
    public GameObject gameOverCanvas;
    public UnityEvent onGameOver;
    public GameObject turnEndButton;
    public GameObject intentionButtonPrefab;
    public bool isGameOver = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BattleUnitList.Insert(0, this.gameObject);
        handManager = HandManager.GetComponent<Hand>();
        HpBarSort();
    }
    public override float Hit(float Damage)
    {
        GameObject HitEffect = Instantiate(HitPrefab, BattleCanvas.transform);
        Destroy(HitEffect, 0.1F);
        return base.Hit(Damage);
    }
    public void BattleStart()
    {
        stageManager.NowStage = StageType.Battle;
        handManager.BattleCardCanvas.gameObject.SetActive(true);
        BattleCanvas.SetActive(true);
        handManager.BattleStart();
        Turn = new Vector4(0, 0, 0, 0);
        TurnObject = this.gameObject;
        turnEndButton.SetActive(true);
    }
    public void TurnSet(GameObject TurnObject)
    {
        if (Hp <= 0)
        {
            Debug.Log("에러: 플레이어가 쓰러짐");
            HpBarSort();
            return;
        }
        if (TurnObject == null)
        {
            Debug.Log("에러: 턴 잡은놈 없음");
        }
        else if (TurnObject == gameObject)
        {
            EffectActive();
            handManager.TurnStart();
            Turn.x += 1;
            //ManaGuard = (int)Math.Floor(ManaGuard / 2F); //턴 시작시 마나가드감소, 소수점 버림
            HpBarSort();
            turnEndButton.SetActive(true);
            handManager.artifactManager.ArtifactActiveOnTurnStart();
            for(int i = 0; i <= skillCostViewerManager.SkillCostViewers.Count; i++)
            {
                if(skillCostViewerManager.SkillCostViewers.Count >= 0 && skillCostViewerManager.SkillCostViewers.Count < skillCostViewerManager.SkillCostViewers.Count)
                skillCostViewerManager.SkillCoolTimeAdd(skillCostViewerManager.SkillCostViewers[i]);
            }
            handManager.SortHand(0);
        }
        else
        {
            turnObject = TurnObject.GetComponent<Enemy>();
            turnObject.TurnStart();
        }
    }
    public void NextTurn()
    {
        if (BattleUnitList.Count - 1 > Turn.y)
        {
            Turn.y += 1;
            TurnSet(BattleUnitList[(int)Turn.y]);
        }
        else
        {
            Turn.y = 0;
            TurnSet(gameObject);
        }
    }
    public override void TurnEnd(int times = 1)
    {
        if(times > 1) { AddEffect("Stun", times - 1); }
        if (true)
        {
            //if (EffectDictionary.ContainsKey("Bind") == false && bindActive) { bindActive = false; handManager.NumberOfTurnStartDorwHandCard += 1; }
            Debug.Log("턴 종료");
            NextTurn();
            handManager.NowCardChange(handManager.NowCard, true);
            turnEndButton.SetActive(false);
            handManager.artifactManager.ArtifactActiveOnTurnEnd();
            if (skillCostViewerManager.SkillCostViewers == null) return;
        }
        else
        {
            Debug.Log("플레이어 턴 아닌데 턴종료 시도됨");
        }
        BattleCanvas.SetActive(true);
    }
    public void EnemyAdd(GameObject NewEnemy)
    {
        EnemyObjectList.Add(NewEnemy);
        BattleUnitList.Add(NewEnemy);
    }
    public override void HpBarSort()
    {
        if (Hp <= 0)
        {
            gameOverCanvas.SetActive(true);
            HpBarText.GetComponent<TextMeshProUGUI>().text = "쓰러짐";
            HpBarSlider.GetComponent<Slider>().value = 0;
        }
        if (Hp >= MaxHp)
        {
            Hp = MaxHp;
        }
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
    public override void AddEffect(string Effect, float Count)
    {
        if (EffectDictionary.ContainsKey(Effect))
        {
            EffectDictionary[Effect] += Count;
            EffectViewManager.GetComponent<EffectViewManager>().AddEffectView(Effect, Count);
        }
        else
        {
            EffectDictionary.Add(Effect, Count);
            EffectViewManager.GetComponent<EffectViewManager>().AddEffectView(Effect, Count);
        }
    }
    public override void RemoveEffect(string Effect)
    {
        if (EffectDictionary.ContainsKey(Effect))
        {
            EffectDictionary.Remove(Effect);
            EffectViewManager.GetComponent<EffectViewManager>().RemoveEffectView(Effect);
        }
        else
        {
            Debug.Log("에러: 제거할 효과가 없음");
        }
    }
    public void BattleEnd()    //전투 종료 처리
    {
        //if (EffectDictionary.ContainsKey("Bind") == false && bindActive) { bindActive = false; handManager.NumberOfTurnStartDorwHandCard += 1; }
        foreach (GameObject handcard in handManager.HandCard)
        {
            BattleUnitList.Remove(handcard);
            Destroy(handcard);
        }
        handManager.artifactManager.ArtifactActiveOnBattleEnd();
        EnemyObjectList.Clear();
        handManager.UsedCardGroup.Clear();
        handManager.UnUsedCardGroup.Clear();
        handManager.HandCard.Clear();
        handManager.BattleCardCanvas.gameObject.SetActive(false);
        BattleCanvas.SetActive(false);
        List<string> effectKeys = new List<string>(EffectDictionary.Keys);
        foreach (string effect in effectKeys)
        {
            RemoveEffect(effect); // 모든 효과 제거
        }
        ManaGuard = 0; // 전투 종료시 마나가드 초기화
        skillCostViewerManager.SkillCostViewerClear();
        HpBarSort();
        int rewardRamdom = UnityEngine.Random.Range(50, 151);
        if (stageManager.stageType == "Battle")
        {
            rewardManager.RewardCardAdd("Stat", $"Gold/{rewardRamdom}");
            //if (rewardRamdom < 20)
            //{
            //    rewardManager.RewardCardAdd("Artifact", "0/3");
            //}
            rewardManager.RewardText.GetComponent<TextMeshProUGUI>().text = "전투에서 승리했습니다. 보상으로 별빛을 획득합니다.";
            rewardManager.RewardGetStart("battleWin");
        }
        else if (stageManager.stageType == "EliteBattle")
        {
            rewardManager.RewardCardAdd("Stat", $"Gold/{rewardRamdom * 2 + 100}");
            rewardManager.RewardText.GetComponent<TextMeshProUGUI>().text = "전투에서 승리했습니다. 보상으로 별빛을 획득합니다.";
            rewardManager.RewardGetStart("battleWin");
        }
        else if (stageManager.stageType == "BossBattle")
        {
            if (stageManager.levelNumber >= stageManager.LevelList.Count - 1)
            {
                stageManager.EndCanvas.SetActive(true);
                return;
            }
            else
            {
                stageManager.levelNumber += 1;
                stageManager.level = stageManager.LevelList[stageManager.levelNumber];
            }
            rewardManager.RewardCardAdd("Stat", $"Gold/{rewardRamdom + 500}");
            rewardManager.RewardText.GetComponent<TextMeshProUGUI>().text = "힘겨운 싸움을 끝내니 별빛 가득한 밤이 찾아왔습니다.";
            rewardManager.RewardGetStart("BossBattleWin");
            stageManager.artifactManager.maxArtifact += 1;
            Debug.Log($"현레밸{stageManager.levelNumber},{stageManager.level} 래밸수{stageManager.LevelList.Count}");
        }
        else
        {
            Debug.Log("에러: 잘못된 스테이지 타입");
        }
    }
    public void GameOver()
    {
        Debug.Log("플레이어 쓰러짐");
        if (isGameOver) return; //중복 실행 방지
        isGameOver = true;
        foreach (GameObject handcard in handManager.HandCard)
        {
            BattleUnitList.Remove(handcard);
            Destroy(handcard);
        }
        handManager.UsedCardGroup.Clear();
        handManager.UnUsedCardGroup.Clear();
        handManager.HandCard.Clear();
        handManager.BattleCardCanvas.gameObject.SetActive(false);
        BattleCanvas.SetActive(false);
        List<string> effectKeys = new List<string>(EffectDictionary.Keys);
        foreach (string effect in effectKeys)
        {
            RemoveEffect(effect); // 모든 효과 제거
        }
        ManaGuard = 0; // 게임 종료시 마나가드 초기화
        List<GameObject> enemyObjectList = new List<GameObject>(EnemyObjectList);
        foreach (GameObject enemy in enemyObjectList)
        {
            Destroy(enemy);
        }
        BattleUnitList = new List<GameObject>
        {
            this.gameObject
        };
        EnemyObjectList = new List<GameObject>();
        skillCostViewerManager.SkillCostViewerClear();
        gameOverCanvas.SetActive(false);
        stageManager.EndCanvas.SetActive(false);
        stageManager.PlayStart(); //게임오버 재시작 일부 처리
        handManager.artifactManager.GameEnd();
        onGameOver.Invoke();
        // 체력 회복을 비롯한 일부는 BattleEnd()에서 처리
    }
}