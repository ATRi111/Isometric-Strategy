using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "�������ͼ���", menuName = "����/�������ͼ���", order = -1)]
public class ParabolaSkill : ProjectileSkill
{
    public List<float> angles;
    public float maxSpeed;

    public override GridObject HitCheck(IsometricGridManager igm, Vector3 from, Vector3 to, out Vector3 hit)
    {
        hit = to;
        //TODO
        return null;
    }
}
