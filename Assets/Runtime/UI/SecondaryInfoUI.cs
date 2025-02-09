using Services.Event;
using UIExtend;
using UnityEngine;

[RequireComponent(typeof(CanvasGroupPlus))]
public class SecondaryInfoUI : TextBase
{
    private RectTransform rectTransform;
    private CanvasGroupPlus canvasGrounp;

    private void ShowInfo(Vector2 screenPoint, string info)
    {
        TextUI.text = info;
        transform.position = screenPoint;
        canvasGrounp.Visible = true;
    }

    private void HideInfo()
    {
        canvasGrounp.Visible = false;
    }

    protected override void Awake()
    {
        base.Awake();
        rectTransform = GetComponent<RectTransform>();
        canvasGrounp = GetComponent<CanvasGroupPlus>();
    }

    private void OnEnable()
    {
        eventSystem.AddListener<Vector2, string>(EEvent.ShowSecondaryInfo, ShowInfo);
        eventSystem.AddListener(EEvent.HideSecondaryInfo, HideInfo);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<Vector2, string>(EEvent.ShowSecondaryInfo, ShowInfo);
        eventSystem.RemoveListener(EEvent.HideSecondaryInfo, HideInfo);
    }

    private void Update()
    {
        if (canvasGrounp.Visible)
            UIExtendUtility.ClampInScreen(rectTransform);
    }
}
