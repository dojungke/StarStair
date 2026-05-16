using System.Collections.Generic;
using UnityEngine;

public class DeckViewer : MonoBehaviour
{
    public Hand handManager;
    public List<GameObject> viewCardList;
    public GameObject DeckViewCanvas;
    public bool on;
    public float cardNumberPerLine = 10;
    public float cardLineNumver = 3;
    public float cardScale = 25;
    public float lastCardScale = 25;
    public GameObject viewCardPrefab;

    public void DeckView()
    {
        ViewCardRemove();
        foreach (string cards in handManager.Deck)
        {
            ViewCardAdd(cards);
            ViewCardSort();
        }
    }
    public void UsedCardDummyView()
    {
        ViewCardRemove();
        foreach (string cards in handManager.UsedCardGroup)
        {
            ViewCardAdd(cards);
            ViewCardSort();
        }
    }
    public void UnUsedCardDummyView()
    {
        ViewCardRemove();
        foreach (string cards in handManager.UnUsedCardGroup)
        {
            ViewCardAdd(cards);
            ViewCardSort();
        }
    }
    public void ViewCardAdd(string cardInfomation)
    {
        string name = cardInfomation.Split("/")[0];
        string color = cardInfomation.Split("/")[1];
        int number = int.Parse(cardInfomation.Split("/")[2]);
        GameObject newViewCard = Instantiate(viewCardPrefab, this.gameObject.transform);
        newViewCard.GetComponent<Card>().NoUse = true;
        newViewCard.GetComponent<Card>().Name = name;
        newViewCard.GetComponent<Card>().Color = color;
        newViewCard.GetComponent<Card>().Number = number;
        newViewCard.GetComponent<Card>().hand = handManager.GetComponent<Hand>();
        newViewCard.GetComponent<Card>().cardAbilityManager = handManager.CardAbilityManager.GetComponent<CardAbilityManager>();
        newViewCard.GetComponent<Card>().deckViewer = this;
        viewCardList.Add(newViewCard);
    }
    public void ViewCardSort()
    {
        int totalCards = viewCardList.Count;

        // 초기값 설정
        cardScale = 25;
        lastCardScale = cardScale;
        cardNumberPerLine = 10;
        cardLineNumver = 3;
        // 초과 카드 수 계산
        float maxCards = cardNumberPerLine * cardLineNumver;

        // 스케일과 줄 수 조정
        while (totalCards > cardNumberPerLine * cardLineNumver)
        {
            cardScale = cardScale * cardNumberPerLine / (cardNumberPerLine + 1f);
            cardNumberPerLine += 1;
            if (cardScale <= lastCardScale * cardLineNumver / (cardLineNumver + 1f))
            {
                cardLineNumver += 1;
                lastCardScale = cardScale;
            }

            // 최소 스케일 제한
            if (cardScale < 1)
            {
                cardScale = 1;
                break;
            }
        }

        // 카드 위치 배치
        for (int i = 0; i < viewCardList.Count; i++)
        {
            GameObject card = viewCardList[i];

            float x = i % cardNumberPerLine * 3f;
            int y = (int)(i / cardNumberPerLine) * -5;

            card.transform.localPosition = new Vector3(x, y, -1f);
        }

        // 전체 스케일 적용
        gameObject.transform.localScale = new Vector3(cardScale, cardScale, 1);
    }
    public void ViewCardRemove()
    {
        List<GameObject> removeCardList = new List<GameObject>(viewCardList);
        foreach (GameObject card in removeCardList)
        {
            Destroy(card);
        }
        viewCardList = new List<GameObject>();
    }
    public void DeckVeiwOnOff()
    {
        if (on == false)
        {
            on = true;
            gameObject.SetActive(true);
            DeckView();
            handManager.BattleManager.GetComponent<BattleManager>().BattleCanvas.SetActive(false);
            DeckViewCanvas.SetActive(true);
        }
        else
        {
            on = false;
            gameObject.SetActive(false);
            ViewCardRemove();
            if (handManager.BattleManager.GetComponent<BattleManager>().stageManager.NowStage == StageManager.StageType.Battle)
            {
                handManager.BattleManager.GetComponent<BattleManager>().BattleCanvas.SetActive(true);
            }
            DeckViewCanvas.SetActive(false);
        }
    }
}
