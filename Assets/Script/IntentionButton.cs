using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntentionButton : MonoBehaviour
{
    public Enemy owner;
    public Image image;
    public TextMeshProUGUI text;
    public ImageButtonType type;
    public EffectData effectData;
    public Sprite intentionCoolDownImage;
    public Sprite attackImage;
    public Sprite healImage;
    public Sprite multiHealImage;
    public Sprite summonImage;
    public Sprite getManaGuardImage;
    private bool destroy;
    public void Start()
    {
        ImageUpdate();
    }
    public void ImageUpdate()
    {
        switch (type)
        {
            case ImageButtonType.IntentionCooltime:
                image.sprite = intentionCoolDownImage;
                text.color = Color.black;
                text.transform.localPosition = new Vector3(-2, 2, 0);
                text.text = $"{owner.intentionManager.CoolTime}";   //텍스트 자체 변경
                break;
            case ImageButtonType.Attack:    //텍스트는 owner에서 변경
                image.sprite = attackImage;
                break;
            case ImageButtonType.Heal:  //텍스트는 owner에서 변경
                image.sprite = healImage;
                break;
            case ImageButtonType.MultiHeal:
                image.sprite = multiHealImage;
                break;
            case ImageButtonType.Effect:    //텍스트는 owner에서 넘겨줌 effectData도 owner에서 넘겨줌
                image.sprite = effectData.Image;
                break;
            case ImageButtonType.Summon:
                image.sprite = summonImage;
                break;
            case ImageButtonType.GetManaGuard:
                image.sprite = getManaGuardImage;
                break;
        }
    }
    public void DestroySelf()
    {
        if (destroy) { }
        else { Destroy(gameObject); destroy = true; }
    }
}
public enum ImageButtonType
{
    IntentionCooltime,
    Attack,
    Heal,
    MultiHeal,
    Effect,
    Summon,
    GetManaGuard
}

