using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ŀ���͵ļ���
/// </summary>
[CreateAssetMenu(fileName = "��Ŀ�꼼��", menuName = "����/��Ŀ�꼼��", order = -1)]
public class AimlessSkill : Skill
{
    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        base.GetOptions(agent, igm, position, ret);
        ret.Add(position);
    }
}