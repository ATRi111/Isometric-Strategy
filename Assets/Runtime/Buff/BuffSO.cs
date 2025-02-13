using UnityEngine;

/// <summary>
/// 叠加方式
/// </summary>
public enum ESuperimposeMode
{
    Coexist,
    Refresh,
}

public enum ERemoveFlag
{
    Unremovable,
    Buff,
    Debuff,
}

/// <summary>
/// 持续一段时间，影响角色属性（不应当影响参数）的对象
/// </summary>
[CreateAssetMenu(fileName = "新状态", menuName = "状态/默认状态")]
public class BuffSO : PawnModifierSO
{
    public int duration;
    public ESuperimposeMode superimposeMode = ESuperimposeMode.Refresh;
    public ERemoveFlag removeFlag = ERemoveFlag.Unremovable;

    public float primitiveValue;
    public ValueModifier modifier;

    protected override string TypeName => "此状态下可用的";

    /// <summary>
    /// 假设victim被施加了此状态，由victim考虑此事对自身的价值
    /// </summary>
    public float ValueForVictim(PawnEntity victim)
    {
        if (modifier != null)
            return modifier.CalculateValue(primitiveValue, victim);
        return primitiveValue;
    }

    public virtual void Tick(int startTime, int currentTime)
    {

    }
}
