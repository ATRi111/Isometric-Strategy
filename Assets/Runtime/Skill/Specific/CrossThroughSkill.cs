using EditorExtend.GridEditor;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "����", menuName = "����/����/����")]
public class CrossThroughSkill : RangedSkill
{
    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        List<Vector2Int> directions = IsometricGridUtility.WithinProjectManhattanDistance(1);
        for (int i = 0; i < directions.Count; i++)
        {
            Vector2Int current = (Vector2Int)position + directions[i];
            GridObject gridObject = igm.GetObjectXY(current);
            if (gridObject == null || gridObject.IsGround)  //���봩������һ����λ
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
        sb.AppendLine("��һ�����򴩹�һ�ŵ�λ���ƶ������Ŀյ���");
    }
}
