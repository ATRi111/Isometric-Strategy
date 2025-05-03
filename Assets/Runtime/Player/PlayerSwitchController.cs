using Services;
using Services.Event;
using Services.ObjectPools;
using UnityEngine;

public class PlayerSwitchController : MonoBehaviour
{
    private IEventSystem eventSystem;
    private IObjectManager objectManager;

    private PawnEntity prev;
    private IsometricGridManager Igm => IsometricGridManager.Instance;

    private void SwitchPawn(PawnEntity pawn)
    {
        if (prev == null)
        {
            prev = pawn;
            objectManager.Activate("ActorIcon", Igm.CellToWorld(pawn.GridObject.CellPosition), Vector3.zero, transform);
        }
        else
        {
            if (prev != pawn)
            {
                Vector3Int prevPosition = prev.GridObject.CellPosition;
                Vector3Int pawnPosition = pawn.GridObject.CellPosition;
                pawn.GridObject.CellPosition += 1000 * Vector3Int.forward;
                prev.GridObject.CellPosition = pawnPosition;
                pawn.GridObject.CellPosition = prevPosition;
            }
            prev = null;
            ObjectPoolUtility.RecycleMyObjects(gameObject);
        }
    }

    private void BeforeBattle()
    {
        ObjectPoolUtility.RecycleMyObjects(gameObject);
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        objectManager = ServiceLocator.Get<IObjectManager>();
    }

    private void OnEnable()
    {
        eventSystem.AddListener<PawnEntity>(EEvent.SwitchPawn, SwitchPawn);
        eventSystem.AddListener(EEvent.BeforeBattle, BeforeBattle);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<PawnEntity>(EEvent.SwitchPawn, SwitchPawn);
        eventSystem.RemoveListener(EEvent.BeforeBattle, BeforeBattle);
    }
}
