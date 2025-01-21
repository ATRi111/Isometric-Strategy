using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "״̬ճ��", menuName = "����/����/״̬ճ��")]
public class PasteBuffSkill : RangedSkill
{
    protected override void MockBuffOnVictim(PawnEntity agent, PawnEntity victim, EffectUnit ret)
    {
        base.MockBuffOnVictim(agent, victim, ret);
        List<Buff> buffs = new();
        agent.BuffManager.GetAllEnabled(buffs);
        for (int i = 0; i < buffs.Count; i++)
        {
            Buff buff = buffs[i];
            BuffEffect buffEffect = agent.BuffManager.MockAdd(buff.so, victim, buff.startTime, Effect.MaxProbability);
            ret.effects.Add(buffEffect);
        }
    }
}
