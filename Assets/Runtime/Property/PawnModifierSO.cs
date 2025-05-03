using Character;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
///  角色修改器（装备，属性，状态，种族等）
/// </summary>
public class PawnModifierSO : ScriptableObject, IDescription
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

    /// <summary>
    /// 模拟启用/禁用此对象产生的属性改变
    /// </summary>
    public void MockPropertyChange(bool register, Dictionary<string, float> ret)
    {
        float k = register ? 1f : -1f;
        for (int i = 0; i < propertyModifier.modifiers.Count; i++)
        {
            PropertyModifier modifier = propertyModifier.modifiers[i];
            string propertyName = modifier.so.name;
            if (!ret.ContainsKey(propertyName))
                ret.Add(propertyName, 0);
            ret[propertyName] += k * modifier.value;
        }
    }

    public virtual void ExtractKeyWords(KeyWordList keyWordList)
    {
        for (int i = 0; i < propertyModifier.modifiers.Count; i++)
        {
            FindPawnPropertySO so = (FindPawnPropertySO)propertyModifier.modifiers[i].so;
            keyWordList.Push(so.name, so.description);
        }
        for (int i = 0; i < skillsAttached.Count; i++)
        {
            Skill skill = skillsAttached[i];
            keyWordList.Push(skill.displayName, skill.Description);
        }
    }


    private string description;
    public string Description
    {
        get
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                description = null;
#endif
            if (string.IsNullOrEmpty(description))
            {
                StringBuilder sb = new();
                Describe(sb);
                sb.Append(extraDescription);
                description = sb.ToString();
            }
            return description;
        }
    }

    protected virtual string TypeName => string.Empty;

    protected virtual void Describe(StringBuilder sb)
    {
        DescribePropertyModifier(sb);
        if (skillsAttached.Count > 0)
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
                sb.Append(skillsAttached[i].displayName);
                sb.Append(" ");
            }
        }
        if (skillsAttached[^1] != null)
            sb.Append(skillsAttached[^1].displayName);
        sb.AppendLine();
    }
}
