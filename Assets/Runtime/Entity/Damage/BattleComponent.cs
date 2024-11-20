using Character;
using MyTool;
using System;
using UnityEngine;

public class BattleComponent : CharacterComponentBase
{
    public IntProperty maxHP;

    public Action<int, int> AfterHPChange;
    [SerializeField]
    private int hp;
    public int HP
    {
        get => hp;
        set
        {
            hp = Mathf.Clamp(value, 0, maxHP.CurrentValue);
            if (hp != value)
            {
                int prev = hp;
                hp = value;
                AfterHPChange?.Invoke(prev, hp);
            }
        }
    }

    public SerializedDictionary<EDamageType, FloatProperty> resistance;

    protected override void Awake()
    {
        base.Awake();
        resistance.Refresh();
    }

    /// <summary>
    /// 返回预想中受到伤害后HP的值，但不会实际修改HP
    /// </summary>
    public int MockDamage(PawnEntity source, EDamageType type, int damage)
    {
        damage = Mathf.RoundToInt(damage * (1f - resistance[type].CurrentValue));
        return Mathf.Clamp(HP - damage, 0, maxHP.CurrentValue);
    }

    public void Refresh()
    {
        maxHP.Refresh();
    }
}
