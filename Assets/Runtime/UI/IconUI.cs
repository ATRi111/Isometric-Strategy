using Services;
using Services.Event;
using UIExtend;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroupPlus))]
public class IconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected IEventSystem eventSystem;
    protected GameManager gameManager;
    [SerializeField]
    protected string message;
    [HideInInspector]
    public CanvasGroupPlus canvasGroup;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(message))
            eventSystem.Invoke(EEvent.ShowMessage, (object)this, eventData.position, message);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        eventSystem.Invoke(EEvent.HideMessage, (object)this);
    }

    protected virtual void Awake()
    {
        gameManager = ServiceLocator.Get<GameManager>();
        eventSystem = ServiceLocator.Get<IEventSystem>();
        canvasGroup = GetComponent<CanvasGroupPlus>();
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {
        eventSystem.Invoke(EEvent.HideMessage, (object)this);
    }
}
