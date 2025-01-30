using UnityEngine;
using UnityEngine.UI;

public class FollowHPBar : HPUI
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private Vector3 damageNumberOffset;

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
        image.fillAmount = current / defenceComponent.maxHP.CurrentValue;
        if (defenceComponent.HP == defenceComponent.maxHP.IntValue && entity is not PawnEntity)
            canvasGroup.Visible = false;
        else
            canvasGroup.Visible = true;
    }

    protected override void Awake()
    {
        base.Awake();
        SetEntity(GetComponentInParent<Entity>());
    }
}
