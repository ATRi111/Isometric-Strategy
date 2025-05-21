using UIExtend;

public class GuidanceButton : ButtonBase
{
    private GuidancePanel panel;
    public EGuidance guidance;

    protected override void OnClick()
    {
        panel.SwitchToGuidance(guidance);
    }

    protected override void Awake()
    {
        base.Awake();
        panel = GetComponentInParent<GuidancePanel>();
    }

    private void Update()
    {
        Button.interactable = panel.current != guidance;
    }
}
