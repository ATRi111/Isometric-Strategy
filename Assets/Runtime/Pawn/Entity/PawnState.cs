using System;
using UnityEngine;

[Serializable]
public class PawnState
{
    public Func<int> MaxHP;
    public Action<int, int> AfterHPChange;
    private int hp;
    public int HP
    {
        get => hp;
        set
        {
            hp = Mathf.Clamp(value, 0, MaxHP());
            if(hp != value)
            {
                int prev = hp;
                hp = value;
                AfterHPChange?.Invoke(prev, hp);
            }
        }
    }

    /// <summary>
    /// �ܼ�ʱ����ֵ�ﵽ��ֵʱ���ֵ��˽�ɫ�ж�
    /// </summary>
    public int waitTime;
}
