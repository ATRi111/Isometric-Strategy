using Character;
using System;
using UnityEngine;

public enum EDamageType
{
    None,

}

public class HealthComponent : CharacterComponentBase
{
    public IntProperty maxHP;

    public Action<int, int> AfterHPChange;
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

    /// <summary>
    /// 返回预想中受到伤害后HP的值，但不会实际修改HP
    /// </summary>
    /// <returns></returns>
    public int MockDamage(PawnEntity source, EDamageType type, int damage)
    {
        //TODO:抗性
        return Mathf.Clamp(HP - damage, 0, maxHP.CurrentValue);
    }

    public void RefreshProperty()
    {
        maxHP.Refresh();
    }
}
