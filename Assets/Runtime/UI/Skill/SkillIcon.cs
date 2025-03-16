using MyTool;
using UnityEngine;

public class SkillIcon : InfoIcon
{
    public Skill skill;
    public bool canUse;
    [SerializeField]
    private Color enabledColor;
    [SerializeField]
    private Color disabledColor;

    protected override void ExtractKeyWords()
    {
        base.ExtractKeyWords();
        skill.ExtractKeyWords(keyWordList);
    }

    public void SetSkill(Skill skill, bool canUse)
    {
        this.skill = skill;
        this.canUse = canUse;
        image.sprite = skill.icon;
        info = skill.displayName.Bold() + "\n" + skill.Description;
        image.color = canUse ? enabledColor : disabledColor;
    }
}
