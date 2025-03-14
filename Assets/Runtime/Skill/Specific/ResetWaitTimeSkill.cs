using Services;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "�ȴ�ʱ������", menuName = "����/����/�ȴ�ʱ������")]
public class ResetWaitTimeSkill : RangedSkill
{
    protected override void MockOtherEffectOnVictim(PawnEntity agent, Entity victim, EffectUnit ret)
    {
        base.MockOtherEffectOnVictim(agent, victim, ret);
        GameManager gameManager = ServiceLocator.Get<GameManager>();
        PawnEntity pawnVictim = victim as PawnEntity;
        TimeEffect effect = new(pawnVictim, pawnVictim.time, gameManager.Time);
        ret.effects.Add(effect);
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        sb.Append("��Ŀ��ĵȴ�ʱ������");
        sb.AppendLine();
    }
}
