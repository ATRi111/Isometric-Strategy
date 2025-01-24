using Character;
using MyTool;

public class SkillManager : CharacterComponentBase
{
    public SerializedHashSet<Skill> learnedSkills;

    public void Learn(Skill skill)
    {
        learnedSkills.Add(skill);
    }
    /// <summary>
    /// 按类型查找技能
    /// </summary>
    public T FindSkill<T>() where T : Skill
    {
        foreach (Skill skill in learnedSkills)
        {
            if (skill is T)
                return skill as T;
        }
        return null;
    }
    /// <summary>
    /// 按名称和类型查找技能（优先看是否有展示名称包含name的技能，再看是否有资产名称包含name的技能）
    /// </summary>
    public T FindSkill<T>(string name) where T : Skill
    {
        foreach (Skill skill in learnedSkills)
        {
            if (skill is T && skill.displayName == name)
                return skill as T;
        }
        foreach (Skill skill in learnedSkills)
        {
            if (skill is T && skill.name.Contains(name))
                return skill as T;
        }
        return null;
    }
    public bool Forget(Skill skill)
    {
        return learnedSkills.Remove(skill);
    }
}
