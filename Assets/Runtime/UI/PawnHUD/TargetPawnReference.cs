using Services.Event;

public class TargetPawnReference : PawnReference
{
    private void BeforeDoAction(PawnEntity _)
    {
        SetPawn(null);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        eventSystem.AddListener<PawnEntity>(EEvent.SetPawnTaregt, SetPawn);
        eventSystem.AddListener<PawnEntity>(EEvent.BeforeDoAction, BeforeDoAction);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        eventSystem.RemoveListener<PawnEntity>(EEvent.SetPawnTaregt, SetPawn);
        eventSystem.RemoveListener<PawnEntity>(EEvent.BeforeDoAction, BeforeDoAction);
    }
}
