using Character;
using UnityEngine;

public class PawnEntity : CharacterEntity
{
    [AutoComponent]
    public GridPawn GridPawn { get; private set; }
    [AutoComponent]
    public PawnBrain Brain { get; private set; }
    [AutoComponent]
    public MoveController MoveController { get; private set; }

    [SerializeField]
    private PawnProperty property;
    public PawnProperty Property => property;
    [SerializeField]
    private PawnSetting setting;
    public PawnSetting Setting => setting;
}
