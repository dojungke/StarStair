using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Card : MonoBehaviour
{
    public string Name;
    public Hand hand;
    public CardAbilityManager cardAbilityManager;
    public GameObject CardImageObject;
    public GameObject CardNameTextObject;
    public GameObject CardDescriptionTextObject;
    public GameObject CardCostTextObject;
    public GameObject CardBackGroundEffect;
    public int CanUseItNow;
    public string KoreanName;
    public string Color;
    public int Number;
    public string Description;
    int enemyLayer = 0;
    public string ActiveType;
    Vector3 FirstmousePos;
    public string ConsecutiveNumbers;
    public bool SameNumber;
    public bool NoUse = false; //장식용 카드일때
    public DeckViewer deckViewer;
    public CardData ThisCard;
    public List<EffectData> relatedEffects = new List<EffectData>();
    public float additionalDamage;
    public List<CardReinforce> reinforceList = new List<CardReinforce>();
    public List<AbilityValues> cardAbilityList = new List<AbilityValues>();
    private void Start()
    {
        if (NoUse == false)
        {
            GetComponent<Animator>().Play("CardDrow");
        }
        enemyLayer = LayerMask.GetMask("EnemyLayer");
        if (NoUse == false)
        {
            if (hand.HandCard.Contains(gameObject) == false)
            {
                hand.AddCard(gameObject);
            }
        }
        Debug.Log("카드추가" + gameObject);
        ThisCard = Resources.Load<CardData>("DeckOfAll/" + Name);
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
        CardSet();
        if (ThisCard.AbilityScript == "CardNomal")
        {
            gameObject.AddComponent(typeof(CardNomal));
        }
        else
        {
            gameObject.AddComponent(Type.GetType(ThisCard.AbilityScript));
        }
        CardAbility cardAbility = gameObject.GetComponent<CardAbility>();
        cardAbility.SkillCostViewerManager = cardAbilityManager.SkillCostViewerManager;
        cardAbility.BattleManager = cardAbilityManager.BattleManager;
        cardAbility.HandManager = cardAbilityManager.HandManager;
        cardAbility.cardAbilityManager = cardAbilityManager.GetComponent<CardAbilityManager>();
        cardAbility.card = this;
    }
    public void CardSet()
    {
        if (ThisCard == null) { ThisCard = Resources.Load<CardData>("DeckOfAll/" + Name); }
        KoreanName = ThisCard.Name;
        CardImageObject.GetComponent<SpriteRenderer>().sprite = ThisCard.Image;
        string cardDescription = ThisCard.Description;
        string cardDescriptionTemp = cardDescription;
        for (int i = 0; i < cardAbilityList.Count; i++)
        {
            if (cardAbilityList[i].AbilityType == "Attack")
            {
                cardAbilityList[i].Value = ThisCard.AbilityList[i].Value + (int)additionalDamage;
                foreach (CardReinforce reinforce in reinforceList) 
                {
                    if (reinforce.reinforceType == CardAbilityType.Attack)
                    {
                    cardAbilityList[i].Value *= (reinforce.reinforceRate * 0.01f + 1);
                    }
                }
                Math.Round(cardAbilityList[i].Value, 0);
            }
            cardDescriptionTemp = cardDescriptionTemp.Replace($"[Value{i}]", $"{cardAbilityList[i].Value}");
            cardAbilityList[i].cardInfo = $"{Name}/{Color}/{Number}";
        }
        CardDescriptionTextObject.GetComponent<TextMeshPro>().text = cardDescriptionTemp;
        CardNameTextObject.GetComponent<TextMeshPro>().text = KoreanName;
        ActiveType = ThisCard.ActiveType;
        relatedEffects = ThisCard.relatedEffect;
        if (Number != 6)
        {
            CardCostTextObject.GetComponent<TextMeshPro>().text = $"{Number}";
        }
        else
        {
            CardCostTextObject.GetComponent<TextMeshPro>().text = "σ";
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
        ;
    }
    public void NowCardChange(bool TurnEnd = false)        //언제까지 쥐고 있을수 없는 것이 많담니다.
    {
        if (TurnEnd == true)
        {
            CardBackGroundEffect.SetActive(false);
            CanUseItNow = 0;
            //턴 아닐때 못쓰게 하고 반투명
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Color spriteRendererColor = spriteRenderer.color;
            spriteRendererColor.a = 0.5F;
            spriteRenderer.color = spriteRendererColor;
        }
        else if(cardAbilityManager.skillCoolTimeViewerSetter.SkillCoolTimeAdd(Name, 0))
        {
            CardBackGroundEffect.SetActive(false);
            CanUseItNow = 1;
            //턴 아닐때 못쓰게 하고 반투명
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Color spriteRendererColor = spriteRenderer.color;
            spriteRendererColor.a = 1.0f;
            spriteRenderer.color = spriteRendererColor;
        }
        else
        {
            CardBackGroundEffect.SetActive(false);
            CanUseItNow = 0;
            //턴 아닐때 못쓰게 하고 반투명
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Color spriteRendererColor = spriteRenderer.color;
            spriteRendererColor.a = 0.5F;
            spriteRenderer.color = spriteRendererColor;
        }
    }
    public void SelectCancel()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        transform.localScale = new Vector3(1, 1, 1);
        hand.relatedEffectViewer.gameObject.SetActive(false);
        hand.SortHand(0);
        Debug.Log("카드 선택 취소");
    }
    public void CardActiveFail()
    {
        Debug.Log("카드 사용 실패");
        transform.position = new Vector3(0, -3, -9);
        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.localScale = new Vector3(1.3f, 1.3f, 1);
        hand.relatedEffectViewer.RelatedEffectView(relatedEffects);
        hand.relatedEffectViewer.gameObject.SetActive(true);
    }
    void OnMouseDown()
    {
        if (NoUse)
        {
            deckViewer.ViewCardSort();
        }
        Debug.Log(Name + "선택 됨");     //카드를 화면 가운데쯤에 크게해서 보여줘요
        transform.position = new Vector3(0, -3, -9);
        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.localScale = new Vector3(1.3f, 1.3f, 1);
        hand.relatedEffectViewer.RelatedEffectView(relatedEffects);
        hand.relatedEffectViewer.gameObject.SetActive(true);
        hand.CardSelect(this.gameObject);
        //최초로 클릭한 위치를 기억해요.
        FirstmousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseDrag()
    {
        hand.relatedEffectViewer.gameObject.SetActive(false);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
    }
    void OnMouseUp()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapCircle(mousePos, 0.3f, enemyLayer);
        if (NoUse)
        {
            transform.position = new Vector3(0, -3, -9);
            transform.rotation = new Quaternion(0, 0, 0, 0);
            transform.localScale = new Vector3(1 / deckViewer.cardScale * 1.3f * 32, 1 / deckViewer.cardScale * 1.3f * 32, 1);
            if (mousePos.y < FirstmousePos.y - 1.5F || mousePos.y > FirstmousePos.y + 1.5F)
            {
                transform.localScale = new Vector3(1, 1, 1);
                hand.relatedEffectViewer.gameObject.SetActive(false);
                deckViewer.ViewCardSort();
            }
            return;
        }
        /*/카드를 버려요
        if (hit != null && hit.CompareTag("UsedCardDummy"))
        {
            hand.HandCard.Remove(this.gameObject);
            hand.UsedCardGroup.Add(Name + "/" + Color + "/" + Number);
            Destroy(this.gameObject);
            hand.SortHand(0);
            Debug.Log("카드 버리기 완료");
        }*/
        //타겟팅 해서 카드를 써요
        else if (ActiveType == "Target")
        {
            if (hit != null && hit.CompareTag("Enemy"))
            {
                Debug.Log(hit);
                if (CanUseItNow == 0)
                {
                    Debug.Log("발동 가능 카드가 아님");
                    CardActiveFail();
                }
                else
                {
                    Debug.Log("카드 발동" + $"{Name}/{Color}/{Number}");
                    cardAbilityManager.CardActive(hit.gameObject, $"{Name}/{Color}/{Number}", CanUseItNow, gameObject);
                }
            }
            else if (mousePos.y < FirstmousePos.y - 1.5F)
            {
                SelectCancel();
            }
            else
            {
                Debug.Log("적 조준 실패");
                CardActiveFail();
            }
        }
        //타겟 없이도 쓸수있는 카드
        else if (ActiveType == "NonTarget")
        {
            if (mousePos.y > FirstmousePos.y + 2F)
            {
                if (CanUseItNow == 0)
                {
                    Debug.Log("발동 가능 카드가 아님");
                    CardActiveFail();
                }
                else
                {
                    Debug.Log("카드 발동" + $"{Name}/{Color}/{Number}");
                    cardAbilityManager.CardActive(gameObject, $"{Name}/{Color}/{Number}", CanUseItNow, gameObject);
                }
            }
            else if (mousePos.y < FirstmousePos.y - 1.5F)
            {
                SelectCancel();
            }
            else
            {
                Debug.Log("적 조준 실패");
                CardActiveFail();
            }
        }
        else
        {
            SelectCancel();
        }
    }
    /*public void UsedCardRemove()
    {
        hand.HandCard.Remove(this.gameObject);
        hand.UsedCardGroup.Add(Name + "/" + Color + "/" + Number);
        Destroy(this.gameObject);
        hand.SortHand(0);
        Debug.Log("카드 사용 완료");
        hand.ConsecutiveNumbers = ConsecutiveNumbers;
        hand.SameNumber = SameNumber;
        hand.SameColor = false; //매턴 처음 내는 카드만 색이 같다는 이유로 사용할 수 있다..
        hand.NowCardChange($"{Name}/{Color}/{Number}");
    }*/
}