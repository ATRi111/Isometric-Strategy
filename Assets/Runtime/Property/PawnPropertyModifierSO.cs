using System.Collections.Generic;
using UnityEngine;

public class PawnPropertyModifierSO : ScriptableObject
{
    public PawnPropertyModifier propertyModifier;
    public List<Skill> skillsAttached;

    public virtual void Register(PawnEntity pawn)
    {
        propertyModifier.Register(pawn);
        for (int i = 0; i < skillsAttached.Count; i++)
        {
            pawn.Brain.Learn(skillsAttached[i]);
        }
    }

    public virtual void Unregister(PawnEntity pawn)
    {
        propertyModifier.Unregister(pawn);
        for (int i = 0; i < skillsAttached.Count; i++)
        {
            pawn.Brain.Forget(skillsAttached[i]);
        }
    }
}
