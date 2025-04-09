using Services;
using Services.Asset;
using Services.Event;
using UIExtend;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroupPlus))]
public class InfoIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private static KeyWordList keyWordList;
    protected static KeyWordList KeyWordList
    {
        get
        {
            if (keyWordList == null)
                keyWordList = ServiceLocator.Get<IAssetLoader>().Load<KeyWordList>("KeyWordList");
            return keyWordList;
        }
    }

    protected IEventSystem eventSystem;
    protected GameManager gameManager;
    [SerializeField]
    protected string info;
    [HideInInspector]
    public CanvasGroupPlus canvasGroup;
    protected Image image;

    public float normalizedDistance;

    protected virtual void ExtractKeyWords()
    {
        KeyWordList.PopExtra();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        ExtractKeyWords();
        if (!string.IsNullOrWhiteSpace(info))
            eventSystem.Invoke(EEvent.ShowInfo, info, normalizedDistance);
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
