using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public int actionTime;

    public static int DistanceBetween(Vector3Int position, Vector3Int target)
    {
        return IsometricGridUtility.ProjectManhattanDistance((Vector2Int)position, (Vector2Int)target);
    }

    /// <summary>
    /// 获取可选的技能施放位置
    /// </summary>
    public virtual void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        ret.Clear();
    }

    /// <summary>
    /// 模拟技能可能产生的效果
    /// </summary>
    public virtual void Mock(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        ret.timeEffect.current += MockTime(agent, position, target, igm);
    }
    /// <summary>
    /// 模拟技能花费的时间
    /// </summary>
    public virtual int MockTime(PawnEntity agent, Vector3Int position, Vector3Int target, IsometricGridManager igm)
    {
        return actionTime;
    }
}
