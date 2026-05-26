using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StairButton : MonoBehaviour
{
    public string stairName;
    public TextMeshProUGUI stairNameText;
    public Image stairImage;
    public StairData data;

    public void buttonSetting() 
    {
        data = (StairData)Resources.Load($"StairData/{stairName}");
        UnityEngine.Debug.Log(data.stairName);
        stairNameText.text = data.stairName;
        stairImage.sprite = data.Image;
    }
    public void clicked() 
    {
        StairHandManager.Instance.StairSelect(stairName);
    }
}
