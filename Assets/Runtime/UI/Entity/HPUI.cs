using UnityEngine;

public class HPUI : MonoBehaviour
{
    protected Entity entity;
    protected BattleComponent battleComponent;

    public virtual void Initialize(Entity entity)
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
