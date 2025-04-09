using MyTool;
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

    public static Vector2 DirectionToPivot(Vector2 direction)
    {
        direction = direction.normalized;
        Vector2 pivot = -direction;
        float absx = Mathf.Abs(direction.x);
        float absy = Mathf.Abs(direction.y);
        float k = 1f;
        if (absx >= absy)
            k = 1f / absx;
        else
            k = 1f / absy;
        pivot *= k;
        pivot = 0.5f * (pivot + Vector2.one);
        return pivot;
    }

    private RectTransform rectTransform;
    private CanvasGroupPlus canvasGrounp;
    private KeyWordList keyWordList;

    private bool focusOnIcon;
    private bool containsMouse;

    private bool locked;

    private void ShowInfo(string info, float normalizedDistance)
    {
        focusOnIcon = true;
        DevideFirstLine(info, out string firstLine, out string left);
        if (firstLine.StartsWith("<b>") && firstLine.EndsWith("</b>"))
            info = firstLine + "\n" + keyWordList.MarkAllKeyWords(left, Mark);     //第一行如果为粗体,则不会被标记
        else
            info = keyWordList.MarkAllKeyWords(info, Mark);
        TextUI.text = info;

        Vector3 direction = Input.mousePosition - 0.5f * new Vector3(Screen.width, Screen.height);
        rectTransform.pivot = DirectionToPivot(direction);

        Vector3 position = normalizedDistance * Screen.width * direction.normalized + Input.mousePosition;
        position = UIExtendUtility.ClampInScreen(position);
        transform.position = position.ResetZ(transform.position.z);


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
        eventSystem.AddListener<string, float>(EEvent.ShowInfo, ShowInfo);
        eventSystem.AddListener<object>(EEvent.HideInfo, HideInfo);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<string, float>(EEvent.ShowInfo, ShowInfo);
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
            if((Vector2)transform.position == (Vector2)Input.mousePosition)
            {
                if (!UIExtendUtility.WithinScreen(rectTransform))
                {
                    Vector2 direction = (Vector2)Input.mousePosition - 0.5f * new Vector2(Screen.width, Screen.height);
                    rectTransform.pivot = DirectionToPivot(-direction);
                }
            }
            else
            {
                UIExtendUtility.ClampInScreen(rectTransform);
            }
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
