public class PrepareMenuBackground : PrepareMenuUI
{
    protected override void OnStartScout()
    {
        base.OnStartScout();
        canvasGroup.Visible = false;
    }
}
