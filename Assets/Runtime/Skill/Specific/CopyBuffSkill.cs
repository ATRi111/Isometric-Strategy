using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "״̬����", menuName = "����/����/״̬����")]
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
