using UnityEngine;

/// <summary>
/// ���ӷ�ʽ
/// </summary>
public enum ESuperimposeMode
{
    Coexist,
    Refresh,
}

/// <summary>
/// ����һ��ʱ�䣬Ӱ���ɫ���ԣ���Ӧ��Ӱ��������Ķ���
/// </summary>
[CreateAssetMenu(fileName = "��״̬", menuName = "״̬/Ĭ��״̬")]
public class BuffSO : PawnModifierSO
{
    public int duration;
    public ESuperimposeMode superimposeMode = ESuperimposeMode.Refresh;

    //TODO:��ֵ�ж�
    public float primitiveValue;

    protected override string TypeName => "��״̬�¿��õ�";

    public virtual void Tick(int startTime, int currentTime)
    {

    }
}
