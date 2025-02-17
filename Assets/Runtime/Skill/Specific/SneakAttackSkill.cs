using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "偷袭", menuName = "技能/特殊/偷袭")]
public class SneakAttackSkill : RangedSkill
{
    public static bool SneakCheck(PawnEntity agent, Entity victim)
    {
        if (victim is PawnEntity pawnVictim)
        {
            Vector2Int direction = (Vector2Int)(victim.GridObject.CellPosition - agent.GridObject.CellPosition);
            return direction == pawnVictim.faceDirection;
        }
        return false;
    }

    public float powerAmplifier;

    protected override void MockDamageOnVictim(IsometricGridManager igm, PawnEntity agent, Entity victim, Vector3Int position, Vector3Int target, List<SkillPower> powers, EffectUnit ret)
    {
        int damage = 0;
        DefenceComponent def = victim.DefenceComponent;
        for (int j = 0; j < powers.Count; j++)
        {
            float attackPower = agent.OffenceComponent.MockAttackPower(powers[j]);
            damage += def.MockDamage(powers[j].type, attackPower);
        }
        if (SneakCheck(agent, victim))
        {
            damage = Mathf.RoundToInt(damage * (1f + powerAmplifier));
        }

        int HP = Mathf.Clamp(def.HP - damage, 0, def.maxHP.IntValue);
        Effect effect = new HPChangeEffect(victim, def.HP, HP);
        ret.effects.Add(effect);
        if (HP == 0)
        {
            effect = new DisableEntityEffect(victim);
            ret.effects.Add(effect);
        }
        else if (victim is PawnEntity pawnVictim)
        {
            HitBackUtility.MockHitBack(igm, position, pawnVictim, HP, hitBackProbability, ret);
        }
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        sb.Append("从背后攻击敌人时,伤害提高");
        sb.Append(powerAmplifier.ToString("P0"));
    }
}
