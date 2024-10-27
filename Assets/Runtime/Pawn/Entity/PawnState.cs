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
    /// 总计时器的值达到此值时，轮到此角色行动
    /// </summary>
    public int waitTime;
}
