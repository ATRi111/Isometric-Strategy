using Services;
using Services.Event;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGrounpPlus))]
public class TimeAxisIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private IEventSystem eventSystem;
    private readonly StringBuilder sb = new();
    public PawnEntity entity;
    [HideInInspector]
    public CanvasGrounpPlus canvasGrounp;

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        canvasGrounp = GetComponent<CanvasGrounpPlus>();
    }

    public void SetPawn(PawnEntity entity)
    {
        this.entity = entity;
        sb.Clear();
        sb.AppendLine(entity.EntityName);
        sb.Append(" £”‡ ±º‰:");
        sb.Append(entity.actionTime);
        sb.AppendLine();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventSystem.Invoke(EEvent.ShowMessage, this, eventData.position, sb.ToString());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventSystem.Invoke(EEvent.HideMessage, this);
    }
}
