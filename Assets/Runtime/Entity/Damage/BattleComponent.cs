using Character;
using MyTool;
using System;
using UnityEngine;

public class BattleComponent : CharacterComponentBase
{
    public CharacterProperty maxHP;

    public Action<int, int> AfterHPChange;
    [SerializeField]
    private int hp;
    public int HP
    {
        get => hp;
        set
        {
            value = Mathf.Clamp(value, 0, maxHP.IntValue);
            if (hp != value)
            {
                int prev = hp;
                hp = value;
                AfterHPChange?.Invoke(prev, hp);
            }
        }
    }

    public SerializedDictionary<EDamageType, CharacterProperty> resistance;

    public void Initialize()
    {
        resistance.Refresh();
        HP = maxHP.IntValue;
    }

    /// <summary>
    /// 返回预想中受到伤害后HP的减少量，但不会实际修改HP
    /// </summary>
    public int MockDamage(EDamageType type, int attackPower)
    {
        attackPower = Mathf.RoundToInt(attackPower / resistance[type].CurrentValue);
        return attackPower;
    }

    public void Refresh()
    {
        maxHP.Refresh();
        foreach(CharacterProperty property in resistance.Values)
        {
            property.Refresh();
        }
    }
}
