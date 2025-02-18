using Services;
using Services.Asset;
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
    protected KeyWordList keyWordList;

    public Vector2 infoPivot;
    public Vector2 infoOffset;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        ExtractKeyWords();
        if (!string.IsNullOrEmpty(info))
            eventSystem.Invoke(EEvent.ShowInfo, eventData.position + infoOffset, infoPivot, info);
    }

    protected virtual void ExtractKeyWords()
    {
        keyWordList.PopExtra();
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
        keyWordList = ServiceLocator.Get<IAssetLoader>().Load<KeyWordList>(nameof(KeyWordList));
        infoPivot = Vector2.up;
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {
        eventSystem.Invoke(EEvent.HideInfo, (object)this);
    }
}
