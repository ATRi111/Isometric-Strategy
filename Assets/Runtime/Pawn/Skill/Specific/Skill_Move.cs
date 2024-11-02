using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_Move", menuName = "Skill/Move")]
public class Skill_Move : Skill
{
    public override void GetOptions(IsometricGridManager igm, Vector2Int position, List<Vector2Int> ret)
    {
        //TODO:ͨ��Ѱ·��ȡ�ɴ�λ��
    }

    public override void Mock(PawnEntity agent, Vector2Int position, Vector2Int target, IsometricGridManager igm, EffectUnit ret)
    {
        base.Mock(agent, position, target, igm, ret);
        //TODO:ģ��·��
    }

    public override int MockTime(PawnEntity agent, Vector2Int position, Vector2Int target, IsometricGridManager igm)
    {
        return DistanceBetween(position, target) * actionTime;
    }
}
