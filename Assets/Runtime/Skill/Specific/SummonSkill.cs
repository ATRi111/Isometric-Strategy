using EditorExtend.GridEditor;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "�ٻ�", menuName = "����/����/�ٻ�", order = -1)]
public class SummonSkill : RangedSkill
{
    public GameObject prefab;

    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        base.GetOptions(agent, igm, position, ret);
        List<Vector2Int> primitive = IsometricGridUtility.WithinProjectManhattanDistance(castingDistance);
        for (int i = 0; i < primitive.Count; i++)
        {
            Vector2Int temp = (Vector2Int)position + primitive[i];
            if (!igm.Contains(temp))            //�ж��Ƿ��ڵ�ͼ��
                continue;
            Vector3Int target = igm.AboveGroundPosition(temp);
            if (!LayerCheck(position, target))  //�ж��ͷ�λ�ø߶Ȳ��Ƿ����
                continue;
            GridObject gridObject = igm.GetObjectXY(temp);
            if (!gridObject.TryGetComponent(out GridSurface _))
                continue;                       //�ж��ͷ�λ���Ƿ�Ϊ�յ�
            ret.Add(target);
        }
    }

    public override void Mock(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        base.Mock(agent, igm, position, target, ret);
        SummonEffect effect = new(agent, prefab, target);
        ret.effects.Add(effect);
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        sb.Append("��ָ��λ���ٻ�һ��");
        sb.Append(prefab.name);
        sb.AppendLine();
    }
}
