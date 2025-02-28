using Services;
using Services.SceneManagement;
using UIExtend;

public class LoadSceneButton : ButtonBase
{
    private ISceneController sceneController;
    public string sceneName;

    protected override void OnClick()
    {
        sceneController.LoadScene(sceneName);
    }

    protected override void Awake()
    {
        base.Awake();
        sceneController = ServiceLocator.Get<ISceneController>();
    }
}
