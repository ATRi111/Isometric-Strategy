using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "�Ƴ�Buff", menuName = "����/����/�Ƴ�Buff")]
public class RemoveBuffSkill : RangedSkill
{
    public EBuffType buffType;

    protected override void MockBuffOnVictim(PawnEntity agent, PawnEntity victim, EffectUnit ret)
    {
        List<Buff> buffs = victim.BuffManager.FindRemovable(buffType);
        if (buffs.Count > 0)
        {
            int r = Effect.randomGroup.RandomInt(0, buffs.Count);
            RemoveBuffEffect effect = new(victim, buffs[r], victim.BuffManager);
            ret.effects.Add(effect);
        }
        base.MockBuffOnVictim(agent, victim, ret);
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        sb.Append("�Ƴ�Ŀ���һ�����");
        switch (buffType)
        {
            case EBuffType.Uncertain:
                sb.Append("?");
                break;
            case EBuffType.Buff:
                sb.Append("����");
                break;
            case EBuffType.Debuff:
                sb.Append("����");
                break;
        }
        sb.Append("״̬");
        sb.AppendLine();
    }
}
