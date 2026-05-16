using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;

public class EnemyEffectViewManager : MonoBehaviour
{
    public GameObject EffectViewprefab;
    public GameObject Enemy;
    public List<string> EffectList = new List<string>();
    public List<GameObject> EffectViewObjectList = new List<GameObject>();
    void Start()
    {
        
    }
    public void AddEffectView(string Effect, float Count)
    {
        if (EffectList.Contains(Effect))
        {
            EffectViewObjectList[EffectList.IndexOf(Effect)].GetComponent<EffectView>().Count = Enemy.GetComponent<Enemy>().EffectDictionary[Effect];
            EffectViewObjectList[EffectList.IndexOf(Effect)].GetComponent<EffectView>().EffectCountText.GetComponent<TextMeshProUGUI>().text = Enemy.GetComponent<Enemy>().EffectDictionary[Effect].ToString();
            Debug.Log("효과 뷰 업데이트: " + Effect + " - " + Enemy.GetComponent<Enemy>().EffectDictionary[Effect]);
        }
        else
        {
            GameObject NewCostViewer = Instantiate(EffectViewprefab, gameObject.transform);
            EffectViewObjectList.Add(NewCostViewer);
            EffectList.Add(Effect);
            EffectViewObjectList[EffectList.IndexOf(Effect)].GetComponent<EffectView>().Count = Enemy.GetComponent<Enemy>().EffectDictionary[Effect];
            EffectViewObjectList[EffectList.IndexOf(Effect)].GetComponent<EffectView>().EffectCountText.GetComponent<TextMeshProUGUI>().text = Enemy.GetComponent<Enemy>().EffectDictionary[Effect].ToString();
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
