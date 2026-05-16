using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventRemoveCard : CardGameEvent
{
    public override void TriggerEvent()
    {
        eventManager.NowEvent = this;
        eventManager.EventText.GetComponent<TextMeshProUGUI>().text = "그 잎을 주면 체력을 30 회복시켜 주마..";
        SelectCardSpawn(new List<string> { "무작위 잎을 준다", "허나 거절한다" }, new List<string> { "무작위 잎을 주고 체력을 30 회복합니다.", "잎을 주지 않고 자리를 피합니다." }, new List<Sprite> { null });
    }
    public override void EventChoiceCardActive(GameObject SelectedCard)
    {
        if (SelectedCard.GetComponent<EventChoiceCard>().Number == 0)
        {
            EndEvent();
            string removedCard = eventManager.battleManager.HandManager.GetComponent<Hand>().Deck[Random.Range(0, eventManager.battleManager.HandManager.GetComponent<Hand>().Deck.Count)];
            eventManager.battleManager.HandManager.GetComponent<Hand>().Deck.Remove(removedCard);
            eventManager.rewardManager.RewardText.GetComponent<TextMeshProUGUI>().text = $"그것은 {removedCard.Split("/")[0]} 잎을 가져갔다..";
            eventManager.rewardManager.RewardCardAdd("Stat", "Hp/30");
            eventManager.stageManager.StageEnter();
            eventManager.rewardManager.RewardGetStart();
        }
        else
        {
            // Logic for when the player chooses not to pick up the card
            eventManager.EventText.GetComponent<TextMeshProUGUI>().text = "제안를 무시하고 앞으로 갔다.";
            EndEvent();
        }
    }
    public override void EndEvent()
    {
        eventManager.EventEnd();
    }
}
