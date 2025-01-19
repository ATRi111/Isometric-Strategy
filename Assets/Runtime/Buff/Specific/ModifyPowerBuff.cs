using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "附加伤害的状态", menuName = "状态/附加伤害的状态")]
public class ModifyPowerBuff : BuffSO
{
    public SkillPower skillPower;

    public bool CanModifyPower(AimSkill skill)
    {
        if(skill.powers.Count == 0)
            return false;
        for(int i = 0; i < skill.powers.Count; i++)
        {
            if (skill.powers[i].power > 0)  //默认情况下，一切威力为正数的技能均可附加伤害
                return true;
        }
        return false;
    }

    public void ModifyPower(List<SkillPower> powers)
    {
        powers.Add(skillPower);
    }

    public override void Register(PawnEntity pawn)
    {
        base.Register(pawn);
        pawn.OffenceComponent.ModifyPower += ModifyPower;
    }

    public override void Unregister(PawnEntity pawn)
    {
        base.Unregister(pawn);
        pawn.OffenceComponent.ModifyPower -= ModifyPower;
    }
}
