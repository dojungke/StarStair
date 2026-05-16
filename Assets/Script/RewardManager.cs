using System.Collections.Generic;
using UnityEngine;
using static StageManager;

public class RewardManager : MonoBehaviour
{
    public BattleManager battleManager;
    public StageManager stageManager;
    public CardAbilityManager cardAbilityManager;
    public CardGetManager cardGetManager;
    public ShopManager shopManager;
    public List<string> RewardHand;
    public List<GameObject> RewardHandObject;
    public float Interval = 1.5f;
    public float RotationInterval = 10f;
    public float HorizonInterval = 1.5f;
    public float widthRatio = 1f; // 화면 너비 비율
    public float heightRatio = 1f; // 화면 높이 비율
    public GameObject RewardCardDummy; // 보상 카드 더미 오브젝트
    public GameObject RewardCanvas; // 보상 카드 캔버스 오브젝트
    public GameObject RewardCardPrefab; // 보상 카드 프리팹
    public Sprite RewardCardImage; // 보상 카드 이미지
    public GameObject RewardText;
    public GameObject SelectedCard; // 선택된 보상 카드
    public GameObject RewardCardCanvas;
    string caller;

    public void Start()
    {

    }
    public void RewardGetStart(string newCaller = "event")
    {
        caller = newCaller;
        stageManager.NowStage = StageType.Reward;
        RewardCanvas.SetActive(true);
        RewardCardCanvas.SetActive(true);
        RewardCardSort();
    }
    public void RewardCardSort()
    {
        int IndexCounter = 0;
        foreach (GameObject Cards in RewardHandObject)
        {
            IndexCounter = IndexCounter + 1;
            Cards.transform.localPosition = new Vector3(Interval * widthRatio * (IndexCounter - (RewardHandObject.Count / 2F) + 0.5F), -Mathf.Abs(HorizonInterval * (IndexCounter - (RewardHandObject.Count / 2f) - 0.5f) * heightRatio) - 5, IndexCounter * -0.1F);
            Cards.transform.rotation = Quaternion.Euler(0, 0, RotationInterval * (IndexCounter - (RewardHandObject.Count / 2F) - 0.5F));
        }
    }
    public void RewardCardActive(GameObject activeCard)
    {
        switch (activeCard.GetComponent<RewardCard>().RewardType)
        {
            case "Card":
                cardGetManager.RandomCardGet(int.Parse(activeCard.GetComponent<RewardCard>().rewardData.Split("/")[0]), int.Parse(activeCard.GetComponent<RewardCard>().rewardData.Split("/")[1]));
                CardRewardEnter();
                break;
            case "Artifact":
                cardGetManager.RandomArtifactGet(int.Parse(activeCard.GetComponent<RewardCard>().rewardData.Split("/")[0]), int.Parse(activeCard.GetComponent<RewardCard>().rewardData.Split("/")[1]));
                CardRewardEnter();
                break;
            case "Stat":
                AbilityValues stat = new AbilityValues
                {
                    AbilityType = "GetStat",
                    Content = activeCard.GetComponent<RewardCard>().rewardData.Split("/")[0],
                    Value = int.Parse(activeCard.GetComponent<RewardCard>().rewardData.Split("/")[1]),
                    Target = TargetTypes.Player
                };
                cardAbilityManager.GetStat(stat);
                RewardGetEnd();
                break;
        }

    }
    public void CardSelect(GameObject NowSelectedCard)
    {
        if (SelectedCard != null)
        {
            SelectedCard.GetComponent<RewardCard>().SelectCancel();
            RewardCardSort();
            Debug.Log("잎 선택 취소");
            SelectedCard = NowSelectedCard;
        }
        else
        {
            SelectedCard = NowSelectedCard;
        }
    }
    public void RewardCardAdd(string rewardType, string rewardData)
    {
        if (rewardType == "Card")
        {
            GameObject newRewawrdCard = Instantiate(RewardCardPrefab, RewardCardDummy.transform);
            newRewawrdCard.GetComponent<RewardCard>().RewardManager = this;
            RewardHandObject.Add(newRewawrdCard);
            RewardHand.Add(rewardData);
            newRewawrdCard.GetComponent<RewardCard>().RewardType = rewardType;
            newRewawrdCard.GetComponent<RewardCard>().rewardData = rewardData;
            newRewawrdCard.GetComponent<RewardCard>().Name = "잎 보상";
            newRewawrdCard.GetComponent<RewardCard>().Description = $"{rewardData.Split("/")[1]} 장의 잎중 하나를 선택하여 획득합니다.";
            newRewawrdCard.GetComponent<RewardCard>().Image = RewardCardImage; //임시로 넣은 이미지, 나중에 카드 이미지로 변경 필요
            newRewawrdCard.GetComponent<RewardCard>().CardSetting();
        }
        else if (rewardType == "Artifact")
        {
            GameObject newRewawrdCard = Instantiate(RewardCardPrefab, RewardCardDummy.transform);
            newRewawrdCard.GetComponent<RewardCard>().RewardManager = this;
            RewardHandObject.Add(newRewawrdCard);
            RewardHand.Add(rewardData);
            newRewawrdCard.GetComponent<RewardCard>().RewardType = rewardType;
            newRewawrdCard.GetComponent<RewardCard>().rewardData = rewardData;
            newRewawrdCard.GetComponent<RewardCard>().Name = "가지 보상";
            newRewawrdCard.GetComponent<RewardCard>().Description = $"{rewardData.Split("/")[1]} 개의 가지중 하나를 선택하여 획득합니다.";
            newRewawrdCard.GetComponent<RewardCard>().Image = RewardCardImage; //임시로 넣은 이미지, 나중에 유물 이미지로 변경 필요
            newRewawrdCard.GetComponent<RewardCard>().CardSetting();
        }
        else if (rewardType == "Stat")
        {
            GameObject newRewawrdCard = Instantiate(RewardCardPrefab, RewardCardDummy.transform);
            newRewawrdCard.GetComponent<RewardCard>().RewardManager = this;
            RewardHandObject.Add(newRewawrdCard);
            RewardHand.Add(rewardData);
            newRewawrdCard.GetComponent<RewardCard>().RewardType = rewardType;
            if (rewardData == "Hp/Rest")
            {
                newRewawrdCard.GetComponent<RewardCard>().Name = "휴식";
                newRewawrdCard.GetComponent<RewardCard>().Description = $"휴식하여 체력을 잃은 체력의 30% + 10 ({(int)((battleManager.MaxHp - battleManager.Hp) * 0.3) + 10})만큼 회복합니다.";
                newRewawrdCard.GetComponent<RewardCard>().rewardData = $"Hp/{(int)((battleManager.MaxHp - battleManager.Hp) * 0.3) + 10}";
            }
            else
            {
                newRewawrdCard.GetComponent<RewardCard>().rewardData = rewardData;
                newRewawrdCard.GetComponent<RewardCard>().Name = "능력치 보상";
                newRewawrdCard.GetComponent<RewardCard>().Description = $"{rewardData.Split("/")[0]} 을 {rewardData.Split("/")[1]} 만큼 획득합니다.";
            }
            newRewawrdCard.GetComponent<RewardCard>().Image = RewardCardImage; //임시로 넣은 이미지, 나중에 스탯 이미지로 변경 필요
            newRewawrdCard.GetComponent<RewardCard>().CardSetting();
        }
    }
    public void RewardGetEnd()
    {
        foreach (GameObject card in RewardHandObject)
        {
            Destroy(card);
        }
        RewardHandObject.Clear();
        RewardHand.Clear();
        RewardCardCanvas.SetActive(false);
        RewardCanvas.SetActive(false);
        if (caller == "battleWin")
        {
            shopManager.ShopStart();
            shopManager.shopUI.SetActive(true);
        }
        else if (caller == "BossBattleWin")
        {
            stageManager.LevelStart();
            stageManager.StageEnter();
            shopManager.ShopStart();
            shopManager.shopUI.SetActive(true);
        }
        else if (caller == "shop")
        {
            shopManager.shopUI.SetActive(true);
        }
        else if (caller == "event")
        {
            shopManager.ShopStart();
            shopManager.shopUI.SetActive(true);
        }
        //stageManager.StageSelectStart();
    }
    public void CardRewardEnter()
    {
        RewardCardCanvas.SetActive(false);
        RewardCanvas.SetActive(false);
    }
}