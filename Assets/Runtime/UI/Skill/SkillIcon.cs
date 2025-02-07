using MyTool;

public class SkillIcon : IconUI
{
    public Skill skill;

    public void SetSkill(Skill skill)
    {
        this.skill = skill;
        image.sprite = skill.icon;
        info = skill.displayName.Bold() + "\n" + skill.Description;
    }
}
