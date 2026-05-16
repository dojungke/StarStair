using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public BattleManager battleManager;
    public StageManager stageManager;
    public CardGetManager cardGetManager;
    public RewardManager rewardManager;
    //public List<string> EventHand;
    public List<GameObject> EventHandObject;
    public float Interval = 1.5f;
    public float RotationInterval = 10f;
    public float HorizonInterval = 1.5f;
    public float widthRatio = 1f; // 화면 너비 비율
    public float heightRatio = 1f; // 화면 높이 비율
    public CardGameEvent NowEvent;
    public GameObject EventChoicePrefab;
    public GameObject EventCardCanvas; // 이벤트 카드 캔버스 오브젝트
    public GameObject EventCardDummy; // 이벤트 카드 더미 오브젝트
    public TextMeshProUGUI EventText; // 이벤트 텍스트 UI\
    public GameObject SelectedCard; // 선택된 이벤트 카드
    public GameObject EventCanvas;
    public Dictionary<String, List<String>> EventList = new Dictionary<string, List<string>>
    {
        { "forestEvent", new List<string> { "EventRemoveCard", "EventGetCard" } },
        { "mountainEvent", new List<string> { "EventGetCard" } },
        { "peakEvent", new List<string> { "EventGetCard", "EventGetCard" } },
    };

    public void Start()
    {
        battleManager.onGameOver.AddListener(GameOver);

    }
    public void EventCardSort()
    {
        int IndexCounter = 0;
        foreach (GameObject Cards in EventHandObject)
        {
            IndexCounter = IndexCounter + 1;
            Cards.transform.localPosition = new Vector3(Interval * widthRatio * (IndexCounter - (EventHandObject.Count / 2F) + 0.5F), -Mathf.Abs(HorizonInterval * (IndexCounter - (EventHandObject.Count / 2f) - 0.5f) * heightRatio) - 5, IndexCounter * -0.1F);
            Cards.transform.rotation = Quaternion.Euler(0, 0, RotationInterval * (IndexCounter - (EventHandObject.Count / 2F) - 0.5F));
        }
    }
    public void EventStart(string EventName)
    {
        gameObject.AddComponent(Type.GetType(EventName));
        CardGameEvent NowEvent = gameObject.GetComponent<CardGameEvent>();
        NowEvent.eventCardDummy = EventCardDummy;
        NowEvent.eventManager = this;
        NowEvent.EventChoicePrefab = EventChoicePrefab;
        EventCardCanvas.SetActive(true);
        EventCanvas.SetActive(true);
        NowEvent.TriggerEvent();
    }
    public void EventEnd()
    {
        List<GameObject> EventChoiceobjectList = new List<GameObject>(EventHandObject);
        foreach (GameObject choice in EventChoiceobjectList)
        {
            EventHandObject.Remove(choice);
            Destroy(choice);
        }
        Destroy(NowEvent);
        EventCardCanvas.SetActive(false);
        EventCanvas.SetActive(false);
        stageManager.StageSelectStart();
    }
    public void CardSelect(GameObject NowSelectedCard)
    {
        if (SelectedCard != null)
        {
            SelectedCard.GetComponent<EventChoiceCard>().SelectCancel();
            EventCardSort();
            Debug.Log("카드 선택 취소");
            SelectedCard = NowSelectedCard;
        }
        else
        {
            SelectedCard = NowSelectedCard;
        }
    }
    public void GameOver()
    {
        EventEnd();
    }
}