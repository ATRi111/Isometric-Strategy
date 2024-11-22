using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "��װ��", menuName = "װ��")]
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
