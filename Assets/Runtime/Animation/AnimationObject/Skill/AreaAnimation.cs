using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ڼ������÷�Χ�ڸ�λ������ͬ������Ķ���
/// </summary>
public class AreaAnimation : AnimationObject
{
    public GameObject prefab;

    public override void Initialize(IAnimationSource source)
    {
        base.Initialize(source);
        if (prefab != null)
        {
            PawnAction action = source as PawnAction;

            for (int i = 0; i < action.area.Count; i++)
            {
                Vector3 world = igm.CellToWorld(action.area[i]);
                objectManager.Activate(prefab.name, world, Vector3.zero, transform);
            }
        }
    }
}
