using UIExtend;
using UnityEngine;

[RequireComponent(typeof(CanvasGroupPlus))]
public class HPUI : MonoBehaviour
{
    public Entity entity;

    protected CanvasGroupPlus canvasGroup;
    protected BattleComponent battleComponent;

    public virtual void SetEntity(Entity entity)
    {
        this.entity = entity;
        battleComponent = entity.BattleComponent;
        battleComponent.AfterHPChange += AfterHPChange;
    }

    protected virtual void AfterHPChange(int prev, int current)
    {

    }

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroupPlus>();
    }

    protected virtual void OnDisable()
    {
        if (battleComponent != null)
            battleComponent.AfterHPChange -= AfterHPChange;
    }

    protected virtual void Update()
    {
        
    }
}
