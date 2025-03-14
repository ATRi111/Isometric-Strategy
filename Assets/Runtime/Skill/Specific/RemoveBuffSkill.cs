using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "�Ƴ�Buff", menuName = "����/����/�Ƴ�Buff")]
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
        sb.Append("�Ƴ�Ŀ���һ�����");
        switch(removeFlag)
        {
            case ERemoveFlag.Buff:
                sb.Append("����");
                break;
            case ERemoveFlag.Debuff:
                sb.Append("����");
                break;
        }
        sb.Append("״̬");
        sb.AppendLine();
    }
}
