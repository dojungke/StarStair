using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public int difficulty = 0;
    public GameObject stageCanvas; // 인스펙터에서 할당할 스테이지 캔버스 오브젝트
    public string level = "forest"; // 현제 레벨
    public int levelNumber = 0;
    public string stageType; // 현제 스테이지 종류 (예: 전투, 사건, 보스 등)
    public List<string> stageHand;
    public List<GameObject> stageHandObject; // 스테이지 덱 오브젝트 리스트
    public List<string> stageUsedCardGroup; // 스테이지에서 사용된 카드 더미
    public List<string> stageUnUsedCardGroup; // 스테이지에서 사용되지 않은 카드 그룹
    public List<string> stageDeck;
    public GameObject stageCardPrefab; // 스테이지 카드 프리팹
    public UsedStageCardDummy stageUsedCardDummy; // 스테이지 카드 더미 오브젝트
    public GameObject stageCardDummy; // 스테이지 카드 더미 오브젝트
    public float Interval; // 카드 간격
    public float RotationInterval; // 카드 회전 간격
    public float HorizonInterval; // 카드 수평 간격
    public float widthRatio = 1f; // 화면 너비 비율
    public float heightRatio = 1f; // 화면 높이 비율
    public int MaxStageHandCard; // 스테이지에서 최대 핸드 카드 수
    public int NumberOfStageStartDrawHandCard; // 스테이지 시작 시 드로우할 카드 수
    public string NowStageCard; // 현재 스테이지 카드
    public GameObject SelectedStageCard; // 선택된 스테이지 카드
    public string ConsecutiveNumbers; // 연속 숫자 여부
    public bool SameNumber; // 같은 숫자 여부
    public bool SameColor; // 같은 색상 여부
    public EnemyManger enemyManger; // 적 매니저
    public CardGetManager cardGetManager; // 카드 획득 매니저
    public TextMeshProUGUI stageText; // 스테이지 텍스트 UI
    public BattleManager battleManager; // 전투 매니저
    public RewardManager rewardManager; // 보상 매니저
    public EventManager eventManager; // 이벤트 매니저
    public ArtifactManager artifactManager; // 유물 매니저
    public GameObject EndCanvas; // 스테이지 카드 이미지 오브젝트
    public GameObject StageCardCanvas;
    public List<string> startDeck;
    public List<string> LevelList = new List<string>  (); //인스팩터에서 할당
    public List<Sprite> backGroundImageList;
    public Image backgroundImage;
    public enum StageType
    {
        Start,
        StageSelect,
        Battle,
        Reward,
        Event,
        GetCard,
    }
    public StageType NowStage;

    public void Start()
    {
        battleManager.onGameOver.AddListener(GameOver);

        // 초기화 작업
        stageHandObject = new List<GameObject>();
        stageUsedCardGroup = new List<string>();
        stageUnUsedCardGroup = new List<string>();

    }
    public void StageCardSort()
    {
        int IndexCounter = 0;
        foreach (GameObject Cards in stageHandObject)
        {
            IndexCounter = IndexCounter + 1;
            Cards.transform.localPosition = new Vector3(Interval * widthRatio * (IndexCounter - (stageHandObject.Count / 2F) + 0.5F), -Mathf.Abs(HorizonInterval * (IndexCounter - (stageHandObject.Count / 2f) - 0.5f) * heightRatio) - 5, IndexCounter * -0.1F);
            Cards.transform.rotation = Quaternion.Euler(0, 0, RotationInterval * (IndexCounter - (stageHandObject.Count / 2F) - 0.5F));
        }
    }
    public void AddCard(GameObject New)
    {
        stageHandObject.Add(New);
        stageHand.Add(New.GetComponent<StageCard>().Name + "/" + New.GetComponent<StageCard>().Color + "/" + New.GetComponent<StageCard>().Number);
        StageCardSort();
    }
    public void Shuffle()
    {
        stageUnUsedCardGroup.AddRange(stageUsedCardGroup);
        stageUsedCardGroup.Clear();
        Debug.Log("스테이지 덱 셔플");
    }
    public void Drow(int DrowNumber = 1, string DrowType = "일반")
    {
        if (stageHand.Count >= MaxStageHandCard)
        {
            Debug.Log("손패 최대수 초과");
        }
        else
        {
            if (stageUnUsedCardGroup.Count < 1)
            {
                BossStageDrow();
            }
            while (DrowNumber >= 1)
            {
                if (stageUnUsedCardGroup.Count < 1) { BossStageDrow(); }
                if (stageUnUsedCardGroup.Count < 1) { return; }
                int DrowCard = UnityEngine.Random.Range(0, stageUnUsedCardGroup.Count);
                string[] DrowCard2 = stageUnUsedCardGroup[DrowCard].Split('/');
                GameObject newcard = Instantiate(stageCardPrefab, Vector3.zero, Quaternion.identity, stageCardDummy.transform);
                Debug.Log("드로우 " + DrowCard2[0] + " 색상:" + DrowCard2[1] + " 숫자:" + DrowCard2[2]);
                newcard.GetComponent<StageCard>().Name = DrowCard2[0];
                newcard.GetComponent<StageCard>().Color = DrowCard2[1];
                newcard.GetComponent<StageCard>().Number = int.Parse(DrowCard2[2]);
                newcard.GetComponent<StageCard>().stageManager = this;
                AddCard(newcard);

                Debug.Log("카드" + stageUnUsedCardGroup[DrowCard] + DrowType);
                stageUnUsedCardGroup.Remove(stageUnUsedCardGroup[DrowCard]);
                DrowNumber -= 1;
            }
        }
    }
    public void BossStageDrow()
    {
        foreach (GameObject OneOfCard in stageHandObject)
        {
            Destroy(OneOfCard);
        }

        stageHandObject.Clear();
        stageHand.Clear();
        stageUnUsedCardGroup.Clear();
        stageUsedCardGroup.Clear();
        stageUnUsedCardGroup.Add("BossBattle/S/6");
        Drow(1, "보스 스테이지 드로우");
        stageText.text = "이번 이야기의 꼬리를 향해가자";
        ConsecutiveNumbers = "true";
        SameNumber = true;
        SameColor = true;
        NowStageCardChange("BossBattle/T/6");
    }
    public void NowStageCardChange(string NewNowCard)
    {
        Debug.Log("현재 카드 변화");
        //일반적인 상황에는 카드를 받아서 봐꿔요.
        NowStageCard = NewNowCard;
        stageUsedCardDummy.NowStageCardChange(NewNowCard);
        foreach (GameObject OneOfCard in stageHandObject)
        {
            OneOfCard.GetComponent<StageCard>().NowCardChange();
        }
    }
    public void NowStageCardChangeToTheCardYouCanUseNow()
    {
        //사용 가능하도록 카드 호출.
        int StartCardRandomNumber = UnityEngine.Random.Range(0, stageHand.Count);
        string StartCardColor = stageHandObject[StartCardRandomNumber].GetComponent<StageCard>().Color;
        StartCardRandomNumber = UnityEngine.Random.Range(1, 7);
        NowStageCardChange("Start" + "/" + StartCardColor + "/" + StartCardRandomNumber);
        Debug.Log("Start" + "/" + StartCardColor + "/" + StartCardRandomNumber);
    }
    public void PlayStart()
    {
        Debug.Log("게임 시작");
        level = "forest";
        backgroundImage.sprite = backGroundImageList[0];
        levelNumber = 0;
        artifactManager.maxArtifact = 4;
        battleManager.HandManager.GetComponent<Hand>().Deck = new List<string>(startDeck);
        battleManager.Hp = battleManager.MaxHp;
        battleManager.isGameOver = false;
        battleManager.HpBarSort();
        LevelStart();
    }
    public void LevelStart()
    {
        artifactManager.artifactCountText.text = $"{artifactManager.artifactCount}/{artifactManager.maxArtifact}";
        if (level == "forest") { backgroundImage.sprite = backGroundImageList[0]; }
        else if (level == "mountain") { backgroundImage.sprite = backGroundImageList[1]; }
        else if (level == "peak") { backgroundImage.sprite = backGroundImageList[2]; }
        if (difficulty >= 1) { battleManager.Hp += battleManager.MaxHp / 2; }
        else { battleManager.Hp = battleManager.MaxHp; }
        battleManager.HpBarSort();
        stageUnUsedCardGroup = new List<string>(stageDeck);
        stageUsedCardGroup = new List<string>();
        StageSelectStart();
        if (stageHand.Count == 0)
        {
            Debug.Log("에러: 전투 시작 드로우가 안됨");
        }
        else
        {
            NowStageCardChangeToTheCardYouCanUseNow();
        }
        stageText.text = "이야기 고리에 새 이야기를 끼우자";
    }
    public void StageTurnStart()
    {
        NowStage = StageType.StageSelect;
        //카드 시전 방식 초기화
        ConsecutiveNumbers = "true"; SameNumber = true; SameColor = true;
        //턴시작 드로우 관리
        if (stageHand.Count < NumberOfStageStartDrawHandCard)
        {
            for (int i = stageHand.Count; i < NumberOfStageStartDrawHandCard; i++)
            {
                Drow(1, "턴 시작");
            }
        }
        //카드 사용가능 여부 확인
        NowStageCardChange(NowStageCard);

        //현재 사용가능한 카드 없으면 사용할수 있는 카드를 호출. Tip: 착한 어린이는 덱을 짤때 카드 연계를 생각해서 짜도록.
        int NumberOfCanNotUseCard = 0;
        foreach (GameObject OneOfstageHand in stageHandObject)
        {
            if (OneOfstageHand.GetComponent<StageCard>().CanUseItNow <= 0)
            {
                NumberOfCanNotUseCard += 1;
            }
        }
        if (NumberOfCanNotUseCard == stageHand.Count + 1)
        {
            NowStageCardChangeToTheCardYouCanUseNow();
        }
        stageText.text = "다음 이야기를 고르자..";
    }
    public void CardSelect(GameObject NowSelectedStageCard)
    {
        if (SelectedStageCard != null)
        {
            SelectedStageCard.transform.localScale = new Vector3(1, 1, 1);
            StageCardSort();
            Debug.Log("카드 선택 취소");
            SelectedStageCard = NowSelectedStageCard;
        }
        else
        {
            SelectedStageCard = NowSelectedStageCard;
        }
    }
    public void StageCardActive(string newStageType, int canUseItNow)
    {
        if (canUseItNow >= 2) { artifactManager.Gold += 50; artifactManager.ArtifactActiveOnGoldChanged(artifactManager.Gold); }
        stageType = newStageType;
        if (stageType == "Battle")
        {
            enemyManger.stageEnemyCount = 0;
            List<string> battleEnemyList = enemyManger.stageEnemy[level + "StageEnemy"][Random.Range(0, enemyManger.stageEnemy[level + "StageEnemy"].Count)];
            enemyManger.MultipleEnemyAdd(battleEnemyList);
            StageEnter();
            battleManager.BattleStart();
        }
        else if (stageType == "EliteBattle")
        {
            enemyManger.stageEnemyCount = 0;
            List<string> battleEnemyList = enemyManger.stageEnemy[level + "StageEliteEnemy"][Random.Range(0, enemyManger.stageEnemy[level + "StageEliteEnemy"].Count)];
            enemyManger.MultipleEnemyAdd(battleEnemyList);
            StageEnter();
            battleManager.BattleStart();
        }
        else if (stageType == "BossBattle")
        {
            enemyManger.stageEnemyCount = 0;
            List<string> battleEnemyList = enemyManger.stageEnemy[level + "StageBossEnemy"][Random.Range(0, enemyManger.stageEnemy[level + "StageBossEnemy"].Count)];
            enemyManger.MultipleEnemyAdd(battleEnemyList);
            StageEnter();
            battleManager.BattleStart();
        }
        else if (stageType == "Rest")
        {
            Debug.Log("휴식 진행");
            rewardManager.RewardText.GetComponent<TextMeshProUGUI>().text = "쉬기로 했다.";
            rewardManager.RewardCardAdd("Stat", "Hp/Rest");
            StageEnter();
            rewardManager.RewardGetStart();
        }
        else if (stageType == "Event")
        {
            Debug.Log("이벤트 진행");
            StageEnter();
            eventManager.EventStart(eventManager.EventList[level + "Event"][Random.Range(0, eventManager.EventList[level + "Event"].Count)]);
        }
    }
    public void StageEnter()
    {
        stageCanvas.SetActive(false);
        Debug.Log("스테이지 진입");
        StageCardCanvas.SetActive(false);
        // 스테이지 진입 시 필요한 초기화 작업을 여기에 추가
    }
    public void GetCardActive(int rare, int number = 3)
    {
        cardGetManager.RandomCardGet(rare, number);
    }
    public void StageSelectStart()
    {
        stageCanvas.SetActive(true);
        StageCardCanvas.SetActive(true);
        StageTurnStart();
        // 스테이지 선택 시작 시 필요한 초기화 작업을 여기에 추가
    }
    public void GameOver()
    {
        List<GameObject> removeStageHandList = new List<GameObject>(stageHandObject);
        foreach (GameObject OneOfCard in removeStageHandList)
        {
            Destroy(OneOfCard);
            stageHandObject.Remove(OneOfCard);
            stageHand.Remove(OneOfCard.GetComponent<StageCard>().Name + "/" + OneOfCard.GetComponent<StageCard>().Color + "/" + OneOfCard.GetComponent<StageCard>().Number);
        }
    }
}