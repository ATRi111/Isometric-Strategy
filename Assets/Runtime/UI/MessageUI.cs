using Services;
using UnityEngine;

[RequireComponent(typeof(CanvasGrounpPlus))]
public class MessageUI : TextBase
{
    private RectTransform rectTransform;
    private CanvasGrounpPlus canvasGrounp;
    private object source;  //当前引发消息的对象

    protected override void Awake()
    {
        base.Awake();
        rectTransform = GetComponent<RectTransform>();
        canvasGrounp = GetComponent<CanvasGrounpPlus>();
    }

    private void ShowMessage(object source, Vector2 screenPoint, string message)
    {
        this.source = source;
        TextUI.text = message;
        transform.position = screenPoint;
        canvasGrounp.Visible = true;
    }

    private void HideMessage(object source)
    {
        if (source != this.source)
            return;
        canvasGrounp.Visible = false;
        this.source = null;
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
