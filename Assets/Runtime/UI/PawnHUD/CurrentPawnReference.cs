using Services.Event;

public class CurrentPawnReference : PawnReference, IPawnReference
{
    protected override void OnEnable()
    {
        eventSystem.AddListener<PawnEntity>(EEvent.BeforeDoAction, SetPawn);
    }

    protected override void OnDisable()
    {
        eventSystem.RemoveListener<PawnEntity>(EEvent.BeforeDoAction, SetPawn);
    }
}
