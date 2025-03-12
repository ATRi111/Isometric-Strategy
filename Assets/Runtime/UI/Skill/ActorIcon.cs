using Services;
using Services.Event;
using Services.ObjectPools;
using UnityEngine;

public class ActorIcon : MonoBehaviour
{
    private IsometricGridManager Igm => IsometricGridManager.Instance;
    private IEventSystem eventSystem;

    private Canvas canvas;
    public int extraSortingOrder;

    private void BeforeDoAction(PawnEntity _)
    {
        GetComponent<MyObject>().Recycle();
    }

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.overrideSorting = true;
        eventSystem = ServiceLocator.Get<IEventSystem>();
    }

    private void OnEnable()
    {
        canvas.sortingOrder = Igm.CellToSortingOrder(transform.position) + extraSortingOrder;
        eventSystem.AddListener<PawnEntity>(EEvent.BeforeDoAction, BeforeDoAction);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<PawnEntity>(EEvent.BeforeDoAction, BeforeDoAction);
    }
}
