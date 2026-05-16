using TMPro;
using UnityEngine;

public class StageCard : MonoBehaviour
{
    public string Name;
    public StageManager stageManager;
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
    public string stageType; // 카드가 활성화될 때의 스테이지 타입 (예: "Battle", "Event", "Boss" 등)
    private void Start()
    {
        if (stageManager == null)
        {
            stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        }
        enemyLayer = LayerMask.GetMask("EnemyLayer");
        if (stageManager.stageHandObject.Contains(gameObject) == false)
        {
            stageManager.AddCard(gameObject);
        }
        Debug.Log("카드추가" + gameObject);
        StageCardData ThisCard = Resources.Load<StageCardData>("StageCards/" + Name);
        KoreanName = ThisCard.Name;
        CardImageObject.GetComponent<SpriteRenderer>().sprite = ThisCard.Image;
        CardDescriptionTextObject.GetComponent<TextMeshPro>().text = ThisCard.Description;
        CardNameTextObject.GetComponent<TextMeshPro>().text = KoreanName;
        stageType = ThisCard.name;
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
        else
        {
            ConsecutiveNumbers = stageManager.ConsecutiveNumbers;
            SameNumber = stageManager.SameNumber;
            string[] NowCard = stageManager.NowStageCard.Split("/");     //강화시전 여부를 검사해요
            if ((NowCard[1], Color) is ("R", "B") or ("G", "R") or ("B", "G") or ("S", "T") or ("T", "S"))
            {
                if ((NowCard[1] == "S" || NowCard[1] == "T") && ((Number - 1 == int.Parse(NowCard[2]) || (Number == 1 && int.Parse(NowCard[2]) == 6))) && (stageManager.ConsecutiveNumbers == "Up" || stageManager.ConsecutiveNumbers == "true"))
                {
                    CardBackGroundEffect.SetActive(true);
                    CanUseItNow = 2;
                    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                    Color spriteRendererColor = spriteRenderer.color;
                    spriteRendererColor.a = 1F;
                    spriteRenderer.color = spriteRendererColor;
                    ConsecutiveNumbers = "Up";
                }
                else if ((NowCard[1] == "S" || NowCard[1] == "T") && ((Number + 1 == int.Parse(NowCard[2]) || (Number == 6 && int.Parse(NowCard[2]) == 1))) && (stageManager.ConsecutiveNumbers == "Down" || stageManager.ConsecutiveNumbers == "true"))
                {
                    CardBackGroundEffect.SetActive(true);
                    CanUseItNow = 2;
                    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                    Color spriteRendererColor = spriteRenderer.color;
                    spriteRendererColor.a = 1F;
                    spriteRenderer.color = spriteRendererColor;
                    ConsecutiveNumbers = "Down";
                }
                else if (Number == int.Parse(NowCard[2]) && stageManager.SameNumber == true)
                {
                    CardBackGroundEffect.SetActive(true);
                    CanUseItNow = 2;
                    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                    Color spriteRendererColor = spriteRenderer.color;
                    spriteRendererColor.a = 1F;
                    spriteRenderer.color = spriteRendererColor;
                }
                else
                {
                    CardBackGroundEffect.SetActive(false);
                    CanUseItNow = 0;
                    //못 쓰는 카드 반투명
                    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                    Color spriteRendererColor = spriteRenderer.color;
                    spriteRendererColor.a = 0.5F;
                    spriteRenderer.color = spriteRendererColor;
                }
            }
            else     //일반 시전 가능 여부를 검사해요.
            {
                if ((NowCard[1] == Color) && ((Number - 1 == int.Parse(NowCard[2]) || (Number == 1 && int.Parse(NowCard[2]) == 6))) && (stageManager.ConsecutiveNumbers == "true" || stageManager.ConsecutiveNumbers == "Up"))
                {
                    CardBackGroundEffect.SetActive(false);
                    CanUseItNow = 1;
                    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                    Color spriteRendererColor = spriteRenderer.color;
                    spriteRendererColor.a = 1F;
                    spriteRenderer.color = spriteRendererColor;
                    ConsecutiveNumbers = "Up";
                }
                else if ((NowCard[1] == Color) && ((Number + 1 == int.Parse(NowCard[2]) || (Number == 6 && int.Parse(NowCard[2]) == 1))) && (stageManager.ConsecutiveNumbers == "true" || stageManager.ConsecutiveNumbers == "Down"))
                {
                    CardBackGroundEffect.SetActive(false);
                    CanUseItNow = 1;
                    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                    Color spriteRendererColor = spriteRenderer.color;
                    spriteRendererColor.a = 1F;
                    spriteRenderer.color = spriteRendererColor;
                    ConsecutiveNumbers = "Down";
                }
                else if (Number == int.Parse(NowCard[2]) && stageManager.SameNumber == true)
                {
                    CardBackGroundEffect.SetActive(false);
                    CanUseItNow = 1;
                    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                    Color spriteRendererColor = spriteRenderer.color;
                    spriteRendererColor.a = 1F;
                    spriteRenderer.color = spriteRendererColor;
                }
                else if (NowCard[1] == Color && stageManager.SameColor == true)
                {
                    CardBackGroundEffect.SetActive(false);
                    CanUseItNow = 1;
                    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                    Color spriteRendererColor = spriteRenderer.color;
                    spriteRendererColor.a = 1F;
                    spriteRenderer.color = spriteRendererColor;
                }
                else
                {
                    CardBackGroundEffect.SetActive(false);
                    CanUseItNow = 0;
                    //못 쓰는 카드 반투명
                    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                    Color spriteRendererColor = spriteRenderer.color;
                    spriteRendererColor.a = 0.5F;
                    spriteRenderer.color = spriteRendererColor;
                }
                if (NowCard[1] == Color && stageManager.SameColor == true && stageManager.SameNumber == true)
                {
                    stageManager.ConsecutiveNumbers = "true"; //첫번째 내는 카드로 인해 연속성 조건을 소모하는 것을 방지합니다.
                }
            }
        }
        Debug.Log($"{Name}카드 사용 가능여부 {CanUseItNow}");
    }
    public void SelectCancel()
    {
        transform.localScale = new Vector3(1, 1, 1);
        stageManager.StageCardSort();
        Debug.Log("카드 선택 취소");
    }
    public void CardActiveFail()
    {
        Debug.Log("카드 사용 실패");
        transform.position = new Vector3(0, -3, -9);
        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.localScale = new Vector3(1.3f, 1.3f, 1);
    }
    void OnMouseDown()    //이 카드를 누르면 신기한 일이 일어날 거에요
    {
        Debug.Log(Name + "선택 됨");     //카드를 화면 가운데쯤에 크게해서 보여줘요
        transform.position = new Vector3(0, -3, -9);
        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.localScale = new Vector3(1.3f, 1.3f, 1);
        stageManager.CardSelect(this.gameObject);
        //최초로 클릭한 위치를 기억해요.
        FirstmousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseDrag()      //이 카드를 드래그할때 기묘한 체험을 할거예요
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);     //마우스 움직이는 대로 카드도 움직여요
        transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
    }
    void OnMouseUp()       //이 카드를 손에서 논 순간 마법이 펼쳐져요
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapCircle(mousePos, 0.3f, enemyLayer);
        //카드를 버려요
        if (hit != null && hit.CompareTag("UsedCardDummy"))
        {
            stageManager.stageHandObject.Remove(this.gameObject);
            stageManager.stageHand.Remove(Name + "/" + Color + "/" + Number);
            stageManager.stageUsedCardGroup.Add(Name + "/" + Color + "/" + Number);
            Destroy(this.gameObject);
            stageManager.StageCardSort();
            Debug.Log("카드 버리기 완료");
            if (stageManager.stageHandObject.Count == 0)
            {
                stageManager.BossStageDrow();
            }
        }
        //타겟 없이도 쓸수있는 카드가 있담니다.
        if (mousePos.y > FirstmousePos.y + 2F)
        {
            if (CanUseItNow == 0)
            {
                Debug.Log("발동 가능 카드가 아님");
                CardActiveFail();
            }
            else
            {
                Debug.Log("카드 발동" + Name);
                if (CanUseItNow == 2)
                {
                    stageManager.StageCardActive(stageType, CanUseItNow);
                    UsedCardRemove();
                }
                else
                {
                    stageManager.StageCardActive(stageType, CanUseItNow);
                    UsedCardRemove();
                }
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
    public void UsedCardRemove()
    {
        stageManager.stageHandObject.Remove(this.gameObject);
        stageManager.stageHand.Remove(Name + "/" + Color + "/" + Number);
        stageManager.stageUsedCardGroup.Add(Name + "/" + Color + "/" + Number);
        Destroy(this.gameObject);
        stageManager.StageCardSort();
        Debug.Log("카드 사용 완료");
        stageManager.NowStageCardChange($"{Name}/{Color}/{Number}");
        stageManager.ConsecutiveNumbers = ConsecutiveNumbers;
        stageManager.SameNumber = SameNumber;
        stageManager.SameColor = false; //매턴 처음 내는 카드만 색이 같다는 이유로 사용할 수 있다..
    }
}