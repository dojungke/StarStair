using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hand : MonoBehaviour
{
    public GameObject CardAbilityManager;
    public UsedCardDummy usedCardDummy;
    public GameObject HandCardDummy;
    public GameObject BattleCardCanvas;
    public RelatedEffectViewer relatedEffectViewer;
    public List<GameObject> HandCard;
    public int SortType;
    public float Interval;
    public float RotationInterval;
    public float HorizonInterval;
    public int NumberOfTurnStartDorwHandCard;
    public int MaxHandCard;
    public string NowCard;
    public GameObject SelectedCard;
    public List<string> Deck;
    public List<string> UnUsedCardGroup;
    public List<string> UsedCardGroup;
    public GameObject Cardprefab;
    public string ConsecutiveNumbers;
    public bool SameNumber;
    public bool SameColor;
    public GameObject BattleManager;
    public ArtifactManager artifactManager;
    float widthRatio = 1f;
    float heightRatio = 1f;
    void Start()
    {
        UnUsedCardGroup = new List<string>(Deck);
    }
    public void AddCard(GameObject New)
    {
        HandCard.Add(New);
        SortHand(SortType);
    }
    public void Shuffle()
    {
        UnUsedCardGroup.AddRange(UsedCardGroup);
        UsedCardGroup.Clear();
        Debug.Log("덱 셔플");
    }
    public void Drow(int DrowNumber = 1, string DrowType = "일반")
    {
        if (HandCard.Count >= MaxHandCard)
        {
            Debug.Log("손패 최대수 초과");
        }
        else
        {
            if (UnUsedCardGroup.Count < 1)
            {
                Shuffle();
            }
            while (DrowNumber >= 1)
            {
                if (UnUsedCardGroup.Count < 1) { Shuffle(); }
                if (UnUsedCardGroup.Count < 1) { return; }
                int UnUsedCardGroupNumber = UnUsedCardGroup.Count;
                if (UnUsedCardGroupNumber < 1) { UnUsedCardGroupNumber = 1; }
                int DrowCard = UnityEngine.Random.Range(0, UnUsedCardGroupNumber);
                string[] DrowCard2 = UnUsedCardGroup[DrowCard].Split('/');
                GameObject newcard = Instantiate(Cardprefab, Vector3.zero, Quaternion.identity, HandCardDummy.transform);
                //Debug.Log("드로우 " + DrowCard2[0] + " 색상:" + DrowCard2[1] + " 숫자:" + DrowCard2[2]);
                newcard.GetComponent<Card>().Name = DrowCard2[0];
                newcard.GetComponent<Card>().Color = DrowCard2[1];
                newcard.GetComponent<Card>().Number = int.Parse(DrowCard2[2]);
                newcard.GetComponent<Card>().hand = GetComponent<Hand>();
                newcard.GetComponent<Card>().cardAbilityManager = CardAbilityManager.GetComponent<CardAbilityManager>();
                AddCard(newcard);
                artifactManager.ArtifactActiveOnCardDrow($"{UnUsedCardGroup[DrowCard]}", newcard.GetComponent<Card>());


                //Debug.Log("카드" + UnUsedCardGroup[DrowCard] + DrowType);
                UnUsedCardGroup.Remove(UnUsedCardGroup[DrowCard]);
                DrowNumber -= 1;
            }
        }
    }
    public void NowCardChange(string NewNowCard, bool TurnEnd = false)
    {
        Debug.Log("현재 카드 변화");
        //일반적인 상황에는 카드를 받아서 봐꿔요.
        if (TurnEnd == false)
        {
            NowCard = NewNowCard;
            usedCardDummy.NowCardChange(NewNowCard);
        }
        //턴이 끝날때는 턴 끝난것만 알려줘요.
        foreach (GameObject OneOfCard in HandCard)
        {
            OneOfCard.GetComponent<Card>().NowCardChange(TurnEnd);
        }
    }
    public void NowCardChangeToTheCardYouCanUseNow()
    {
        //사용 가능하도록 카드 호출.
        if (HandCard.Count == 0) { return; }
        int StartCardRandomNumber = UnityEngine.Random.Range(0, HandCard.Count);
        string StartCardColor = HandCard[StartCardRandomNumber].GetComponent<Card>().Color;
        StartCardRandomNumber = UnityEngine.Random.Range(1, 7);
        NowCardChange("Mana" + "/" + StartCardColor + "/" + StartCardRandomNumber);
    }
    public void BattleStart()
    {
        UnUsedCardGroup = new List<string>(Deck);
        TurnStart();
        if (HandCard.Count == 0)
        {
            Debug.Log("에러: 전투 시작 드로우가 안됨");
        }
        else
        {
            NowCardChangeToTheCardYouCanUseNow();
        }
    }
    public void TurnStart()
    {
        //카드 시전 방식 초기화
        ConsecutiveNumbers = "true"; SameNumber = true; SameColor = true;
        //턴시작 드로우 관리
        if (HandCard.Count < NumberOfTurnStartDorwHandCard)
        {
            for (int i = HandCard.Count; i < NumberOfTurnStartDorwHandCard; i++)
            {
                Drow(1, "턴 시작");
            }
        }
        //카드 사용가능 여부 확인
        NowCardChange(NowCard);

        //현재 사용가능한 카드 없으면 사용할수 있는 카드를 호출. Tip: 착한 어린이는 덱을 짤때 카드 연계를 생각해서 짜도록.
        bool CanUseCard = false;
        foreach (GameObject OneOfHandCard in HandCard)
        {
            if (OneOfHandCard.GetComponent<Card>().CanUseItNow > 0)
            {
                CanUseCard = true;
            }
        }
        if (CanUseCard == false)
        {
            NowCardChangeToTheCardYouCanUseNow();
        }
    }
    public void SortHand(int SortType)
    {
        //0=단순 간격 조정, 1=색깔, 2=숫자 기본적으로는 낮은숫자->큰숫자 σ는 6취급 빨->초->파->검->흰
        if (SortType == 0)
        {
            float IndexCounter = 0;
            foreach (GameObject Cards in HandCard)
            {
                IndexCounter = IndexCounter + 1;
                Cards.transform.localPosition = new Vector3(Interval * widthRatio * (IndexCounter - (HandCard.Count / 2F) + 0.5F), -Mathf.Abs(HorizonInterval * (IndexCounter - (HandCard.Count / 2f) - 0.5f) * heightRatio) - 5, IndexCounter * -0.1F);
                Cards.transform.rotation = Quaternion.Euler(0, 0, RotationInterval * (IndexCounter - (HandCard.Count / 2F) - 0.5F));
                Cards.transform.localScale = new Vector3(1, 1, 1);
                Cards.GetComponent<Card>().CardSet();
                //Debug.Log(HandCard.Count);
                //Debug.Log(IndexCounter);
            }
        }
    }
    public void CardSelect(GameObject NowSelectedCard)
    {
        if (SelectedCard != null)
        {
            SelectedCard.transform.localScale = new Vector3(1, 1, 1);
            SortHand(0);
            Debug.Log("카드 선택 취소");
            SelectedCard = NowSelectedCard;
        }
        else
        {
            SelectedCard = NowSelectedCard;
        }
    }
}