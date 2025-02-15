using Services;
using Services.ObjectPools;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 辅助受击特效生成
/// </summary>
public static class HitUtility
{
    public static void GenerateHit(PawnAction action)
    {
        HashSet<PawnEntity> generated = new();
        for (int i = 0;i < action.effectUnit.effects.Count;i++)
        {
            if(action.effectUnit.effects[i] is HPChangeEffect effect)
            {
                if(effect.WillHappen && effect.victim is PawnEntity pawnVictim && !generated.Contains(pawnVictim))
                {
                    IMyObject obj = ServiceLocator.Get<IObjectManager>().Activate("Hit",
                        pawnVictim.transform.position,
                        Vector3.zero,
                        pawnVictim.transform);
                    Hit hit = obj.Transform.GetComponent<Hit>();
                    hit.Initialize(action.agent, effect);
                    generated.Add(pawnVictim);
                }
            }
        }
    }
}
