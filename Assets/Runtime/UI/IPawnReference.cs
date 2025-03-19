using System;

public interface IPawnReference
{
    public PawnEntity CurrentPawn { get; }

    public Action OnRefresh { get; set; }
}
