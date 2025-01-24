using Services.Event;
using UnityEngine.EventSystems;

public class PawnIcon : IconUI , IPointerClickHandler
{
    private PawnEntity pawn;

    public void OnPointerClick(PointerEventData eventData)
    {
        eventSystem.Invoke(EEvent.ShowPawnPanel, pawn);
    }

    protected override void Awake()
    {
        base.Awake();
        pawn = GetComponentInParent<PawnEntity>();
    }

    private void Start()
    {
        message = pawn.gameObject.name; //等待名称改完后
    }
}
