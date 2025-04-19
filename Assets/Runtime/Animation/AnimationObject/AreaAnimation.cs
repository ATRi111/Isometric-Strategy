using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ڼ������÷�Χ�ڸ�λ������ͬ������Ķ���
/// </summary>
public class AreaAnimation : AnimationObject
{
    public GameObject prefab;
    private readonly List<Vector3Int> area = new();

    public override void Initialize(IAnimationSource source)
    {
        base.Initialize(source);
        if (prefab != null)
        {
            PawnAction action = source as PawnAction;
            AimSkill skill = action.skill as AimSkill;
            skill.MockArea(igm, action.position, action.target, area);
            for (int i = 0; i < area.Count; i++)
            {
                Vector3 world = igm.CellToWorld(area[i]);
                objectManager.Activate(prefab.name, world, Vector3.zero, transform);
            }
        }
    }
}
