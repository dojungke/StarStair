using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RelatedEffectViewer : MonoBehaviour
{
    public List<GameObject> effectViewerList = new List<GameObject>();
    public GameObject effectViewerPrefab;
    public List<EffectData> effectDatas = new List<EffectData>();
    public void RelatedEffectView(List<EffectData> effectDataList)
    {
        if (effectDataList.SequenceEqual(effectDatas) == false)
        {
            foreach (GameObject effectViewObject in effectViewerList)
            {
                Destroy(effectViewObject);
            }
            effectViewerList.Clear();
            effectDatas = new List<EffectData>(effectDataList);

            foreach (EffectData effectData in effectDataList)
            {
                GameObject newEffectView = Instantiate(effectViewerPrefab, gameObject.transform);
                effectViewerList.Add(newEffectView);
                newEffectView.GetComponent<EffectView>().Name = effectData.Name;
                newEffectView.GetComponent<EffectView>().ThisEffect = effectData;
                newEffectView.GetComponent<EffectView>().EffectCountText.GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }
}