using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Slider hpSlider;
    public TextMeshProUGUI hpText;
    public Unit unit;
    public void HpBarSetting() 
    {
        hpSlider.value = unit.hp/unit.maxHP;
        hpText.text = $"{unit.hp}/{unit.maxHP}";
    }
}
