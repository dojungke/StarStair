using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string Name;
    public float MaxHp;
    public float Hp;
    public float ManaGuard;
    public Dictionary<string, float> EffectDictionary;
    public bool Stun;
    public bool bindActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Awake()
    {
        EffectDictionary = new Dictionary<string, float>();
    }
    public void EffectActive()
    {
        List<string> NowEffect = new List<string>(EffectDictionary.Keys);
        //EffectDictionary에 있는 효과를 모두 적용한다
        foreach (string Effect in NowEffect)
        {
            switch (Effect.Split("/")[0])
            {
                case "Fire":
                    //갯수만큼 피해를 입히고 반으로 줄어든다(소수점 버림)
                    if (EffectDictionary.ContainsKey("Kindling")) //불쏘시게 보유 여부 판별. 카드이름과 버프이름이 살짝 다르니 주의할것! Kinding, Kindling
                    {
                        EffectDictionary["Fire"] += 4;
                        Hit(EffectDictionary["Fire"]); EffectDictionary["Kindling"] -= 1; break; //불쏘시게가 있을 경우 불쏘시게 효과로 Fire 효과가 4 증가하고, Kindling 효과는 1 감소한다
                    }
                    else
                    {
                        Hit(EffectDictionary["Fire"]);
                        EffectDictionary["Fire"] = (int)(EffectDictionary["Fire"] / 2); break;
                    }
                case "Bleeding":
                    //8만큼 방어무시 피해를 입히고 1 줄어든다. 여러게를 부여할수 있다.
                    HpDamage(8);
                    EffectDictionary[Effect] -= 1; break;
                case "Stun":
                    //스턴 상태일 경우 턴을 넘긴다
                    Debug.Log("스턴 상태로 턴을 넘김");
                    Stun = true; EffectDictionary[Effect] -= 1; break;
                case "Scales":
                    if (ManaGuard <= 0) { ManaGuard = (int)EffectDictionary["Scales"]; HpBarSort(); }
                    break;
                case "Regeneration":
                    Hp += EffectDictionary[Effect]; HpBarSort(); break;
                case "Bind":
                    BattleManager battleManager = gameObject.GetComponent<BattleManager>();
                    EffectDictionary[Effect] -= 1;
                    if (battleManager != null && bindActive == false)
                    {
                        bindActive = true;
                        battleManager.handManager.NumberOfTurnStartDorwHandCard -= 1;
                    } //턴시작 잎 개수 복원은 턴종료시에 검사하고 발동
                    break;
            }
            EffectCountUpdate(Effect); //효과 업데이트
        }
        if (Stun)
        {
            Stun = false; //스턴 상태는 턴이 끝나면 풀린다
            TurnEnd(); //턴 종료
        }
    }
    public float HpDamage(float Damage)
    {
        Hp -= Damage;
        HpBarSort();
        return Damage;
    }
    public virtual float Hit(float Damage)
    {
        ManaGuard -= Damage;
        if (ManaGuard < 0)
        {
            float HitDamage = 0;
            Debug.Log($"{Name} 마나가드 파괴됨");
            HitDamage = -ManaGuard;
            Hp -= HitDamage;
            Debug.Log($"{Name}에게 " + HitDamage + "의 피해를 주었다. 남은 체력 " + Hp);
            ManaGuard = 0;
            if (Hp <= 0)
            {
                Debug.Log($"{Name} 쓰러짐");
            }
            HpBarSort();
            return HitDamage;
        }
        else
        {
            Debug.Log($"{Name} 마나 가드에 " + Damage + "의 피해를 주었다. 남은 마나가드 " + ManaGuard + "남은 체력 " + Hp);
            HpBarSort();
            return 0;
        }
    }
    public virtual void HpBarSort() { } //Enemy, BattleManager에서 적은거 쓸거임
    public virtual void TurnEnd(int times = 1) { } //Enemy나 BattleManager에서 적은거 쓸거임
    public virtual void AddEffect(string Effect, float Count) { } //Enemy나 BattleManager에서 적은거 쓸거임
    public virtual void RemoveEffect(string Effect) { } //Enemy나 BattleManager에서 적은거 쓸거임
    public virtual void EffectCountUpdate(string Effect)
    {
        if (EffectDictionary[Effect] <= 0)
        {
            RemoveEffect(Effect);
        }
        else
        {
            Debug.Log(Name + "의효과 " + Effect + "의 갯수가 " + EffectDictionary[Effect] + "로 업데이트 되었습니다.");
            AddEffect(Effect, 0);
        }
    }
}