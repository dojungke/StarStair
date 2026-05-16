using System.Collections.Generic;
using UnityEngine;

public class EnemyManger : MonoBehaviour
{
    public BattleManager battleManager;
    public GameObject EnemyDummy;
    public GameObject EnemyPrefab;
    public Dictionary<string, List<List<string>>> stageEnemy = new Dictionary<string, List<List<string>>> { };
    public int stageEnemyCount = 0;
    void Start()
    {
        stageEnemy["forestStageEnemy"] = new List<List<string>>
        {
            new List<string> { "Enemy2Li", "EnemyAmurRatSnake" },
            new List<string> { "Enemy2Li", "Enemy2Li" },
            new List<string> { "EnemyRat", "Enemy2Li" },
            new List<string> { "EnemyAmurRatSnake", "EnemyAmurRatSnake" },
            new List<string> { "EnemyRat", "EnemyRat", "EnemyRat" },
        };
        stageEnemy["forestStageEliteEnemy"] = new List<List<string>>
        {
            new List<string> { "Enemy2Li", "EnemyFire2Li", "Enemy2Li" },
            new List<string> { "EnemyDhole", "EnemyDhole", "EnemyDhole" },
        };
        stageEnemy["forestStageBossEnemy"] = new List<List<string>>
        {
            new List<string> { "EnemyTheGreatSnake" },
            new List<string> { "EnemyForestSniper" },
        };
        stageEnemy["mountainStageEnemy"] = new List<List<string>>
        {
            new List<string> { "EnemyFireLizard", "EnemyFireLizard" },
            new List<string> { "EnemyEnt", "EnemyFireLizard" },
            new List<string> { "EnemyEnt", "EnemyDhole" },
            new List<string> { "EnemySmallStoneGolem", "EnemyFireLizard" },
            new List<string> { "EnemyFireLizard", "EnemySmallStoneGolem" },
        };
        stageEnemy["mountainStageEliteEnemy"] = new List<List<string>>
        {
            new List<string> { "EnemyStoneGolem", "EnemySmallStoneGolem" },
            new List<string> { "EnemyEnt", "EnemyEnt", "EnemyEnt" },
        };
        stageEnemy["mountainStageBossEnemy"] = new List<List<string>>
        {
            new List<string> { "EnemyLostOfBeast","EnemyDhole" },
        };
        stageEnemy["peakStageEnemy"] = new List<List<string>>
        {
            new List<string> { "EnemyFireLizard", "EnemyForestSniper" },
            new List<string> { "EnemyMegu", "EnemyFireLizard" },
            new List<string> { "EnemyTheGreatSnake", "EnemyPlantFairy" },
            new List<string> { "EnemyFire2Li","EnemyPlantFairy", "EnemyFire2Li" },
            new List<string> { "EnemySmallStoneGolem", "EnemySmallStoneGolem", "EnemyEnt" },
        };
        stageEnemy["peakStageEliteEnemy"] = new List<List<string>>
        {
            new List<string> { "EnemyMegu", "EnemyMegu", "EnemyMegu" },
            new List<string> { "EnemyImoogi" },
        };
        stageEnemy["peakStageBossEnemy"] = new List<List<string>>
        {
            new List<string> { "EnemyOldOdd","EnemyOldOdd2" },
        };
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void EnemySort()
    {
        List<GameObject> enemyObjectList = battleManager.GetComponent<BattleManager>().EnemyObjectList;
        //적 오브젝트 X축 4.5간격으로 가운데에서 부터 정렬하는 코드
        //적 오브젝트 생성될때 Enemy에서 자동으로 호출함
        int count = enemyObjectList.Count;
        if (count == 0) return;

        float spacing = 3f;
        float startX = 5+(-(spacing * (count - 1)) / 2f);

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = enemyObjectList[i].transform.position;
            pos.x = startX + i * spacing;
            enemyObjectList[i].transform.position = pos;
        }
    }
    public void EnemyAdd(string enemy)
    {
        EnemyPrefab = Resources.Load<GameObject>("prefab/Enemy/" + enemy);
        if(EnemyPrefab == null) { Debug.LogWarning($"{enemy}로딩 실패"); }
        GameObject newEnemy = Instantiate(EnemyPrefab, EnemyDummy.transform);
        newEnemy.GetComponent<Enemy>().BattleManager = battleManager.gameObject;
        stageEnemyCount += 1;
    }
    public void MultipleEnemyAdd(List<string> enenmyList)
    {
        foreach (string enemy in enenmyList)
        {
            EnemyAdd(enemy);
        }
    }
    public void RemoveEnemy(GameObject enemy)
    {
        if (battleManager.Turn.y != 0)
        {
            battleManager.NextTurn();
        }
        //적이 쓰러졌을때 1초뒤 제거
        StartCoroutine(EndTurnAfterDelay(0.6f, enemy));
    }
    private System.Collections.IEnumerator EndTurnAfterDelay(float delaySeconds, GameObject enemy)    //시간을 멈춰라 마이 월드야
    {
        yield return new WaitForSeconds(delaySeconds);
        stageEnemyCount -= 1;
        if (battleManager.GetComponent<BattleManager>().Hp <= 0)
        {
            delaySeconds = 0f;
        }
        if (stageEnemyCount <= 0)
        {
            Debug.Log("모든 적 쓰러짐");
            if (stageEnemyCount != -621)
            {
                stageEnemyCount = -621; //중복 실행 방지
                battleManager.GetComponent<BattleManager>().BattleEnd();
            }
        }
        else
        {
            if (battleManager.GetComponent<BattleManager>().TurnObject == enemy)
            {
                Debug.Log("sdasdas");
            }
        }
        battleManager.GetComponent<BattleManager>().EnemyObjectList.Remove(enemy);
        battleManager.GetComponent<BattleManager>().BattleUnitList.Remove(enemy);
        Destroy(enemy);
        EnemySort();
    }
}
