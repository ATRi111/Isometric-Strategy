using MyTool;

public class SkillIcon : InfoIcon
{
    public Skill skill;

    protected override void ExtractKeyWords()
    {
        base.ExtractKeyWords();
        skill.ExtractKeyWords(keyWordList);
    }

    public void SetSkill(Skill skill)
    {
        this.skill = skill;
        image.sprite = skill.icon;
        info = skill.displayName.Bold() + "\n" + skill.Description;
    }
}
