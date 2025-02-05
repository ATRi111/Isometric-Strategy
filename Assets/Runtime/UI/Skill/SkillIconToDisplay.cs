using UnityEngine;

public class SkillIconToDisplay : MonoBehaviour
{
    private PawnPanel pawnPanel;
    private SkillIcon skillIcon;
    public int index;

    private void Refresh(PawnEntity pawnEntity)
    {
        if(index < pawnEntity.SkillManager.learnedSkills.Count)
        {
            skillIcon.canvasGroup.Visible = true;
            skillIcon.SetSkill(pawnEntity.SkillManager.learnedSkills[index]);
        }
        else
        {
            skillIcon.canvasGroup.Visible = false;
        }
    }

    private void Awake()
    {
        skillIcon = GetComponent<SkillIcon>();
        pawnPanel = GetComponentInParent<PawnPanel>();
        pawnPanel.RefreshAll += Refresh;
    }
}
