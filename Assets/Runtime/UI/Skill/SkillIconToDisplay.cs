using UnityEngine;

public class SkillIconToDisplay : MonoBehaviour
{
    private IPawnReference pawnReference;
    private SkillIcon skillIcon;
    public int index;

    private void Refresh()
    {
        PawnEntity pawn = pawnReference.CurrentPawn;
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
        pawnReference = GetComponentInParent<IPawnReference>();
        pawnReference.OnRefresh += Refresh;
    }
}
