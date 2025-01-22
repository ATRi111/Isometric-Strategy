//战斗和侦察开始时自动隐藏的UI
public class PrepareMenuBackground : PrepareMenuUI
{
    protected override void OnStartScout()
    {
        base.OnStartScout();
        canvasGroup.Visible = false;
    }
}
