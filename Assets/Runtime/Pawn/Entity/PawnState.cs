using System;

public class PawnState
{
    public Action<int, int> AfterHPChange;
    private int hp;
    public int HP
    {
        get => hp;
        set
        {
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

    public PawnState(int hp, int waitTime)
    {
        this.hp = hp;
        this.waitTime = waitTime;
    }
}
