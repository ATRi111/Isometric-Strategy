using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "����", menuName = "����/����/����")]
public class TeleportSkill : RangedSkill
{
    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        base.GetOptions(agent, igm, position, ret);
        List<Vector2Int> primitive = IsometricGridUtility.WithinProjectManhattanDistance(castingDistance);
        for (int i = 0; i < primitive.Count; i++)
        {
            Vector2Int temp = (Vector2Int)position + primitive[i];
            if (!igm.Contains(temp))    //�ж��Ƿ��ڵ�ͼ��
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
