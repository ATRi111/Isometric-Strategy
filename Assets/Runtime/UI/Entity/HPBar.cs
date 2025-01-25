using UnityEngine;
using UnityEngine.UI;

public class HPBar : HPUI
{
    [SerializeField]
    private Image image;

    protected override void AfterHPChange(int prev, int current)
    {
        base.AfterHPChange(prev, current);
        image.fillAmount = current / defenceComponent.maxHP.CurrentValue;
        if (defenceComponent.HP == defenceComponent.maxHP.IntValue && entity is not PawnEntity)
            canvasGroup.Visible = false;
        else
            canvasGroup.Visible = true;
    }

    protected override void Awake()
    {
        base.Awake();
        entity = GetComponentInParent<Entity>();
        entity.DefenceComponent.AfterHPChange += AfterHPChange;
    }
}
