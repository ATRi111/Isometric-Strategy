using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �е����ļ���
/// </summary>
public abstract class ProjectileSkill : Skill
{
    protected readonly Dictionary<Vector3Int, Vector3Int> cache_HitPoint = new();

    public abstract Vector3Int CalculateHitPoint(Vector3Int target);
}
