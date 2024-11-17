using UnityEngine;
using UnityEngine.EventSystems;

public class SkillIconToSelect : MonoBehaviour,IPointerClickHandler
{
    private SkillUIManager skillUIManager;
    private SkillIcon icon;

    public void OnPointerClick(PointerEventData eventData)
    {
        skillUIManager.AfterSelectSkill?.Invoke(icon.skill);
    }

    private void Awake()
    {
        icon = GetComponent<SkillIcon>();
        skillUIManager = SkillUIManager.FindInstance();
    }
}
