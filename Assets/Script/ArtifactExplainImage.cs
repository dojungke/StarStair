using TMPro;
using UnityEngine;

public class ArtifactExplainImage : MonoBehaviour
{
    public TextMeshProUGUI artifactExplainName;
    public TextMeshProUGUI artifactExplainExplain;
    public TextMeshProUGUI artifactSellButtonText;
    public GameObject SellButton;
    public Artifact artifact;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (artifact.thisArtifact.Prize != -1)
        {
            artifactSellButtonText.text = $"ÆÇ¸Å {artifact.thisArtifact.Prize}º°ºû";
        }
        else
        {
            SellButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ArtifactSell()
    {
        artifact.artifactManager.SellArtifact(artifact);
    }
    public void ArtifactExplainOff()
    {
        artifact.artifactExplanationUIONOFF();
    }
}