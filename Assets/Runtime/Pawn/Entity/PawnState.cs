public class PawnState
{
    public int hp;
    /// <summary>
    /// �ܼ�ʱ����ֵ�ﵽ��ֵʱ���ֵ��˽�ɫ�ж�
    /// </summary>
    public int waitTime;

    public PawnState(int hp, int waitTime)
    {
        this.hp = hp;
        this.waitTime = waitTime;
    }
}
