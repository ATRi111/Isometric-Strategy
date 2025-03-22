using MyTool;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillIcon : InfoIcon
{
    public Skill skill;
    public bool canUse;
    private SkillUIManager skillUIManager;

    [SerializeField]
    private Color enabledColor;
    [SerializeField]
    private Color disabledColor;

    protected override void ExtractKeyWords()
    {
        base.ExtractKeyWords();
        skill.ExtractKeyWords(KeyWordList);
    }

    public void SetSkill(Skill skill, bool canUse)
    {
        this.skill = skill;
        this.canUse = canUse;
        image.sprite = skill.icon;
        info = skill.displayName.Bold() + "\n" + skill.Description;
        image.color = canUse ? enabledColor : disabledColor;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        skillUIManager.PreviewSkillOption?.Invoke(skill);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        skillUIManager.StopPreviewSkillOption?.Invoke();
    }

    protected override void Awake()
    {
        base.Awake();
        skillUIManager = SkillUIManager.FindInstance();
    }
}
