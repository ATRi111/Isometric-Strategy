using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ѡ��Ŀ��ļ���
/// </summary>
public class AimlessSkill : Skill
{
    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        base.GetOptions(agent, igm, position, ret);
        ret.Add(position);
    }
}
