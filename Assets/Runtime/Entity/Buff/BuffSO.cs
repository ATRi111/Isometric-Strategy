using UnityEngine;

/// <summary>
/// ���ӷ�ʽ
/// </summary>
public enum ESuperimposeMode
{
    Coexist,
    Refresh,
}

[CreateAssetMenu(fileName = "��״̬", menuName = "״̬/Ĭ��״̬")]
public class BuffSO : PawnPropertyModifierSO
{
    public int duration;
    public ESuperimposeMode superimposeMode;
    public float primitiveValue;

    public virtual void Tick(int startTime, int currentTime)
    {

    }
}
