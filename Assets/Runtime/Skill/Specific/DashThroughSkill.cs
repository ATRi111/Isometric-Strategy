using EditorExtend.GridEditor;
using MyTool;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "�ǳ�����", menuName = "����/����/�ǳ�����")]
public class DashThroughSkill : ChargeAttackSkill
{
    public float speedMultiplier = 2f;

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
                if (gridObject.IsGround)
                {
                    ret.Add(igm.AboveGroundPosition(current));
                    break;
                }
            }
        }
    }

    public override void MockArea(IsometricGridManager igm, Vector3Int position, Vector3Int target, List<Vector3Int> ret)
    {
        base.MockArea(igm, position, target, ret);
        Vector2Int current = (Vector2Int)position;
        Vector2Int delta = EDirectionTool.NearestDirection4((Vector3)(target - position));
        for (int i = 0; i < castingDistance; i++)
        {
            current += delta;
            Vector3Int p = igm.AboveGroundPosition(current);
            ret.Add(p);
            if (p == target)
                break;
        }
    }

    protected override void MockOtherEffectOnAgent(IsometricGridManager igm, PawnEntity agent, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        base.MockOtherEffectOnAgent(igm, agent, position, target, ret);
        List<Vector3> route = new() { position, target };
        ret.effects.Add(new MoveEffect(agent, position, target, route, speedMultiplier));
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        sb.AppendLine("��һ�����򴩹�һ�ŵ�λ,�ƶ������Ŀյ���,");
        sb.AppendLine("�Դ��������е�λ����˺�");
    }
}
