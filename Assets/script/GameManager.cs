using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool quickBattle = false;
    public static GameManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
