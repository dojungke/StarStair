using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public TMP_InputField inputField; // 인스펙터에서 할당
    public CardGetManager cardGetManager;
    public EnemyManger enemyManger; // 인스펙터에서 할당
    public StageManager stageManager; // 인스펙터에서 할당
    public Hand handManager; // 인스펙터에서 할당
    public BattleManager battleManager; // 인스펙터에서 할당
    public ArtifactManager artifactManager; // 인스펙터에서 할당
    public void CommandActive(string commandName)
    {
        if (commandName.Contains("GetCardActive"))
        {
            string[] getCards = commandName.Split(' ');
            getCards = getCards[1..]; // 첫 번째 요소는 명령어 이름이므로 제외
            List<string> GetCardList= new List<string>(getCards);
            cardGetManager.GetCardActive(GetCardList);
        }
        else if (commandName.Contains("PlayStart"))
        {
            stageManager.PlayStart();
        }
        else if (commandName.Contains("Drow"))
        {
            string[] drowCommand = commandName.Split(' ');
            handManager.Drow(int.Parse(drowCommand[1]), "명령어");
        }
        else if (commandName.Contains("StageDrow"))
        {
            string[] drowCommand = commandName.Split(' ');
            stageManager.Drow(int.Parse(drowCommand[1]), "명령어");
        }
        else if (commandName.Contains("kill"))
        {
            List<GameObject> enemyObjectList = new List<GameObject>(battleManager.EnemyObjectList);
            foreach (GameObject enemy in enemyObjectList)
            {
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    enemyComponent.HpDamage(9999999);
                }
            }
        }
        else if (commandName.Contains("Hp"))
        {
            battleManager.Hp = battleManager.MaxHp;
            battleManager.HpBarSort();
        }
        else if (commandName.Contains("GiveArtifact"))
        {
            artifactManager.AddArtifact(commandName.Split(' ')[1]);
        }
    }


    public void CommandActiveButtonClick()
    {
        CommandActive(inputField.text);
        inputField.text = ""; // 입력 필드 초기화
    }
}
