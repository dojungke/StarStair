using TMPro;
using UnityEngine;

public class ShopCard : MonoBehaviour
{
    public string Name;
    public ShopManager shopManager;
    public GameObject CardImageObject;
    public GameObject CardNameTextObject;
    public GameObject CardDescriptionTextObject;
    public GameObject CardCostTextObject;
    public GameObject CardPriceText;
    public int CanUseItNow;
    public string KoreanName;
    public string Color;
    public int chanceTime;
    public string Description;
    int enemyLayer = 0;
    public string ActiveType;
    Vector3 FirstmousePos;
    public int price;
    public string shopType; // 카드가 활성화될 때의 스테이지 타입 (예: "Battle", "Event", "Boss" 등)
    private void Start()
    {
        if (shopManager == null)
        {
            shopManager = GameObject.Find("shopManager").GetComponent<ShopManager>();
        }
        enemyLayer = LayerMask.GetMask("EnemyLayer");
        if (shopManager.shopHandObject.Contains(gameObject) == false)
        {
            shopManager.AddCard(gameObject);
        }
        Debug.Log("카드추가" + gameObject);
        //CardImageObject.GetComponent<SpriteRenderer>().sprite = ThisCard.Image;
        CardDescriptionTextObject.GetComponent<TextMeshPro>().text = Description;
        CardNameTextObject.GetComponent<TextMeshPro>().text = Name;
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
        NowGoldChange();
    }
    public void NowGoldChange()
    {
        CardPriceText.GetComponent<TextMeshPro>().text = $"{price}☆";
        CardCostTextObject.GetComponent<TextMeshPro>().text = $"{chanceTime}";
        //현재 보유중인 골드량에 따라서 카드를 사용할 수 있는지 없는지 알려줍니다.
        if (shopManager.artifactManager.Gold >= price)
        {
            CanUseItNow = 1;
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Color spriteRendererColor = spriteRenderer.color;
            spriteRendererColor.a = 1F;
            spriteRenderer.color = spriteRendererColor;
        }
        else
        {
            CanUseItNow = 0;
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Color spriteRendererColor = spriteRenderer.color;
            spriteRendererColor.a = 0.5F;
            spriteRenderer.color = spriteRendererColor;
        }
    }
    public void SelectCancel()
    {
        transform.localScale = new Vector3(1, 1, 1);
        shopManager.ShopCardSort();
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
        shopManager.CardSelect(this.gameObject);
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
            shopManager.shopHandObject.Remove(this.gameObject);
            shopManager.shopHand.Remove(Name + "/" + Color + "/" + chanceTime);
            Destroy(this.gameObject);
            shopManager.ShopCardSort();
            Debug.Log("카드 버리기 완료");
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
                    shopManager.ShopCardActive(shopType);
                    UsedCardRemove();
                }
                else
                {
                    shopManager.ShopCardActive(shopType);
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
        shopManager.shopHand.Remove(shopManager.shopHand[shopManager.shopHandObject.IndexOf(gameObject)]);
        shopManager.shopHandObject.Remove(this.gameObject);
        Destroy(this.gameObject);
        shopManager.ShopCardSort();
        Debug.Log("카드 사용 완료");
    }
}