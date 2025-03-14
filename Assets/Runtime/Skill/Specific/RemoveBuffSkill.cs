using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "移除Buff", menuName = "技能/特殊/移除Buff")]
public class RemoveBuffSkill : RangedSkill
{
    public ERemoveFlag removeFlag;

    protected override void MockBuffOnVictim(PawnEntity agent, PawnEntity victim, EffectUnit ret)
    {
        List<Buff> buffs = victim.BuffManager.FindRemovable(removeFlag);
        if(buffs.Count > 0)
        {
            int r = Effect.randomGroup.NextInt(0, buffs.Count);
            RemoveBuffEffect effect = new(victim, buffs[r], victim.BuffManager);
            ret.effects.Add(effect);
        }
        base.MockBuffOnVictim(agent, victim, ret);
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        sb.Append("移除目标的一个随机");
        switch(removeFlag)
        {
            case ERemoveFlag.Buff:
                sb.Append("正面");
                break;
            case ERemoveFlag.Debuff:
                sb.Append("负面");
                break;
        }
        sb.Append("状态");
        sb.AppendLine();
    }
}
