using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : HPUI
{
    private PawnPanel pawnPanel;
    public TextMeshProUGUI tmp;
    public Image front;
    public Image back;

    private void Refresh()
    {
        SetEntity(pawnPanel.SelectedPawn);
        Color color;
        if (entity is PawnEntity pawn)
        {
            color = pawn.faction switch
            {
                EFaction.Ally => Color.green,
                EFaction.Enemy => Color.red,
                _ => Color.blue,
            };
        }
        else
            color = Color.blue;
        front.color = color;
        back.color = 0.6f * color;
        front.fillAmount = defenceComponent.HP / defenceComponent.maxHP.CurrentValue;
        tmp.text = $"{defenceComponent.HP}/{defenceComponent.maxHP.IntValue}";
    }

    protected override void AfterHPChange(int prev, int current)
    {
        base.AfterHPChange(prev, current);
        front.fillAmount = current / defenceComponent.maxHP.CurrentValue;
        tmp.text = $"{defenceComponent.HP}/{defenceComponent.maxHP.IntValue}";
    }

    protected override void Awake()
    {
        base.Awake();
        pawnPanel = GetComponentInParent<PawnPanel>();
        pawnPanel.RefreshAll += Refresh;
    }
}
