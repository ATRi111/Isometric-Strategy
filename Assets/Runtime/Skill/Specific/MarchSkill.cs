using Services;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "急行军", menuName = "技能/特殊/急行军")]
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
        sb.Append("消耗所有冲锋层数,");
        sb.Append("每层使目标的等待时间减少");
        sb.Append(deltaTimePerLevel);
        sb.AppendLine();
    }
}
