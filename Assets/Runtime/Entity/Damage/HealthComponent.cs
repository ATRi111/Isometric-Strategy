using Character;
using MyTool;
using System;
using UnityEngine;

public class HealthComponent : CharacterComponentBase
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

    /// <summary>
    /// ����Ԥ�����ܵ��˺���HP��ֵ��������ʵ���޸�HP
    /// </summary>
    /// <returns></returns>
    public int MockDamage(PawnEntity source, EDamageType type, int damage)
    {
        //TODO:����
        return Mathf.Clamp(HP - damage, 0, maxHP.CurrentValue);
    }

    public void RefreshProperty()
    {
        maxHP.Refresh();
    }
}
