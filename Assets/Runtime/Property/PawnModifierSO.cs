using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PawnModifierSO : ScriptableObject
{
    public PawnPropertyModifier propertyModifier;
    public List<Skill> skillsAttached;
    public string extraDescription;

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

    public string Description
    {
        get
        {
            StringBuilder sb = new();
            Describe(sb);
            sb.Append(extraDescription);
            return sb.ToString();
        }
    }

    protected virtual string TypeName => string.Empty; 

    protected virtual void Describe(StringBuilder sb)
    {
        DescribePropertyModifier(sb);
        if(skillsAttached.Count > 0)
            DescribeSkills(sb);
    }

    protected virtual void DescribePropertyModifier(StringBuilder sb)
    {
        propertyModifier.Describe(sb);
    }

    protected virtual void DescribeSkills(StringBuilder sb)
    {
        sb.Append(TypeName);
        sb.Append("¼¼ÄÜ£º");
        for (int i = 0; i < skillsAttached.Count - 1; i++)
        {
            sb.Append(skillsAttached[i].displayName);
            sb.Append(" ");
        }
        sb.Append(skillsAttached[^1].displayName);
        sb.AppendLine();
    }
}
