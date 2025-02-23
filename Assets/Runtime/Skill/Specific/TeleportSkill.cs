using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "传送", menuName = "技能/特殊/传送")]
public class TeleportSkill : RangedSkill
{
    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        base.GetOptions(agent, igm, position, ret);
        List<Vector2Int> primitive = IsometricGridUtility.WithinProjectManhattanDistance(castingDistance);
        for (int i = 0; i < primitive.Count; i++)
        {
            Vector2Int temp = (Vector2Int)position + primitive[i];
            if (!igm.Contains(temp))    //判断是否在地图内
                continue;
            GridObject gridObject = igm.GetObjectXY(temp);
            if (!gridObject.IsGround)
                continue;
            Vector3Int target = igm.AboveGroundPosition(temp);
            ret.Add(target);
        }
    }

    public override void Mock(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        base.Mock(agent, igm, position, target, ret);
        ret.effects.Add(new TeleportEffect(agent, position, target));
    }
}
