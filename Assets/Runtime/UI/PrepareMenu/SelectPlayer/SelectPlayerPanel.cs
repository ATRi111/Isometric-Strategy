using Services;
using Services.Event;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectPlayerPanel : MonoBehaviour, IPointerEnterHandler
{
    private IEventSystem eventSystem;
    private PlayerIcon[] icons;

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        icons = GetComponentsInChildren<PlayerIcon>();
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].index = i;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventSystem.Invoke(EEvent.CheckGuaidance, EGuidance.Camera);
    }
}
