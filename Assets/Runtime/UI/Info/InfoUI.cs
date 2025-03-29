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

    public static void DevideFirstLine(string text, out string firstLine, out string left)
    {
        int index = text.IndexOf("\n");
        if (index != -1)
        {
            firstLine = text[..index];
            left = text[(index + 1)..];
        }
        else
        {
            firstLine = text;
            left = string.Empty;
        }
    }

    private RectTransform rectTransform;
    private CanvasGroupPlus canvasGrounp;
    private KeyWordList keyWordList;

    private bool focusOnIcon;
    private bool containsMouse;
    [SerializeField]
    private Vector2 offset;

    private bool locked;

    private void ShowInfo(Vector2 screenPoint, Vector2 pivot, string info)
    {
        focusOnIcon = true;
        DevideFirstLine(info, out string firstLine, out string left);
        if (firstLine.StartsWith("<b>") && firstLine.EndsWith("</b>"))
            info = firstLine + "\n" + keyWordList.MarkAllKeyWords(left, Mark);     //第一行如果为粗体,则不会被标记
        else
            info = keyWordList.MarkAllKeyWords(info, Mark);
        TextUI.text = info;
        rectTransform.pivot = pivot;
        transform.position = screenPoint + offset;
        UIExtendUtility.ClampInScreen(rectTransform);
        canvasGrounp.Visible = true;
    }

    private void HideInfo(object source)
    {
        focusOnIcon = false;
    }

    private void LockUnlock()
    {
        if(locked)
        {
            canvasGrounp.threshold_blockRaycast = 1f;
            locked = false;
        }
        else
        {
            canvasGrounp.threshold_blockRaycast = 0f;
            locked = true;
        }
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
        eventSystem.AddListener<Vector2, Vector2, string>(EEvent.ShowInfo, ShowInfo);
        eventSystem.AddListener<object>(EEvent.HideInfo, HideInfo);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<Vector2, Vector2, string>(EEvent.ShowInfo, ShowInfo);
        eventSystem.RemoveListener<object>(EEvent.HideInfo, HideInfo);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            canvasGrounp.threshold_blockRaycast = 1f;
            locked = false;
        }

        UIExtendUtility.GetBorder(rectTransform, out float left, out float right, out float bottom, out float top);
        Vector2 mouse = Input.mousePosition;
        containsMouse = mouse.x >= left && mouse.x <= right
            && mouse.y >= bottom && mouse.y <= top;

        if (!containsMouse && !focusOnIcon && !locked)
            canvasGrounp.Visible = false;

        if (canvasGrounp.Visible)
        {
            CheckSecondaryInfo();
            UIExtendUtility.ClampInScreen(rectTransform);
        }

        if(Input.GetKeyUp(KeyCode.T))
            LockUnlock();
    }

    private void CheckSecondaryInfo()
    {
        Vector3 mouse = Input.mousePosition;
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(TextUI, mouse, null);
        if (linkIndex > -1)
        {
            TMP_LinkInfo linkInfo = TextUI.textInfo.linkInfo[linkIndex];
            string description = keyWordList.GetDescription(linkInfo.GetLinkText());
            if (!string.IsNullOrWhiteSpace(description))
                eventSystem.Invoke(EEvent.ShowSecondaryInfo, (Vector2)Input.mousePosition, description);
        }
        else
        {
            eventSystem.Invoke(EEvent.HideSecondaryInfo);
        }
    }
}
