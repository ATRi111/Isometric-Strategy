using UnityEngine;

/// <summary>
/// 叠加方式
/// </summary>
public enum ESuperimposeMode
{
    Coexist,
    Refresh,
}

/// <summary>
/// 持续一段时间，影响角色属性（不应当影响参数）的对象
/// </summary>
[CreateAssetMenu(fileName = "新状态", menuName = "状态/默认状态")]
public class BuffSO : PawnModifierSO
{
    public int duration;
    public ESuperimposeMode superimposeMode = ESuperimposeMode.Refresh;

    //TODO:价值判断
    public float primitiveValue;

    protected override string TypeName => "此状态下可用的";

    public virtual void Tick(int startTime, int currentTime)
    {

    }
}
