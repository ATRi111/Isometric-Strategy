using UnityEngine;
using UnityEngine.UI;

public class FollowHPBar : HPUI
{
    public Image front;
    public Image back;
    public Vector3 damageNumberOffset;

    public int damageNumberCount;

    public Vector3 UseDamageNumberPosition()
    {
        damageNumberCount++;
        return transform.position + damageNumberCount * damageNumberOffset;
    }

    protected override void AfterHPChange(int prev, int current)
    {
        base.AfterHPChange(prev, current);
        damageNumberCount = 0;
        front.fillAmount = current / defenceComponent.maxHP.CurrentValue;
        canvasGroup.Visible = true;
    }

    protected override void Awake()
    {
        base.Awake();
        SetEntity(GetComponentInParent<Entity>());
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
    }
}
