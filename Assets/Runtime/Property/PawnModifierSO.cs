using MyTool;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
///  角色修改器（装备，属性，状态，种族等）
/// </summary>
public class PawnModifierSO : ScriptableObject
{
    public Sprite icon;
    public PawnPropertyModifier propertyModifier;
    public List<Skill> skillsAttached;
    public string extraDescription;

    public virtual void Register(PawnEntity pawn)
    {
        propertyModifier.Register(pawn);
        for (int i = 0; i < skillsAttached.Count; i++)
        {
            pawn.SkillManager.Learn(skillsAttached[i]);
        }
    }

    public virtual void Unregister(PawnEntity pawn)
    {
        propertyModifier.Unregister(pawn);
        for (int i = 0; i < skillsAttached.Count; i++)
        {
            pawn.SkillManager.Forget(skillsAttached[i]);
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
        sb.Append("技能：");
        for (int i = 0; i < skillsAttached.Count - 1; i++)
        {
            if (skillsAttached[i] != null)
            {
                sb.Append(skillsAttached[i].displayName.Bold());
                sb.Append(" ");
            }
        }
        if (skillsAttached[^1] != null)
            sb.Append(skillsAttached[^1].displayName.Bold());
        sb.AppendLine();
    }
}
