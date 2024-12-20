using Services.ObjectPools;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �����ɫ��HPUI
/// </summary>
public class FollowingHPBar : HPUI
{
    private MyObject myObject;
    [SerializeField]
    private Image image;

    public override void SetEntity(Entity entity)
    {
        base.SetEntity(entity);
        entity.BeforeDisable += BeforeEntityDisable;
        if (defenceComponent.HP == defenceComponent.maxHP.IntValue && entity is not PawnEntity)
            canvasGroup.Visible = false;
        else
            canvasGroup.Visible = true;
    }

    private void BeforeEntityDisable()
    {
        myObject.Recycle();
        entity.BeforeDisable -= BeforeEntityDisable;
    }

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
        myObject = GetComponent<MyObject>();
    }

    protected override void Update()
    {
        base.Update();
        if (entity != null)
            transform.position = entity.transform.position;
    }
}
