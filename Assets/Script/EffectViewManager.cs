using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;

public class EffectViewManager : MonoBehaviour
{
    public GameObject EffectViewprefab;
    public GameObject BattleManager;
    public List<string> EffectList;
    public List<GameObject> EffectViewObjectList;
    void Start()
    {
        //EffectViewprefab = Resources.Load<GameObject>("prefab/EffectView Button");
        EffectList = new List<string>();
        EffectViewObjectList = new List<GameObject>();
    }
    public void AddEffectView(string Effect, float Count)
    {
        if (EffectList.Contains(Effect))
        {
            EffectViewObjectList[EffectList.IndexOf(Effect)].GetComponent<EffectView>().Count = BattleManager.GetComponent<BattleManager>().EffectDictionary[Effect];
            EffectViewObjectList[EffectList.IndexOf(Effect)].GetComponent<EffectView>().EffectCountText.GetComponent<TextMeshProUGUI>().text = BattleManager.GetComponent<BattleManager>().EffectDictionary[Effect].ToString();
        }
        else
        {
            GameObject NewCostViewer = Instantiate(EffectViewprefab, gameObject.transform);
            EffectViewObjectList.Add(NewCostViewer);
            EffectList.Add(Effect);
            EffectViewObjectList[EffectList.IndexOf(Effect)].GetComponent<EffectView>().Count = BattleManager.GetComponent<BattleManager>().EffectDictionary[Effect];
            EffectViewObjectList[EffectList.IndexOf(Effect)].GetComponent<EffectView>().EffectCountText.GetComponent<TextMeshProUGUI>().text = BattleManager.GetComponent<BattleManager>().EffectDictionary[Effect].ToString();
            EffectViewObjectList[EffectList.IndexOf(Effect)].GetComponent<EffectView>().Name = Effect;
        }
    }
    public void RemoveEffectView(string Effect)
    {
        if (EffectList.Contains(Effect))
        {
            int index = EffectList.IndexOf(Effect);
            Destroy(EffectViewObjectList[index]);
            EffectViewObjectList.RemoveAt(index);
            EffectList.RemoveAt(index);
        }
        else
        {
            Debug.Log("에러: 효과 뷰 제거 실패, 해당 효과 없음");
        }
    }
}
