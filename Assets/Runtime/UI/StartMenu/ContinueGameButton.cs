using Services;
using Services.Save;
using Services.SceneManagement;
using UIExtend;

public class ContinueGameButton : ButtonBase
{
    protected override void OnClick()
    {
        ServiceLocator.Get<ISaveManager>().GetGroup(1).Load();
        ServiceLocator.Get<ISceneController>().LoadNextScene();
    }
}