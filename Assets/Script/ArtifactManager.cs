using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum rarity
{
    common = 0,
    uncommon = 1,
    rare = 2,
    epic = 3,
    legendary = 4,
}
public class ArtifactManager : MonoBehaviour
{
    public int Gold = 0;
    public Hand handManager;
    public BattleManager battleManager;
    public CardAbilityManager cardAbilityManager;
    public List<Artifact> artifactList;
    public GameObject artifactPrefab;
    public GameObject artifactExplainImagePrefab;
    public GameObject artifactExplainImageObjecct;
    public TextMeshProUGUI artifactCountText;
    public int maxArtifact = 4;
    public int artifactCount = 0;
    public void Start()
    {
        AddArtifact("Gold");
        ArtifactActiveOnGoldChanged(Gold);
    }
    public void AddArtifact(string artifact)
    {
        artifactExplainImageObjecct = gameObject;
        GameObject newArtifact = Instantiate(artifactPrefab, gameObject.transform);
        newArtifact.AddComponent(Type.GetType(artifact));
        Artifact artifactScript = newArtifact.GetComponent<Artifact>();
        artifactScript.artifactManager = this;
        artifactScript.artifactName = artifact;
        artifactList.Add(artifactScript);
        artifactScript.thisArtifact = Resources.Load<ArtifactData>("ArtifactData/" + artifact);
        artifactCount += artifactScript.thisArtifact.Size;
        artifactCountText.text = $"{artifactCount}/{maxArtifact}";
        artifactScript.OnGet();
    }
    public void RemoveArtifact(Artifact removeArtifact)
    {
        removeArtifact.OnRemove();
        artifactCount -= removeArtifact.thisArtifact.Size;
        artifactCountText.text = $"{artifactCount}/{maxArtifact}";
        Destroy(removeArtifact.gameObject);
        artifactList.Remove(removeArtifact);
    }
    public void SellArtifact(Artifact sellArtifact)
    {
        Gold += sellArtifact.thisArtifact.Prize;
        RemoveArtifact(sellArtifact);
        ArtifactActiveOnGoldChanged(Gold);
    }
    public void GameEnd()
    {
        List<Artifact> artifactList2 = new List<Artifact>(artifactList);
        foreach (Artifact artifact in artifactList2)
        {
            if (artifact.artifactName != "Gold") { RemoveArtifact(artifact); }
        }
        Gold = 0;
        ArtifactActiveOnGoldChanged(0);
    }
    public void ArtifactActiveOnCardDrow(string activeCardData, Card activeCard)
    {
        foreach (Artifact artifact in artifactList)
        {
            artifact.OnCardDrow(activeCardData, activeCard);
            activeCard.CardSet();
        }
    }
    public void ArtifactActiveOnCardAttack(string activeCardData, Enemy targetEnemy, float damage)
    {
        foreach (Artifact artifact in artifactList)
        {
            artifact.OnCardAttack(activeCardData, targetEnemy, damage);
        }
    }
    public void ArtifactActiveOnCardActive(string activeCardData, int canUseItNow)
    {
        foreach (Artifact artifact in artifactList)
        {
            artifact.OnCardActive(activeCardData, canUseItNow);
        }
    }
    public void ArtifactActiveOnBattleStart()
    {
        foreach (Artifact artifact in artifactList)
        {
            artifact.OnBattleStart();
        }
    }
    public void ArtifactActiveOnBattleEnd()
    {
        foreach (Artifact artifact in artifactList)
        {
            artifact.OnBattleEnd();
        }
    }
    public void ArtifactActiveOnTurnStart()
    {
        foreach (Artifact artifact in artifactList)
        {
            artifact.OnTurnStart();
        }
    }
    public void ArtifactActiveOnTurnEnd()
    {
        foreach (Artifact artifact in artifactList)
        {
            artifact.OnTurnEnd();
        }
    }
    public void ArtifactActiveOnGoldChanged(int changeGold)
    {
        foreach (Artifact artifact in artifactList)
        {
            artifact.OnGoldChanged(Gold);
        }
    }
}

