using Services.Event;
using UIExtend;
using UnityEngine;

[RequireComponent(typeof(CanvasGroupPlus))]
public class InfoUI : TextBase
{
    private RectTransform rectTransform;
    private CanvasGroupPlus canvasGrounp;
    private object source;  //当前引发消息的对象

    private void ShowInfo(object source, Vector2 screenPoint, string info)
    {
        this.source = source;
        TextUI.text = info;
        transform.position = screenPoint;
        canvasGrounp.Visible = true;
    }

    private void HideInfo(object source)
    {
        if (source != this.source)
            return;
        canvasGrounp.Visible = false;
        this.source = null;
    }

    protected override void Awake()
    {
        base.Awake();
        rectTransform = GetComponent<RectTransform>();
        canvasGrounp = GetComponent<CanvasGroupPlus>();
    }

    private void OnEnable()
    {
        eventSystem.AddListener<object, Vector2, string>(EEvent.ShowInfo, ShowInfo);
        eventSystem.AddListener<object>(EEvent.HideInfo, HideInfo);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<object, Vector2, string>(EEvent.ShowInfo, ShowInfo);
        eventSystem.RemoveListener<object>(EEvent.HideInfo, HideInfo);
    }

    private void Update()
    {
        if(canvasGrounp.Visible)
            UIExtendUtility.ClampInScreen(rectTransform);
    }
}
