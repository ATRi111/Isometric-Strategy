using Services;
using Services.Asset;
using Services.Event;
using TMPro;
using UIExtend;
using UnityEngine;

[RequireComponent(typeof(CanvasGroupPlus))]
public class InfoUI : TextBase
{
    public static string Mark(string text)
    {
        return $"<link=\"{text}\"><u><color=\"blue\">{text}</color></u></link>";
    }

    private RectTransform rectTransform;
    private CanvasGroupPlus canvasGrounp;
    private KeyWordList keyWordList;
    private object source;  //当前引发消息的对象

    private bool focusOnIcon;
    private bool containsMouse;
    [SerializeField]
    private Vector2 offset;

    private void ShowInfo(object source, Vector2 screenPoint, string info)
    {
        if (this.source != null)
            return;
        this.source = source;
        focusOnIcon = true;
        info = keyWordList.MarkAllKeyWords(info, Mark);
        TextUI.text = info;
        transform.position = screenPoint + offset;
        UIExtendUtility.ClampInScreen(rectTransform);
        canvasGrounp.Visible = true;
    }

    private void HideInfo(object source)
    {
        if (source != this.source)
            return;
        focusOnIcon = false;
        this.source = null;
    }

    protected override void Awake()
    {
        base.Awake();
        rectTransform = GetComponent<RectTransform>();
        canvasGrounp = GetComponent<CanvasGroupPlus>();
        keyWordList = ServiceLocator.Get<IAssetLoader>().Load<KeyWordList>(nameof(KeyWordList));
        keyWordList.Initialize();
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
        if(Input.GetMouseButtonDown(0))
        {
            canvasGrounp.Visible = false;
        }

        UIExtendUtility.GetBorder(rectTransform, out float left, out float right, out float bottom, out float top);
        Vector2 mouse = Input.mousePosition;
        containsMouse = mouse.x >= left && mouse.x <= right
            && mouse.y >= bottom && mouse.y <= top;

        if (!containsMouse && !focusOnIcon)
            canvasGrounp.Visible = false;

        if (canvasGrounp.Visible)
        {
            CheckSecondaryInfo();
            UIExtendUtility.ClampInScreen(rectTransform);
        }
    }

    private void CheckSecondaryInfo()
    {
        Vector3 mouse = Input.mousePosition;
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(TextUI, mouse, null);
        if (linkIndex > -1)
        {
            TMP_LinkInfo linkInfo = TextUI.textInfo.linkInfo[linkIndex];
            string description = keyWordList.GetDescription(linkInfo.GetLinkText());
            eventSystem.Invoke(EEvent.ShowSecondaryInfo, (Vector2)Input.mousePosition, description);
        }
        else
        {
            eventSystem.Invoke(EEvent.HideSecondaryInfo);
        }
    }
}
