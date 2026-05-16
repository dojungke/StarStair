using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillCostViewerManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<string> SkillCostViewers;
    public List<GameObject> SkillCostViewersObject;
    GameObject SkillCostViewerprefab;
    public GameObject skillCostViewerContent;
    public GameObject CardAbilityManager;
    int IndexOfObject;
    public SkillCostViewer skillCostViewer;
    public CardAbilityManager cardAbilityManager;
    void Start()
    {
        SkillCostViewerprefab = Resources.Load<GameObject>("prefab/SkillCostViewer Button");
        cardAbilityManager = CardAbilityManager.GetComponent<CardAbilityManager>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public bool SkillCoolTimeAdd(string CardName, int addcost = 1)
    {
        CardData ThisCard = Resources.Load<CardData>("DeckOfAll/" + CardName);
        if (SkillCostViewers.Contains(CardName))
        {
            IndexOfObject = SkillCostViewers.IndexOf(CardName);
            GameObject CostViewerObject = SkillCostViewersObject[IndexOfObject];
            skillCostViewer = CostViewerObject.GetComponent<SkillCostViewer>();
            skillCostViewer.NowCost += 1;
            if (skillCostViewer.NowCost >= skillCostViewer.MaxCost)
            {
                //카드 실행 가능 여부 및 실행은 카드 어빌리티 메니저가 수행한다. 여기서는 리스트 삭제, 오브젝트 파괴만 진행
                SkillCostViewers.Remove(CardName);
                SkillCostViewersObject.Remove(SkillCostViewersObject[IndexOfObject]);
                Destroy(CostViewerObject);
                return (true);
            }
            else
            {
                skillCostViewer.SkillCostViewerText.GetComponent<TextMeshProUGUI>().text = CardName+"("+skillCostViewer.NowCost+"/"+skillCostViewer.MaxCost+")";
                return (false);
            }
        }
        else
        {
            if( addcost == 0 )return true;
            //새로운 카드 코스트 확인 UI를 생성한다.
            GameObject NewCostViewer = Instantiate(SkillCostViewerprefab, Vector3.zero, Quaternion.identity, skillCostViewerContent.transform);
            skillCostViewer = NewCostViewer.GetComponent<SkillCostViewer>();
            skillCostViewer.Name = CardName;
            SkillCostViewersObject.Add(NewCostViewer);
            SkillCostViewers.Add(CardName);
            skillCostViewer.NowCost = 1;
            skillCostViewer.MaxCost = ThisCard.coolTime;
            skillCostViewer.SkillCostViewerText.GetComponent<TextMeshProUGUI>().text = CardName + "(" + "1" + "/" + ThisCard.coolTime + ")";
            return (false);
        }
    }
    public void SkillCostViewerClear()
    {
        foreach (GameObject CostViewerObject in SkillCostViewersObject)
        {
            Destroy(CostViewerObject);
        }
        SkillCostViewers.Clear();
        SkillCostViewersObject.Clear();
    }
}