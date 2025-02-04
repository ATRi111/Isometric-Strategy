using UIExtend;

public class HideButton : ButtonBase
{
    public CanvasGroupPlus canvasGroup;

    protected override void OnClick()
    {
        canvasGroup.Visible = false;
    }
}
