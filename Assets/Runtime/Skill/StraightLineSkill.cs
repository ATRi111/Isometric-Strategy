using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ֱ�ߵ�������", menuName = "����/ֱ�ߵ�������", order = -1)]
public class StraightLineSkill : ProjectileSkill
{
    public override GridObject HitCheck(IsometricGridManager igm, Vector3 from, Vector3 to, Vector3Int target, List<Vector3> trajectory)
    {
        GridObject gridObject = igm.LineSegmentCast(from, to, out Vector3 hit);
        if (trajectory != null)
        {
            trajectory.Add(from);
            trajectory.Add(hit);
        }
        return gridObject;
    }
}