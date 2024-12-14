using Services.Event;
using UIExtend;
using UnityEngine;

[RequireComponent(typeof(CanvasGroupPlus))]
public class MessageUI : TextBase
{
    private RectTransform rectTransform;
    private CanvasGroupPlus canvasGrounp;
    private object source;  //当前引发消息的对象

    private void ShowMessage(object source, Vector2 screenPoint, string message)
    {
        this.source = source;
        TextUI.text = message;
        transform.position = screenPoint;
        UIExtendUtility.ClampInScreen(rectTransform);
        canvasGrounp.Visible = true;
    }

    private void HideMessage(object source)
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
        eventSystem.AddListener<object, Vector2, string>(EEvent.ShowMessage, ShowMessage);
        eventSystem.AddListener<object>(EEvent.HideMessage, HideMessage);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<object, Vector2, string>(EEvent.ShowMessage, ShowMessage);
        eventSystem.RemoveListener<object>(EEvent.HideMessage, HideMessage);
    }
}
