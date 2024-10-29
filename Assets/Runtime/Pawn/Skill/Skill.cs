using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public int actionTime;

    /// <summary>
    /// 获取可选的技能施放位置
    /// </summary>
    public virtual void GetOptions(Vector2Int position, List<Vector2Int> ret)
    {
        ret.Clear();
    }

    /// <summary>
    /// 模拟技能可能产生的效果
    /// </summary>
    public virtual void Mock(PawnEntity agent, Vector2Int target,IsometricGridManager igm, EffectUnit ret)
    {
        ret.timeEffect.current += actionTime;
    }
}
