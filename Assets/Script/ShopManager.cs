using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shopUI;
    public List<string> shopHand;
    public List<GameObject> shopHandObject; // 스테이지 덱 오브젝트 리스트
    public GameObject ShopCardPrefab; // 스테이지 카드 프리팹
    public UsedShopCardDummy shopUsedCardDummy; // 스테이지 카드 더미 오브젝트
    public GameObject shopCardDummy; // 스테이지 카드 더미 오브젝트
    public float Interval; // 카드 간격
    public float RotationInterval; // 카드 회전 간격
    public float HorizonInterval; // 카드 수평 간격
    public float widthRatio = 1f; // 화면 너비 비율
    public float heightRatio = 1f; // 화면 높이 비율
    public int MaxShopHandCard; // 스테이지에서 최대 핸드 카드 수
    public int NumberOfShopStartDrawHandCard; // 스테이지 시작 시 드로우할 카드 수
    public GameObject SelectedShopCard; // 선택된 스테이지 카드
    public ArtifactManager artifactManager; // 아티팩트 매니저
    public RewardManager rewardManager; // 보상 매니저
    public List<string> shopItemTypeList = new List<string> { "Card", "Artifact" }; // 상점 아이템 타입 리스트
    public void AddCard(GameObject New)
    {
        shopHandObject.Add(New);
        shopHand.Add(New.GetComponent<ShopCard>().Name + "/" + New.GetComponent<ShopCard>().Color + "/" + New.GetComponent<ShopCard>().chanceTime);
        ShopCardSort();
    }
    public void ShopStart()
    {
        shopUI.SetActive(true);
        List<GameObject> shopCardList = new List<GameObject>(shopHandObject);
        for (int i = 0; i < shopHandObject.Count; i++)
        {
            if(shopHand.Count <= 0) { return; }
            shopCardList[i].GetComponent<ShopCard>().chanceTime -= 1;
            if (shopCardList[i].GetComponent<ShopCard>().chanceTime <= 0)
            {
                Destroy(shopHandObject[i]);
                shopHand.Remove(shopHand[i]);
                shopHandObject.Remove(shopHandObject[i]);
            }
            else { shopCardList[i].GetComponent<ShopCard>().NowGoldChange(); }
        }
        ShopCardDrow();
        shopCardList.Clear();
    }
    public void ShopCardSort()
    {
        int IndexCounter = 0;
        foreach (GameObject Cards in shopHandObject)
        {
            IndexCounter = IndexCounter + 1;
            Cards.transform.localPosition = new Vector3(Interval * widthRatio * (IndexCounter - (shopHandObject.Count / 2F) + 0.5F), -Mathf.Abs(HorizonInterval * (IndexCounter - (shopHandObject.Count / 2f) - 0.5f) * heightRatio) - 5, IndexCounter * -0.1F);
            Cards.transform.rotation = Quaternion.Euler(0, 0, RotationInterval * (IndexCounter - (shopHandObject.Count / 2F) - 0.5F));
        }
    }
    public void CardSelect(GameObject NowSelectedShopCard)
    {
        if (SelectedShopCard != null)
        {
            SelectedShopCard.transform.localScale = new Vector3(1, 1, 1);
            ShopCardSort();
            Debug.Log("카드 선택 취소");
            SelectedShopCard = NowSelectedShopCard;
        }
        else
        {
            SelectedShopCard = NowSelectedShopCard;
        }
    }
    public void ShopCardActive(string ShopCardType)
    {
        rewardManager.RewardCardAdd(ShopCardType.Split('/')[0], $"{ShopCardType.Split('/')[1]}/{ShopCardType.Split('/')[2]}");
        artifactManager.Gold -= int.Parse(ShopCardType.Split('/')[3]);
        CardGoldUpdate();
        RewardEnter();
    }
    public void CardGoldUpdate()
    {
        artifactManager.ArtifactActiveOnGoldChanged(artifactManager.Gold);
        foreach (GameObject shopcard in shopHandObject)
        {
            shopcard.GetComponent<ShopCard>().NowGoldChange();
        }
    }
    public void RewardEnter()
    {
        shopUI.SetActive(false);
        rewardManager.RewardGetStart("shop");
        rewardManager.RewardText.GetComponent<TextMeshProUGUI>().text = "얻은 것을 확인해보자";
    }
    public void ShopCardDrow()
    {
        int rarity;
        int choiceNumber;
        int price;
        for (int i = shopHand.Count; i < NumberOfShopStartDrawHandCard; i++)
        {
            if (shopHandObject.Count <= MaxShopHandCard)
            {
                GameObject NewCard = Instantiate(ShopCardPrefab, Vector3.zero, Quaternion.identity, shopCardDummy.transform);
                NewCard.transform.localScale = new Vector3(1, 1, 1);
                shopHandObject.Add(NewCard);
                ShopCard newShopCardScript = NewCard.GetComponent<ShopCard>();
                newShopCardScript.shopManager = this;
                //대충 카드 종류, 가격 구하는 코드
                switch (shopItemTypeList[Random.Range(0, shopItemTypeList.Count)])
                {
                    case "Card":
                        newShopCardScript.Name = "잎 구매";
                        rarity = Random.Range(1, 101);
                        if (rarity <= 60) { rarity = 0; }
                        else if (rarity <= 85) { rarity = 1; }
                        else if (rarity <= 95) { rarity = 2; }
                        else { rarity = 2; }
                        choiceNumber = Random.Range(1, 6);
                        price = ((int)Mathf.Pow(rarity + 1, Random.Range(2, 4)) + Random.Range(1, 8 + choiceNumber)) * 10;
                        newShopCardScript.shopType = $"Card/{rarity}/{choiceNumber}/{price}";
                        newShopCardScript.Description = $"{rarity} 등급의 잎 {choiceNumber}개 중에 하나를 선택해서 얻습니다. 가격 : {price} 별빛";
                        newShopCardScript.price = price;
                        break;
                    case "Artifact":
                        newShopCardScript.Name = "가지 구매";
                        rarity = Random.Range(1, 101);
                        if (rarity <= 60) { rarity = 0; }
                        else if (rarity <= 85) { rarity = 1; }
                        else if (rarity <= 95) { rarity = 2; }
                        else { rarity = 2; }
                        choiceNumber = Random.Range(1, 6);
                        price = ((rarity + 1) * 100 + Random.Range(choiceNumber, choiceNumber + 5) * 10);
                        newShopCardScript.shopType = $"Artifact/{rarity}/{choiceNumber}/{price}";
                        newShopCardScript.Description = $"{rarity} 등급의 가지 {choiceNumber}개 중에 하나를 선택해서 얻습니다. 가격 : {price} 별빛";
                        newShopCardScript.price = price;
                        break;
                }
                newShopCardScript.Color = "T";
                newShopCardScript.chanceTime = Random.Range(1, 6);
                newShopCardScript.CardCostTextObject.GetComponent<TextMeshPro>().text = $"{newShopCardScript.chanceTime}시간";
                shopHand.Add(NewCard.GetComponent<ShopCard>().Name + "/" + NewCard.GetComponent<ShopCard>().Color + "/" + NewCard.GetComponent<ShopCard>().chanceTime);
            }
            else
            {
                Debug.Log("더 이상 카드를 뽑을 수 없습니다.");
            }
            ShopCardSort();
        }
    }
}