public abstract class Artifact : MonoBehaviour
{
    public string artifactName;
    public rarity artifactRarity;
    public string artifactDescription;
    public Sprite artifactImage;
    public int artifactSize;
    public ArtifactManager artifactManager;
    public GameObject artifactExplainImageObjecct;
    public ArtifactData thisArtifact;
    public virtual void Start()
    {
        artifactRarity = thisArtifact.Rarity;
        artifactDescription = thisArtifact.Description;
        artifactImage = thisArtifact.Image;
        artifactSize = thisArtifact.Size;
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(artifactExplanationUIONOFF);
        gameObject.GetComponent<ArtifactButton>().artifactImage.sprite = artifactImage;
    }
    public virtual void OnCardDrow(string activeCardData, Card activeCard) { }
    public virtual void OnBattleStart() { }
    public virtual void OnBattleEnd() { }
    public virtual void OnTurnStart() { }
    public virtual void OnNowCardChange(string activeCardData) { }
    public virtual void OnTurnEnd() { }
    public virtual void OnHit(string activeDamageData, Unit activeUnitData) { }
    public virtual void OnCardAttack(string activeCardData, Enemy activeEnemyData, float damage) { }
    public virtual void OnCardActive(string activeCardData, int canUseItNow) { }
    public virtual void OnEnemyDeath(Enemy activeEnemyData) { }
    public virtual void OnGet() { }
    public virtual void OnRemove() { }
    public virtual void OnGoldChanged(int Gold) { }
    public void artifactExplanationUIONOFF()
    {
        if (artifactExplainImageObjecct == artifactManager.gameObject)
        {
            artifactExplainImageObjecct = Instantiate(artifactManager.artifactExplainImagePrefab, transform);
            ArtifactExplainImage artifactExplainImage = artifactExplainImageObjecct.GetComponent<ArtifactExplainImage>();
            artifactExplainImage.artifact = this;
            artifactExplainImage.artifactExplainName.text = thisArtifact.Name;
            artifactExplainImage.artifactExplainExplain.text = thisArtifact.Description;
            artifactExplainImageObjecct.transform.localPosition = new Vector3(60, -80, 0);
        }
        else
        {
            Destroy(artifactExplainImageObjecct);
            artifactExplainImageObjecct = artifactManager.gameObject;
        }
    }
}
public class Gold : Artifact
{
    public override void OnGoldChanged(int Gold)
    {
        gameObject.GetComponent<ArtifactButton>().artifactText.text = $"{Gold}";
    }
}
public class ShapeSword : Artifact
{
    override public void OnCardDrow(string activeData, Card activeCard)
    {
        base.OnCardDrow(activeData, activeCard);
        string cardColor = activeData.Split('/')[1];
        activeCard.additionalDamage += 2;
    }
}
public class SpaceTimeRing : Artifact
{
    override public void OnCardDrow(string activeData, Card activeCard)
    {
        base.OnCardDrow(activeData, activeCard);
        string cardColor = activeData.Split('/')[1];
        if (cardColor == "S" || cardColor == "T")
        {
            activeCard.additionalDamage += 4;
        }
    }
}
public class RedRing : Artifact
{
    override public void OnCardDrow(string activeData, Card activeCard)
    {
        base.OnCardDrow(activeData, activeCard);
        string cardColor = activeData.Split('/')[1];
        if (cardColor == "R")
        {
            activeCard.additionalDamage += 4;
        }
    }
}
public class BlueRing : Artifact
{
    override public void OnCardDrow(string activeData, Card activeCard)
    {
        base.OnCardDrow(activeData, activeCard);
        string cardColor = activeData.Split('/')[1];
        if (cardColor == "B")
        {
            activeCard.additionalDamage += 4;
        }
    }
}
public class GreenRing : Artifact
{
    override public void OnCardDrow(string activeData, Card activeCard)
    {
        base.OnCardDrow(activeData, activeCard);
        string cardColor = activeData.Split('/')[1];
        if (cardColor == "G")
        {
            activeCard.additionalDamage += 4;
        }
    }
}
public class GreenDice : Artifact
{
    override public void OnCardDrow(string activeData, Card activeCard)
    {
        base.OnCardDrow(activeData, activeCard);
        string cardColor = activeData.Split('/')[1];
        if (cardColor == "G")
        {
            int RandomNum = UnityEngine.Random.Range(1, 7) + UnityEngine.Random.Range(1, 7);
            activeCard.reinforceList.Add(new CardReinforce { reinforceType = CardAbilityType.Attack, reinforceRate = RandomNum * 10 });
        }
    }
}
public class FireSword : Artifact
{
    public override void OnCardAttack(string activeCardData, Enemy activeEnemyData, float damage)
    {
        base.OnCardAttack(activeCardData, activeEnemyData, damage);
        if (activeCardData.Split("/")[1] == "R" && damage > 0)
        {
            activeEnemyData.AddEffect("Fire", damage);
        }
    }
}
public class MagicGloves : Artifact
{
    public override void OnGet()
    {
        base.OnGet();
        artifactManager.handManager.NumberOfTurnStartDorwHandCard += 1;
    }
    public override void OnRemove()
    {
        base.OnGet();
        artifactManager.handManager.NumberOfTurnStartDorwHandCard -= 1;
    }
}
public class ManaEngineeringEngin : Artifact
{
    public int power = 0;
    public override void Start()
    {
        base.Start();
        gameObject.GetComponent<ArtifactButton>().artifactText.text = $"{power}";
    }
    public override void OnTurnEnd()
    {
        foreach (GameObject handCard in artifactManager.handManager.HandCard)
        {
            Card nowCard = handCard.GetComponent<Card>();
            nowCard.additionalDamage -= power;
            nowCard.CardSet();
        }
        power = 0;
        gameObject.GetComponent<ArtifactButton>().artifactText.text = $"{power}";
    }
    public override void OnBattleEnd()
    {
        power = 0;
        gameObject.GetComponent<ArtifactButton>().artifactText.text = $"{power}";
    }
    public override void OnCardDrow(string activeCardData, Card activeCard)
    {
        activeCard.additionalDamage += power;
        activeCard.CardSet();
    }
    public override void OnCardActive(string activeCardData, int canUseItNow)
    {
        power += 5;
        foreach (GameObject handCard in artifactManager.handManager.HandCard)
        {
            Card nowCard = handCard.GetComponent<Card>();
            nowCard.additionalDamage += 5;
            nowCard.CardSet();
        }
        gameObject.GetComponent<ArtifactButton>().artifactText.text = $"{power}";
    }
}
public class BlueWave : Artifact
{
    public override void OnCardDrow(string activeCardData, Card activeCard)
    {
        if (activeCardData.Split('/')[1] == "B") { activeCard.additionalDamage += 3; }
        activeCard.CardSet();
    }
    public override void OnCardAttack(string activeCardData, Enemy activeEnemyData, float damage)
    {
        if (activeCardData.Split('/')[1] == "B")
        {
            foreach (GameObject enemy in artifactManager.battleManager.EnemyObjectList)
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                enemyScript.ManaGuard = 0;
                enemyScript.Hit((int)damage/2);
                enemyScript.HpBarSort();
            }
        }
    }
}