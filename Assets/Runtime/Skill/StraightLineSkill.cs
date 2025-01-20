using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "直线弹道技能", menuName = "技能/直线弹道技能", order = -1)]
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