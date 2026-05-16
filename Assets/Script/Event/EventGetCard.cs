using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventGetCard : CardGameEvent
{
    public override void TriggerEvent()
    {
        eventManager.NowEvent = this;
        eventManager.EventText.GetComponent<TextMeshProUGUI>().text = "길을 가다 잎을 발겼했어..";
        SelectCardSpawn(new List<string> { "줍는다", "줍지 않는다" }, new List<string> { "잎을 주워 확인한다", "잎을 무시하고 앞으로 간다" }, new List<Sprite> { null });
    }
    public override void EventChoiceCardActive(GameObject SelectedCard)
    {
        if (SelectedCard.GetComponent<EventChoiceCard>().Number == 0)
        {
            eventManager.rewardManager.RewardText.GetComponent<TextMeshProUGUI>().text = "잎을 주워 확인하자.";
            eventManager.EventEnd();
            eventManager.stageManager.StageEnter();
            if (Random.Range(0, 100) < 50)
            {
                eventManager.rewardManager.RewardCardAdd("Card", "1/3");
            }
            else
            {
                eventManager.rewardManager.RewardCardAdd("Card", "0/3");
            }
            eventManager.rewardManager.RewardGetStart();
        }
        else
        {
            // Logic for when the player chooses not to pick up the card
            eventManager.EventText.GetComponent<TextMeshProUGUI>().text = "잎을 무시하고 앞으로 갔다.";
            EndEvent();
        }
    }
    public override void EndEvent()
    {
        eventManager.EventEnd();
    }
}
