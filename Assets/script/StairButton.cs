using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StairButton : MonoBehaviour
{
    public string stairName;
    public TextMeshProUGUI stairNameText;
    public TextMeshProUGUI stairCostText;
    public Image stairImage;
    public StairData data;
    public TextMeshProUGUI stepNameText;
    public TextMeshProUGUI stepDescriptionText;
    public Transform actionContent;
    List<ActionButton> actionList = new List<ActionButton>();
    public int nowStep = 0;

    public void ButtonSetting()
    {
        data = (StairData)Resources.Load($"StairData/{stairName}");
        stairNameText.text = data.stairName;
        stairImage.sprite = data.Image;
        stepNameText.text = data.StepInfo[0].stepName;
        stepDescriptionText.text = data.StepInfo[0].stepDescription;
        stairCostText.text = $"{data.StepInfo.Count}";
        ActionSetting();
    }
    public void Clicked()
    {
        StairHandManager.Instance.StairSelect(stairName);
    }
    public void StepInfo(int select)
    {
        if(data == null && stairName != null) { ButtonSetting(); }
        nowStep += select;
        if (nowStep >= data.StepInfo.Count) 
        {
            nowStep = data.StepInfo.Count - 1;
        }
        else if (nowStep < 0)
        {
            nowStep = 0;
        }
        stepNameText.text = $"{data.StepInfo[nowStep].stepName} ({nowStep+1}/{data.StepInfo.Count})";
        stepDescriptionText.text = data.StepInfo[nowStep].stepDescription;
        ActionSetting();
    }
    void ActionSetting()
    {
        foreach (ActionButton actionButton in actionList) 
        {
            Destroy(actionButton.gameObject);
        }
        actionList.Clear();
        foreach (StepAction action in data.StepInfo[nowStep].actions)
        {
            List<ActionType> dontView  = new List<ActionType> { ActionType.Dummy, ActionType.Move, ActionType.PlayAnimation };
            if (dontView.Contains(action.actionType))
            { 
                continue;
            }
            ActionButton newActionButton = Instantiate(StairHandManager.Instance.actionButtonPrefab, actionContent).GetComponent<ActionButton>();
            actionList.Add(newActionButton);
            newActionButton.ActionText.text = action.value;
            switch (action.actionType)
            {
                case ActionType.SingleAttack:
                    newActionButton.ActionImage.sprite = Resources.Load<Sprite>("Icons/Attack");
                    break;

                case ActionType.Quick:
                    newActionButton.ActionImage.sprite = Resources.Load<Sprite>("Icons/Quickless");
                    break;
            }
        }
    }
}
