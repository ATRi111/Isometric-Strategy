using Services;
using Services.Event;
using UIExtend;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGrounpPlus))]
public class IconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected IEventSystem eventSystem;
    protected GameManager gameManager;
    protected string message;
    [HideInInspector]
    public CanvasGrounpPlus canvasGrounp;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(message))
            eventSystem.Invoke(EEvent.ShowMessage, (object)this, eventData.position, message);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        eventSystem.Invoke(EEvent.HideMessage, (object)this);
    }

    private void Awake()
    {
        gameManager = ServiceLocator.Get<GameManager>();
        eventSystem = ServiceLocator.Get<IEventSystem>();
        canvasGrounp = GetComponent<CanvasGrounpPlus>();
    }
}
