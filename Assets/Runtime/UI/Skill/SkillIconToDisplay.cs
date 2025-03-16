using UnityEngine;

public class SkillIconToDisplay : MonoBehaviour
{
    private PawnPanel pawnPanel;
    private SkillIcon skillIcon;
    public int index;

    private void Refresh()
    {
        PawnEntity pawn = pawnPanel.SelectedPawn;
        if (index < pawn.SkillManager.learnedSkills.Count)
        {
            skillIcon.canvasGroup.Visible = true;
            skillIcon.SetSkill(pawn.SkillManager.learnedSkills[index], true);
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
