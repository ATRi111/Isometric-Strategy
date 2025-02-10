using Services;
using Services.Event;
using UIExtend;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroupPlus))]
public class IconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected IEventSystem eventSystem;
    protected GameManager gameManager;
    [SerializeField]
    protected string info;
    [HideInInspector]
    public CanvasGroupPlus canvasGroup;
    protected Image image;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(info))
            eventSystem.Invoke(EEvent.ShowInfo, (object)this, eventData.position, info);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        eventSystem.Invoke(EEvent.HideInfo, (object)this);
    }

    protected virtual void Awake()
    {
        gameManager = ServiceLocator.Get<GameManager>();
        eventSystem = ServiceLocator.Get<IEventSystem>();
        canvasGroup = GetComponent<CanvasGroupPlus>();
        image = GetComponentInChildren<Image>();
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {
        eventSystem.Invoke(EEvent.HideInfo, (object)this);
    }
}
