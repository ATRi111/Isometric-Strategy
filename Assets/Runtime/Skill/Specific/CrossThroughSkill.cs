using EditorExtend.GridEditor;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "穿行", menuName = "技能/特殊/穿行")]
public class CrossThroughSkill : RangedSkill
{
    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        List<Vector2Int> directions = IsometricGridUtility.WithinProjectManhattanDistance(1);
        for (int i = 0; i < directions.Count; i++)
        {
            Vector2Int current = (Vector2Int)position + directions[i];
            GridObject gridObject = igm.GetObjectXY(current);
            if (gridObject == null || gridObject.IsGround)  //必须穿过至少一个单位
                continue;
            for (int j = 1; j < castingDistance; j++)
            {
                current += directions[i];
                gridObject = igm.GetObjectXY(current);
                if (gridObject == null)
                    break;
                if(gridObject.IsGround)
                {
                    ret.Add(igm.AboveGroundPosition(current));
                    break;
                }
            }
        }
    }

    protected override void MockOtherEffectOnAgent(IsometricGridManager igm, PawnEntity agent, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        base.MockOtherEffectOnAgent(igm, agent, position, target, ret);
        ret.effects.Add(new TeleportEffect(agent, position, target));
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        sb.AppendLine("向一个方向穿过一排单位，移动到其后的空地上");
    }
}
