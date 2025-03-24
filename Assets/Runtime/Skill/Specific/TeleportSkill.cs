using EditorExtend.GridEditor;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "����", menuName = "����/����/����")]
public class TeleportSkill : RangedSkill
{
    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        ret.Clear();
        List<Vector2Int> primitive = IsometricGridUtility.WithinProjectManhattanDistance(castingDistance);
        if (aimAtSelf)
            primitive.Add(Vector2Int.zero);
        for (int i = 0; i < primitive.Count; i++)
        {
            Vector2Int temp = (Vector2Int)position + primitive[i];
            if (!igm.Contains(temp))            //�ж��Ƿ��ڵ�ͼ��
                continue;
            Vector3Int target = igm.AboveGroundPosition(temp);
            if (!LayerCheck(position, target))  //�ж��ͷ�λ�ø߶Ȳ��Ƿ����
                continue;
            GridObject gridObject = igm.GetObjectXY(temp);
            if (!gridObject.IsGround)           //�ж���ص��Ƿ�Ϊ�յ�
                continue;
            ret.Add(target);
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
        sb.AppendLine("ʹ�����ƶ���Ŀ��λ��");
    }
}
