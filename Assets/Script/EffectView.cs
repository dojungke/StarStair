using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectView : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string Name;
    public GameObject EffectViewText;
    public GameObject EffectCountText;
    public GameObject EffectExplainImagePrefab;
    public Image EffectImage;
    public float Count;
    public EffectData ThisEffect;
    bool EffectExplainOn = false;
    GameObject newEffectExplainImage;
    void Start()
    {
        newEffectExplainImage = gameObject;
        if (ThisEffect == null)
        {
            ThisEffect = Resources.Load<EffectData>($"Effect/{Name.Split("/")[0]}");
        }
        if (ThisEffect.Image == null)
        {
            EffectViewText.GetComponent<TextMeshProUGUI>().text = Name;
            EffectViewText.SetActive(true);
            EffectImage.gameObject.SetActive(false);
        }
        else
        {
            EffectImage.sprite = ThisEffect.Image;
            EffectViewText.SetActive(false);
            EffectImage.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    public void EffectExplanationUIONOFF()
    {
        if (EffectExplainOn == false)
        {
            EffectExplainOn = true;
            newEffectExplainImage = Instantiate(EffectExplainImagePrefab, gameObject.transform);
            newEffectExplainImage.GetComponent<EffectExplainImage>().effectView = gameObject;
            newEffectExplainImage.GetComponent<EffectExplainImage>().effectExplainName.text = ThisEffect.Name;
            newEffectExplainImage.GetComponent<EffectExplainImage>().effectExplainExplain.text = ThisEffect.Description;
        }
        else
        {
            EffectExplainOn = false;
            Destroy(newEffectExplainImage);
        }
    }
}
