using Services;
using Services.Event;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectPlayerImage : MonoBehaviour, IPointerClickHandler
{
    private IEventSystem eventSystem;
    private PlayerManager playerManager;
    private PlayerIcon playerIcon;
    private Image image;

    public bool Selected
    {
        get => image.enabled;
        set
        {
            image.enabled = value;
            if (value)
                playerManager.Select(playerIcon.index);
            else
                playerManager.Unselect(playerIcon.index);
        }
    }

    public bool Selectable => !playerManager.FullySelected || Selected;

    public void AfterBattle(bool _)
    {
        Selected = false;
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        image = transform.GetChild(0).GetComponent<Image>();
        playerIcon = GetComponentInParent<PlayerIcon>();
        playerManager = PlayerManager.FindInstance();
    }

    private void OnEnable()
    {
        eventSystem.AddListener<bool>(EEvent.AfterBattle, AfterBattle);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<bool>(EEvent.AfterBattle, AfterBattle);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Selectable)
            Selected = !Selected;
    }
}
