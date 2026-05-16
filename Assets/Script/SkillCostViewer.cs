using UnityEngine;

public class SkillCostViewer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string Name;
    public GameObject SkillCostViewerText;
    public int MaxCost;
    public int NowCost;
    CardData ThisCard;
    public bool CostPaying = false;
    void Start()
    {

    }
}
