using Services;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "���о�", menuName = "����/����/���о�")]
public class MarchSkill : RangedSkill
{
    public int deltaTimePerLevel;

    protected override void MockOtherEffectOnVictim(PawnEntity agent, Entity victim, EffectUnit ret)
    {
        base.MockOtherEffectOnVictim(agent, victim, ret);
        GameManager gameManager = ServiceLocator.Get<GameManager>();
        PawnEntity pawnVictim = victim as PawnEntity;
        int deltaTime = -Mathf.Min(agent.parameterDict[ChargeSkill.ChargeLevel] * deltaTimePerLevel, pawnVictim.time - gameManager.Time);
        TimeEffect effect = new(pawnVictim);
        effect.current += deltaTime;
    }

    public override void ExtractKeyWords(KeyWordList keyWordList)
    {
        base.ExtractKeyWords(keyWordList);
        Parameter p = PawnEntity.ParameterTable[ChargeSkill.ChargeLevel];
        keyWordList.Push(p.name, p.description);
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        sb.Append("�������г�����,");
        sb.Append("ÿ��ʹĿ��ĵȴ�ʱ�����");
        sb.Append(deltaTimePerLevel);
        sb.AppendLine();
    }
}
