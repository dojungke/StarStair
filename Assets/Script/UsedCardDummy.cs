using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsedCardDummy : MonoBehaviour
{
    public string Name;
    public GameObject CardImageObject;
    public GameObject CardNameTextObject;
    public GameObject CardDescriptionTextObject;
    public GameObject CardCostTextObject;
    void Start()
    {
        //AI 만새!
        //UsedCardDummy의 크기를 해상도에 맞춰서 조정 (삼성 S10+ 기준: 3040x1440)
        //float baseWidth = 3040f;
        //float baseHeight = 1440f;
        //float screenWidth = Screen.width;
        //float screenHeight = Screen.height;

        // 기준 해상도 대비 비율 계산
        //float widthRatio = screenWidth / baseWidth;
        //float heightRatio = screenHeight / baseHeight;
        //float scaleRatio = Mathf.Min(widthRatio, heightRatio);

        // 기본 트렌스폼 값에 곱으로 적용
        //gameObject.GetComponent<Transform>().position = new Vector2(-8 * widthRatio, -4 * heightRatio);
        //gameObject.GetComponent<Transform>().localScale = new Vector3(2.4F * widthRatio, 4F * widthRatio, 1);
    }
    public void NowCardChange(string NowCard)
    {
        Name = NowCard;
        string[] Name2 = Name.Split('/');
        string Number = Name2[2];
        string Color = Name2[1];
        CardData ThisCard = Resources.Load<CardData>("DeckOfAll/" + Name2[0]);
        string KoreanName = ThisCard.Name;
        CardImageObject.GetComponent<SpriteRenderer>().sprite = ThisCard.Image;
        string cardDescription = ThisCard.Description;
        List<AbilityValues> cardAbilityList = new List<AbilityValues>();
        foreach (AbilityValues abilitys in ThisCard.AbilityList)
        {
            AbilityValues newAbility = new AbilityValues
            {
                AbilityType = abilitys.AbilityType,
                Value = abilitys.Value,
                Content = abilitys.Content,
                Target = abilitys.Target
            };
            cardAbilityList.Add(newAbility);
        }
        for (int i = 0; i < cardAbilityList.Count; i++)
        {
            CardDescriptionTextObject.GetComponent<TextMeshPro>().text = cardDescription.Replace($"[Value{i}]", $"{cardAbilityList[i].Value}");
        }
        CardNameTextObject.GetComponent<TextMeshPro>().text = KoreanName;
        if (Number != "6")
        {
            CardCostTextObject.GetComponent<TextMeshPro>().text = Number;
        }
        else
        {
            CardCostTextObject.GetComponent<TextMeshPro>().text = "σ";
        }
        if (Color == "R")
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(160, 60, 60, 255);
        }
        if (Color == "G")
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(80, 160, 60, 255);
        }
        if (Color == "B")
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(60, 120, 160, 255);
        }
        if (Color == "S")
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(125, 125, 125
                , 255);
        }
        if (Color == "T")
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(225, 225, 225, 255);
        }
    }
}
