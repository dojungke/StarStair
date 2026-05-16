using TMPro;
using UnityEngine;

public class EventChoiceCard : MonoBehaviour
{
    public string Name;
    public EventManager EventManager;
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
    public string EventType; // 지금 진행중인 이벤트
    public Sprite Image;
    public int ChoiceNumber; // 선택지 번호
    public void CardSetting()
    {
        CardImageObject.GetComponent<SpriteRenderer>().sprite = Image;
        CardDescriptionTextObject.GetComponent<TextMeshPro>().text = Description;
        CardNameTextObject.GetComponent<TextMeshPro>().text = Name;
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
        ;
    }
    public void SelectCancel()
    {
        transform.localScale = new Vector3(1, 1, 1);
        EventManager.EventCardSort();
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
        Debug.Log(Name + "선택 됨");    //카드를 화면 가운데쯤에 크게해서 보여줘요
        transform.position = new Vector3(0, -3, -9);
        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.localScale = new Vector3(1.3f, 1.3f, 1);
        EventManager.CardSelect(this.gameObject);
        FirstmousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);    //최초로 클릭한 위치를 기억해요.
    }
    private void OnMouseDrag()      //이 카드를 드래그할때 기묘한 체험을 할거예요
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);    //마우스 움직이는 대로 카드도 움직여요
        transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
    }
    void OnMouseUp()    //이 카드를 손에서 논 순간 마법이 펼쳐져요
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapCircle(mousePos, 0.3f, enemyLayer);
        if (mousePos.y > FirstmousePos.y + 2F)
        {
            EventManager.NowEvent.EventChoiceCardActive(this.gameObject);
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
}