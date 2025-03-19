using Services.Event;

public class TargetPawnReference : PawnReference
{
    protected override void OnEnable()
    {
        base.OnEnable();
        eventSystem.AddListener<PawnEntity>(EEvent.SetPawnTaregt, SetPawn);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        eventSystem.RemoveListener<PawnEntity>(EEvent.SetPawnTaregt, SetPawn);
    }
}
