using Services;
using Services.SceneManagement;
using UIExtend;

public class StartGameButton : ButtonBase
{
    protected override void OnClick()
    {
        ServiceLocator.Get<ISceneController>().LoadNextScene();       
    }
}