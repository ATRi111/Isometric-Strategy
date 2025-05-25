using Services;
using Services.Event;
using System.Collections.Generic;
using UIExtend;
using UnityEngine;

[RequireComponent(typeof(CanvasGroupPlus))]
public class GuidancePanel : MonoBehaviour
{
    private IEventSystem eventSystem;
    private readonly Dictionary<EGuidance, GuidancePage> pages = new();
    public readonly HashSet<EGuidance> checkedGuidance = new();
    private CanvasGroupPlus canvasGroup;
    [HideInInspector]
    public EGuidance current;

    public void CheckGuidance(EGuidance guidance)
    {
        if (canvasGroup.Visible == false && pages.ContainsKey(guidance) && !checkedGuidance.Contains(guidance))
        {
            canvasGroup.Visible = true;
            HideAllPage();
            pages[guidance].canvasGroup.Visible = true;
            current = guidance;
            checkedGuidance.Add(guidance);
        }
    }

    public void SwitchToGuidance(EGuidance guidance)
    {
        canvasGroup.Visible = true;
        if (pages.ContainsKey(guidance))
        {
            HideAllPage();
            pages[guidance].canvasGroup.Visible = true;
            current = guidance;
        }
    }

    public void HideAllPage()
    {
        foreach (GuidancePage page in pages.Values)
        {
            page.canvasGroup.Visible = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            if (canvasGroup.Visible)
            {
                HideAllPage();
                canvasGroup.Visible = false;
            }
            else
            {
                pages[current].canvasGroup.Visible = true;
                canvasGroup.Visible = true;
            }
        }
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        canvasGroup = GetComponent<CanvasGroupPlus>();
        current = EGuidance.Basic;
        GuidancePage[] temp = GetComponentsInChildren<GuidancePage>();
        for (int i = 0; i < temp.Length; i++)
        {
            pages.Add(temp[i].guidance, temp[i]);
        }
    }

    private void OnEnable()
    {
        eventSystem.AddListener<EGuidance>(EEvent.CheckGuaidance, CheckGuidance);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<EGuidance>(EEvent.CheckGuaidance, CheckGuidance);
    }
}
