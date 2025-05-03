using UIExtend;
using UnityEngine;

[RequireComponent(typeof(CanvasGroupPlus))]
public class HPUI : MonoBehaviour
{
    [HideInInspector]
    public Entity entity;

    protected CanvasGroupPlus canvasGroup;
    protected DefenceComponent defenceComponent;

    public virtual void SetEntity(Entity entity)
    {
        if (defenceComponent != null)
            defenceComponent.AfterHPChange -= AfterHPChange;
        this.entity = entity;
        defenceComponent = entity.DefenceComponent;
        defenceComponent.AfterHPChange += AfterHPChange;
    }

    protected virtual void AfterHPChange(int prev, int current)
    {

    }

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroupPlus>();
    }

    protected virtual void Update()
    {

    }
}
