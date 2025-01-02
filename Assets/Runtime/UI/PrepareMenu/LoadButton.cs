using Services;
using Services.Save;
using UIExtend;

public class LoadButton : ButtonBase
{
    private SaveGroupController groupController;

    protected override void OnClick()
    {
        groupController.Load();
    }

    protected override void Awake()
    {
        base.Awake();
        groupController = ServiceLocator.Get<ISaveManager>().GetGroup(1);
    }
}
