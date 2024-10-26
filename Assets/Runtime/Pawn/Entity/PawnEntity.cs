using Character;
using UnityEngine;

public class PawnEntity : CharacterEntity
{
    [AutoComponent]
    public GridPawn GridPawn { get; private set; }
    [AutoComponent]
    public PawnBrain Brain { get; private set; }
    [AutoComponent]
    public GridMoveController MoveController { get; private set; }

    [SerializeField]
    private PawnProperty defaultProperty;
    [SerializeField]
    private PawnProperty property;
    public PawnProperty Property => property;

    [SerializeField]
    private PawnSetting setting;
    public PawnSetting Setting => setting;

    [SerializeField]
    private PawnState state;
    public PawnState State => state;

    private void OnEnable()
    {
        Refresh();
    }

    private void LateUpdate()
    {
        Refresh();
    }

    public void Refresh()
    {
        property = defaultProperty.Clone() as PawnProperty;
    }
}
