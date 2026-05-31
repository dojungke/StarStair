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
        hpSlider.value = unit.hp / (unit.CONstitution * 10);
        if ((int)unit.hp <= 0) { hpText.text = "down!"; }
        else { hpText.text = $"{unit.hp}/{unit.CONstitution * 10}"; }
    }
}
