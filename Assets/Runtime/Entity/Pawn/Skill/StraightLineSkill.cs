using EditorExtend.GridEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ֱ���ͼ���", menuName = "����/ֱ���ͼ���", order = -1)]
public class StraightLineSkill : ProjectileSkill
{
    public override GridObject HitCheck(IsometricGridManager igm, Vector3 from, Vector3 to, out Vector3 hit)
    {
        GridObject gridObject = igm.LineSegmentCast(from, to, out hit);
        return gridObject;
    }
}