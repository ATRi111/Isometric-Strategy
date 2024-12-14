using Services.ObjectPools;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ¸úËæ½ÇÉ«µÄHPUI
/// </summary>
[RequireComponent(typeof(MyObject))]
public class FollowingHPBar : HPUI
{
    private MyObject myObject;
    [SerializeField]
    private Image image;

    public override void Initialize(Entity entity)
    {
        base.Initialize(entity);
        entity.BeforeDisable += BeforeEntityDisable;
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
