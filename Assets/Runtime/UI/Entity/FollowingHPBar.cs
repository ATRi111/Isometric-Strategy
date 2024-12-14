using Services.ObjectPools;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ¸úËæ½ÇÉ«µÄHPUI
/// </summary>
public class FollowingHPBar : HPUI
{
    private MyObject myObject;
    [SerializeField]
    private Image image;

    public override void Initialize(Entity entity)
    {
        base.Initialize(entity);
        entity.BeforeDisable += BeforeEntityDisable;
        if (battleComponent.HP == battleComponent.maxHP.IntValue && entity is not PawnEntity)
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
        image.fillAmount = current / battleComponent.maxHP.CurrentValue;
        if (battleComponent.HP == battleComponent.maxHP.IntValue && entity is not PawnEntity)
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
