using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "状态复制", menuName = "技能/特殊/状态复制")]
public class CopyBuffSkill : RangedSkill
{
    protected override void MockBuffOnVictim(PawnEntity agent, PawnEntity victim, EffectUnit ret)
    {
        base.MockBuffOnVictim(agent, victim, ret);
        List<Buff> buffs = new();
        victim.BuffManager.GetAllEnabled(buffs);
        for (int i = 0; i < buffs.Count; i++)
        {
            Buff buff = buffs[i];
            BuffEffect buffEffect = agent.BuffManager.MockAdd(buff.so, buff.startTime, Effect.MaxProbability);
            ret.effects.Add(buffEffect);
        }
    }
}
