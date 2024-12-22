using MyTool;

public class SkillIcon : IconUI
{
    public Skill skill;

    public void SetSkill(Skill skill)
    {
        this.skill = skill;
        message = skill.displayName.Bold() + "\n" + skill.Description;
    }
}
