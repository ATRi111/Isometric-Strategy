using Services;
using Services.Event;
using Services.SceneManagement;
using System;
using UnityEngine;

/// <summary>
/// 控制关卡选择、场景/相机切换
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager FindInstance()
    {
        return GameObject.Find("PrepareMenu").GetComponent<LevelManager>();
    }

    private ISceneController sceneController;
    private IEventSystem eventSystem;
    private GameManager gameManager;

    public Camera prepareMenuCamera;
    public Camera levelCamera;

    /// <summary>
    /// 开始侦察战场
    /// </summary>
    public Action OnStartScout;
    /// <summary>
    /// （战斗结束或侦察结束后）回到准备菜单
    /// </summary>
    public Action OnReturnToPrepareMenu;
    /// <summary>
    /// 显示关卡描述
    /// </summary>
    public Action ShowLevelDescription;

    public void StartBattle()
    {
        SwitchToLevel();
        gameManager.StartBattle();
    }

    private void SwitchToPrepareMenu()
    {
        if (levelCamera != null)
            levelCamera.gameObject.SetActive(false);
        prepareMenuCamera.gameObject.SetActive(true);
    }

    private void SwitchToLevel()
    {
        prepareMenuCamera.gameObject.SetActive(false);
        if (levelCamera != null)
            levelCamera.gameObject.SetActive(true);
    }

    private void LoadLevel(int sceneIndex)
    {
        sceneController.LoadScene(sceneIndex, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    private void AfterLoadScene(int index)
    {
        GameObject obj = GameObject.Find("LevelCamera");
        if (obj != null)
        {
            levelCamera = obj.GetComponent<Camera>();
            levelCamera.clearFlags = CameraClearFlags.SolidColor;
            levelCamera.gameObject.SetActive(false);
        }
    }

    private void AfterUnLoadScene(int index)
    {
        OnReturnToPrepareMenu?.Invoke();
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        sceneController = ServiceLocator.Get<ISceneController>();
        gameManager = ServiceLocator.Get<GameManager>();
        OnStartScout += SwitchToLevel;
        OnReturnToPrepareMenu += SwitchToPrepareMenu;
    }

    private void OnEnable()
    {
        eventSystem.AddListener<int>(EEvent.AfterLoadScene, AfterLoadScene);
        eventSystem.AddListener<int>(EEvent.AfterUnLoadScene, AfterUnLoadScene);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<int>(EEvent.AfterLoadScene, AfterLoadScene);
        eventSystem.RemoveListener<int>(EEvent.AfterUnLoadScene, AfterUnLoadScene);
    }

    private void Start()
    {
        prepareMenuCamera = Camera.main;
    }
}
