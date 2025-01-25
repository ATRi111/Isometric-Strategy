using Services;
using Services.SceneManagement;
using UIExtend;

public class ReturnButton : ButtonBase
{
    protected override void OnClick()
    {
        ServiceLocator.Get<ISceneController>().LoadScene(2);
    }
}
