using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "新装备", menuName = "装备")]
public class Equipment : PawnPropertyModifierSO
{
    public EEquipmentSlot slot;
    public List<Skill> skillsAttached;

    public override void Register(PawnEntity pawn)
    {
        base.Register(pawn);
        for (int i = 0; i < skillsAttached.Count; i++)
        {
            pawn.Brain.Learn(skillsAttached[i]);
        }
    }

    public override void Unregister(PawnEntity pawn)
    {
        base.Unregister(pawn);
        for (int i = 0; i < skillsAttached.Count; i++)
        {
            pawn.Brain.Forget(skillsAttached[i]);
        }
    }
}
