using TMPro;
using UnityEngine;

public class EffectExplainImage : MonoBehaviour
{
    public TextMeshProUGUI effectExplainName;
    public TextMeshProUGUI effectExplainExplain;
    public GameObject effectView;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void EffectExplainOff()
    {
        effectView.GetComponent<EffectView>().EffectExplanationUIONOFF();
    }
}
