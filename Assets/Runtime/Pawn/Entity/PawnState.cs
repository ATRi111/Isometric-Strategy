public class PawnState
{
    public int hp;
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
