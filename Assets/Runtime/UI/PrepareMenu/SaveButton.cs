using Services;
using Services.Save;
using UIExtend;

public class SaveButton : ButtonBase
{
    private SaveGroupController groupController;

    protected override void OnClick()
    {
        groupController.Save();
    }

    protected override void Awake()
    {
        base.Awake();
        groupController = ServiceLocator.Get<ISaveManager>().GetGroup(1);
    }
}
