using EditorExtend.GridEditor;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum EVictimType
{
    Everything = 0,
    Entity = 1,
    Pawn = 2,
}

/// <summary>
/// ��һ����Χ���ͷŵļ���
/// </summary>
[CreateAssetMenu(fileName = "�����ͼ���", menuName = "����/�����ͼ���", order = -1)]
public class RangedSkill : AimSkill
{
    public int castingDistance = 1;
    public EVictimType victimType = EVictimType.Entity;

    /// <summary>
    /// ��ȡ��ѡʩ��λ��
    /// </summary>
    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        base.GetOptions(agent, igm, position, ret);
        List<Vector2Int> primitive = IsometricGridUtility.WithinProjectManhattanDistance(castingDistance);
        List<Entity> victims = new();
        for (int i = 0; i < primitive.Count; i++)
        {
            Vector2Int temp = (Vector2Int)position + primitive[i];
            Vector3Int target = temp.AddZ(igm.AboveGroundLayer(temp));
            if (!LayerCheck(position, target))
                continue;
            switch(victimType)
            {
                case EVictimType.Everything:
                    if(igm.ObjectDict.ContainsKey(target + Vector3Int.back))
                        ret.Add(target);
                    break;
                case EVictimType.Entity:
                case EVictimType.Pawn:
                    GetVictims(agent, igm, position, target, victims);
                    if (victims.Count > 0)
                        ret.Add(target);
                    break;
            }
        }
    }

    /// <summary>
    /// �ж�ĳ��Entity�Ƿ�����Ϊ���ܵ�Ŀ��
    /// </summary>
    public override bool FilterVictim(Entity entity)
    {
        return victimType  switch
        {
            EVictimType.Everything or EVictimType.Entity => true,
            EVictimType.Pawn => entity is PawnEntity,
            _ => false,
        };
}

    /// <summary>
    /// ��ȡ���ܽ������е�Ŀ��
    /// </summary>
    public override void GetVictims(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, List<Entity> ret)
    {
        base.GetVictims(agent, igm, position, target, ret);
        if (igm.EntityDict.ContainsKey(target) && FilterVictim(igm.EntityDict[target]))
            ret.Add(igm.EntityDict[target]);
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        DescribeCastingDistance(sb);
    }

    protected virtual void DescribeCastingDistance(StringBuilder sb)
    {
        sb.Append("ʩ�ŷ�Χ:");
        sb.Append(castingDistance);
        sb.AppendLine();
    }
}
