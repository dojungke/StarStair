using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactButton : MonoBehaviour
{
    public Image artifactImage;
    public TextMeshProUGUI artifactText;
    public Artifact artifact;
    public void ImageRemove()
    {
        artifact.artifactExplanationUIONOFF();
    }
}