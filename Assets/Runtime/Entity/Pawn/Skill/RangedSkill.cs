using EditorExtend.GridEditor;
using System.Collections.Generic;
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
[CreateAssetMenu(fileName = "��Χ�ͼ���", menuName = "����/��Χ�ͼ���")]
public class RangedSkill : Skill
{
    public int castingDistance;
    public EVictimType victimType;

    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        base.GetOptions(agent, igm, position, ret);
        List<Vector2Int> primitive = IsometricGridUtility.WithinProjectManhattanDistance(castingDistance);
        List<Entity> victims = new();
        for (int i = 0; i < primitive.Count; i++)
        {
            Vector2Int temp = (Vector2Int)position + primitive[i];
            Vector3Int target = temp.AddZ(igm.AboveGroundLayer(temp));
            GetVictims(agent, igm, position, target, victims);
            if (victims.Count > 0)
                ret.Add(target);
        }
    }

    public override void GetVictims(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, List<Entity> ret)
    {
        base.GetVictims(agent, igm, position, target, ret);
        //TODO:AOE����
        switch (victimType)
        {
            case EVictimType.Everything:
            case EVictimType.Entity:
                if (igm.EntityDict.ContainsKey(target))
                    ret.Add(igm.EntityDict[target]);
                break;
            case EVictimType.Pawn:
                if (igm.EntityDict.ContainsKey(target) && igm.EntityDict[target] is PawnEntity pawn)
                    ret.Add(pawn);
                break;
        }
    }
}
