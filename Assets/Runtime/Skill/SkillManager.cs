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
    /// �����Ͳ��Ҽ���
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
    /// �����ƺ����Ͳ��Ҽ��ܣ����ȿ��Ƿ���չʾ���ư���name�ļ��ܣ��ٿ��Ƿ����ʲ����ư���name�ļ��ܣ�
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
