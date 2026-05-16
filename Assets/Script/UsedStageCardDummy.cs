using TMPro;
using UnityEngine;

public class UsedStageCardDummy : MonoBehaviour
{
    public string Name;
    public GameObject CardImageObject;
    public GameObject CardNameTextObject;
    public GameObject CardDescriptionTextObject;
    public GameObject CardCostTextObject;
    void Start()
    {

    }
    public void NowStageCardChange(string NowCard)
    {
        Name = NowCard;
        string[] Name2 = Name.Split('/');
        string Number = Name2[2];
        string Color = Name2[1];
        StageCardData ThisCard = Resources.Load<StageCardData>("StageCards/" + Name2[0]);
        string KoreanName = ThisCard.Name;
        CardImageObject.GetComponent<SpriteRenderer>().sprite = ThisCard.Image;
        CardDescriptionTextObject.GetComponent<TextMeshPro>().text = ThisCard.Description;
        CardNameTextObject.GetComponent<TextMeshPro>().text = KoreanName;
        if (Number != "6")
        {
            CardCostTextObject.GetComponent<TextMeshPro>().text = Number;
        }
        else
        {
            CardCostTextObject.GetComponent<TextMeshPro>().text = "¥ò";
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
