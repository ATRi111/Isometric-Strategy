using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "×´Ì¬Õ³Ìù", menuName = "¼¼ÄÜ/ÌØÊâ/×´Ì¬Õ³Ìù")]
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
            BuffEffect buffEffect = victim.BuffManager.MockAdd(buff.so, buff.startTime, Effect.MaxProbability);
            ret.effects.Add(buffEffect);
        }
    }
}
