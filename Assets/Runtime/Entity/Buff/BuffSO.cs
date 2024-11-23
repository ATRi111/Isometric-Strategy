using UnityEngine;

/// <summary>
/// 叠加方式
/// </summary>
public enum ESuperimposeMode
{
    Coexist,
    Refresh,
}

[CreateAssetMenu(fileName = "新状态", menuName = "状态/默认状态")]
public class BuffSO : PawnPropertyModifierSO
{
    public int duration;
    public ESuperimposeMode superimposeMode;
    public float primitiveValue;

    public virtual void Tick(int startTime, int currentTime)
    {

    }
}
