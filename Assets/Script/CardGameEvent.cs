using System.Collections.Generic;
using UnityEngine;

public abstract class CardGameEvent : MonoBehaviour
{
    public string eventName;
    public string eventDescription;
    public int eventID;
    public GameObject eventCardDummy;
    public GameObject EventChoicePrefab; // 이벤트 선택 카드 프리팹
    public EventManager eventManager; // 이벤트 매니저

    public abstract void TriggerEvent();  // 반드시 자식 클래스에서 구현
    public abstract void EndEvent();      // 반드시 자식 클래스에서 구현
    public abstract void EventChoiceCardActive(GameObject SelectedCard);
    public virtual void SelectCardSpawn(List<string> ChoiceName, List<string> ChoiceDescription, List<Sprite> ChoiceImage)
    {
        Debug.Log(ChoiceName);
        for (int i = 0; i < ChoiceName.Count; i++)
        {
            GameObject eventChoice = GameObject.Instantiate(EventChoicePrefab, Vector3.zero, Quaternion.identity, eventCardDummy.transform);
            eventChoice.GetComponent<EventChoiceCard>().Name = ChoiceName[i];
            eventChoice.GetComponent<EventChoiceCard>().Description = ChoiceDescription[i];
            eventChoice.GetComponent<EventChoiceCard>().EventManager = eventManager;
            if (ChoiceImage != null)
            {
                //추가예정
            }
            eventChoice.GetComponent<EventChoiceCard>().Number = i;
            eventManager.EventHandObject.Add(eventChoice);
            eventChoice.GetComponent<EventChoiceCard>().CardSetting();
        }
        eventManager.EventCardSort(); // 이벤트 카드 정렬
    }
}