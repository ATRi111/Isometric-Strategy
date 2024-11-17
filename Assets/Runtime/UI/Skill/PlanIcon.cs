using Services;
using Services.Event;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlanIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    private Plan plan;
    private IEventSystem eventSystem;
    protected string message;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(message))
            eventSystem.Invoke(EEvent.ShowMessage, (object)this, eventData.position, message);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventSystem.Invoke(EEvent.HideMessage, (object)this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SkillUIManager.FindInstance().AfterSelectPlan?.Invoke(plan);
    }

    public void SetPlan(Plan plan)
    {
        message = plan.ToString();
        this.plan = plan;
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        eventSystem = ServiceLocator.Get<IEventSystem>();
        image.alphaHitTestMinimumThreshold = 0.2f;
    }

    private void OnDisable()
    {
        eventSystem.Invoke(EEvent.HideMessage, (object)this);
    }
}
