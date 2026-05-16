using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental;
using UnityEngine;
using static StageManager;

public class CardGetManager : MonoBehaviour
{
    public List<string> GetCardList = new List<string>();
    public List<GameObject> GetCardObjectList = new List<GameObject>();
    public GameObject cardObject; // 인스펙터에서 할당할 카드 오브젝트
    public GameObject GetCardCanvas;
    public GameObject GetCardDummy;
    public TextMeshProUGUI GetCardText;
    public Hand handManager;
    public StageManager stageManager;
    public RewardManager rewardManager;
    public ArtifactManager artifactManager;
    bool cardGetStart = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GetCardSort()
    {
        foreach (string cardName in GetCardList)
        {
            //적 오브젝트 X축 4.5간격으로 가운데에서 부터 정렬하는 코드
            //적 오브젝트 생성될때 Enemy에서 자동으로 호출함
            int count = GetCardObjectList.Count;
            if (count == 0) return;

            float spacing = 3.5f;
            float startX = -(spacing * (count - 1)) / 2f;

            for (int i = 0; i < count; i++)
            {
                Vector3 pos = GetCardObjectList[i].transform.position;
                pos.x = startX + i * spacing;
                GetCardObjectList[i].transform.position = pos;
            }
        }
    }
    public void GetCardActive(List<string> getCard)
    {
        GetCardText.text = "얻을 것을 고르자";
        if (stageManager.battleManager.Hp <= 0) { return; }
        stageManager.NowStage = StageType.GetCard;
        if (cardGetStart) return; // 이미 카드 획득이 시작되었으면 중복 실행 방지
        cardGetStart = true; // 카드 획득 시작 플래그 설정
        GetCardList = getCard;
        foreach (string cardName in GetCardList)
        {
            string[] ThisCard = cardName.Split('/');
            GameObject newCard = Instantiate(cardObject, GetCardDummy.transform);
            newCard.GetComponent<GetCard>().getRewardType = "Card";
            newCard.GetComponent<GetCard>().cardGetManager = this;
            GetCardObjectList.Add(newCard);
            newCard.GetComponent<GetCard>().Name = ThisCard[0];
            newCard.GetComponent<GetCard>().Color = ThisCard[1];
            newCard.GetComponent<GetCard>().Number = int.Parse(ThisCard[2]);
        }
        GetCardSort();
        GetCardCanvas.SetActive(true);
        GetCardDummy.SetActive(true);
    }
    public void GetArtifactActive(List<string> getCard)
    {
        if (stageManager.battleManager.Hp <= 0) { return; }
        stageManager.NowStage = StageType.GetCard;
        if (cardGetStart) return; // 이미 카드 획득이 시작되었으면 중복 실행 방지
        cardGetStart = true; // 카드 획득 시작 플래그 설정
        GetCardList = getCard;
        foreach (string cardName in GetCardList)
        {
            string[] ThisCard = cardName.Split('/');
            GameObject newCard = Instantiate(cardObject, GetCardDummy.transform);
            newCard.GetComponent<GetCard>().getRewardType = "Artifact";
            newCard.GetComponent<GetCard>().cardGetManager = this;
            GetCardObjectList.Add(newCard);
            newCard.GetComponent<GetCard>().Name = ThisCard[0];
            newCard.GetComponent<GetCard>().Color = ThisCard[1];
            newCard.GetComponent<GetCard>().Number = int.Parse(ThisCard[2]);
        }
        GetCardSort();
        GetCardCanvas.SetActive(true);
        GetCardDummy.SetActive(true);
    }
    public void GetCardSelected(string SelectedCard)
    {
        handManager.Deck.Add(SelectedCard);
        GetCardClose();
    }
    public void GetArtifactSelected(string SelectedArtifact)
    {
        if (artifactManager.artifactCount + Resources.Load<ArtifactData>("ArtifactData/" + SelectedArtifact).Size > artifactManager.maxArtifact)
        {
            Debug.Log("가지 최대치 초과!!!!!!!!!!");
            GetCardText.text = "가지가 너무 많아...\n기존의 가지를 판매하거나 새로운 가지를 포기해야해.";
            return;
        }
        artifactManager.AddArtifact(SelectedArtifact);
        GetCardClose();
    }
    public void GetCardClose()
    {
        GetCardCanvas.SetActive(false);
        GetCardDummy.SetActive(false);
        foreach (GameObject card in GetCardObjectList)
        {
            Destroy(card);
        }
        GetCardObjectList.Clear();
        GetCardList.Clear();
        cardGetStart = false; // 카드 획득 종료 플래그 초기화
        if (rewardManager.RewardHandObject.Count > 0)
        {
            rewardManager.RewardGetStart(); // 보상 카드 획득 재시작
        }
        else
        {
            rewardManager.RewardGetEnd();
        }
    }
    public void RandomCardGet(int rare = 0, int number = 3)
    {
        List<string> rare0CardList = new List<string> { "TheFool", "Kinding", "MagicMolar", "FireBall", "MagicSwordsmanship" };
        List<string> rare1CardList = new List<string> { "AccelerationMagic", "TaleOfLight", "Honkai", "MagicArrow", "MagicShockWave" };
        List<string> rare2CardList = new List<string> { "FallenStar", "TheAbyss" };
        List<string> randomGetCardList = new List<string>();
        for (int i = 0; i < number; i++)
        {
            int randomNumber = Random.Range(0, 100);
            if (randomNumber <= 3) //확률적으로 2단계 까지 더 희귀한 카드 획득 가능
            {
                rare += 2;
            }
            else if (randomNumber <= 20)
            {
                rare += 1;
            }
            else
            {
                
            }
            if (rare > 2) rare = 2; // rare가 최고 레이도보다 높으면 최대 레어도로 고정 현제 2까지 밖에 없음
            string cardName;
            string cardColor;
            string cardNumber;
            if (rare == 0)
            {
                cardName = rare0CardList[Random.Range(0, rare0CardList.Count)];
            }
            else if (rare == 1)
            {
                cardName = rare1CardList[Random.Range(0, rare1CardList.Count)];
            }
            else
            {
                cardName = rare2CardList[Random.Range(0, rare2CardList.Count)];
            }
            Debug.Log($"{cardName},{rare}");
            CardData ThisCard = Resources.Load<CardData>("DeckOfAll/" + cardName);
            cardColor = ThisCard.cardColor.ToString();
            cardNumber = ThisCard.timeCost.ToString()[Random.Range(0, ThisCard.timeCost.ToString().Length)].ToString();
            randomGetCardList.Add($"{cardName}/{cardColor}/{cardNumber}");
        }
        GetCardActive(randomGetCardList);
    }
    public void RandomArtifactGet(int rare = 0, int number = 3)
    {
        List<string> rare0ArtifactList = new List<string> { "RedRing", "BlueRing", "GreenRing", "ShapeSword" };
        List<string> rare1ArtifactList = new List<string> { "SpaceTimeRing", "GreenDice", "MagicGloves" };
        List<string> rare2ArtifactList = new List<string> { "FireSword", "ManaEngineeringEngin", "BlueWave" };
        List<string> randomGetArtifactList = new List<string>();
        for (int i = 0; i < number; i++)
        {
            int randomNumber = Random.Range(0, 100);
            if (randomNumber <= 3) //확률적으로 2단계 까지 더 희귀한 카드 획득 가능
            {
                rare += 2;
            }
            else if (randomNumber <= 20)
            {
                rare += 1;
            }
            else
            {
                
            }
            if (rare > 2) rare = 2; // rare가 최고 레이도보다 높으면 최대 레어도로 고정 현제 2까지 밖에 없음
            string ArtifactName;
            string ArtifactColor;
            string ArtifactNumber;
            if (rare == 0)
            {
                ArtifactName = rare0ArtifactList[Random.Range(0, rare0ArtifactList.Count)];
            }
            else if (rare == 1)
            {
                ArtifactName = rare1ArtifactList[Random.Range(0, rare1ArtifactList.Count)];
            }
            else
            {
                ArtifactName = rare2ArtifactList[Random.Range(0, rare2ArtifactList.Count)];
            }
            Debug.Log(ArtifactName);
            ArtifactData ThisArtifact = Resources.Load<ArtifactData>("DeckOfAll/" + ArtifactName);
            ArtifactColor = "S";
            ArtifactNumber = "1";
            randomGetArtifactList.Add($"{ArtifactName}/{ArtifactColor}/{ArtifactNumber}");
        }
        GetArtifactActive(randomGetArtifactList);
    }
}