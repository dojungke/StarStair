using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetCard : MonoBehaviour
{
    public string Name;
    public string getRewardType;
    public GameObject CardImageObject;
    public GameObject CardNameTextObject;
    public GameObject CardDescriptionTextObject;
    public GameObject CardCostTextObject;
    public string KoreanName;
    public string Color;
    public int Number;
    public string Description;
    public CardGetManager cardGetManager;
    public List<AbilityValues> cardAbilityList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (getRewardType == "Card")
        {
            CardData ThisCard = Resources.Load<CardData>("DeckOfAll/" + Name);
            KoreanName = ThisCard.Name;
            CardImageObject.GetComponent<SpriteRenderer>().sprite = ThisCard.Image;
            string cardDescription = ThisCard.Description;
            cardAbilityList = new List<AbilityValues>();
            foreach (AbilityValues abilitys in ThisCard.AbilityList)
            {
                AbilityValues newAbility = new AbilityValues
                {
                    AbilityType = abilitys.AbilityType,
                    Value = abilitys.Value,
                    Content = abilitys.Content,
                    Target = abilitys.Target,
                    cardInfo = $"{Name}/{Color}/{Number}"
                };
                cardAbilityList.Add(newAbility);
            }
            string cardDescriptionTemp = cardDescription;
            for (int i = 0; i < cardAbilityList.Count; i++)
            {
                cardDescriptionTemp = cardDescriptionTemp.Replace($"[Value{i}]", $"{cardAbilityList[i].Value}");
            }
            CardDescriptionTextObject.GetComponent<TextMeshPro>().text = cardDescriptionTemp;
            CardNameTextObject.GetComponent<TextMeshPro>().text = KoreanName;
        }
        else if (getRewardType == "Artifact")
        {
            ArtifactData ThisArtifact = Resources.Load<ArtifactData>("ArtifactData/" + Name);
            KoreanName = ThisArtifact.Name;
            CardImageObject.GetComponent<SpriteRenderer>().sprite = ThisArtifact.Image;
            Description = ThisArtifact.Description;
            CardDescriptionTextObject.GetComponent<TextMeshPro>().text = Description;
            CardNameTextObject.GetComponent<TextMeshPro>().text = KoreanName;
        }
        if (Number != 6)
        {
            CardCostTextObject.GetComponent<TextMeshPro>().text += Number;
        }
        else
        {
            CardCostTextObject.GetComponent<TextMeshPro>().text += "σ";
        }
        if (Color == "R")
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(160, 60, 60, (byte)(gameObject.GetComponent<SpriteRenderer>().color.a * 255));
        }
        if (Color == "G")
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(80, 160, 60, (byte)(gameObject.GetComponent<SpriteRenderer>().color.a * 255));
        }
        if (Color == "B")
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(60, 120, 160, (byte)(gameObject.GetComponent<SpriteRenderer>().color.a * 255));
        }
        if (Color == "S")
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(125, 125, 125, (byte)(gameObject.GetComponent<SpriteRenderer>().color.a * 255));
        }
        if (Color == "T")
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(225, 225, 225, (byte)(gameObject.GetComponent<SpriteRenderer>().color.a * 255));
        }
    }
    void OnMouseDown()
    {
        if (getRewardType == "Card")
        {
            // 카드 선택 시 카드 매니저에 선택된 카드 이름 전달
            Debug.Log("카드 선택 시작");
            cardGetManager.GetCardSelected(Name + "/" + Color + "/" + Number);
            Debug.Log("Selected Card: " + Name + "/" + Color + "/" + Number);
        }
        else if (getRewardType == "Artifact")
        {
            Debug.Log("유물 선택 시작");
            cardGetManager.GetArtifactSelected(Name);
            Debug.Log("Selected Artifact: ");
        }
    }
}