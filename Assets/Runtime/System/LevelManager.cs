using Services;
using Services.Event;
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

    private IEventSystem eventSystem;
    private GameManager gameManager;

    public Camera prepareMenuCamera;
    public Camera levelCamera;

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
        gameManager.StartBattle();
    }

    private void SwitchToPrepareMenu()
    {
        if (levelCamera != null)
            levelCamera.gameObject.SetActive(false);
        prepareMenuCamera.gameObject.SetActive(true);
    }

    private void AfterLoadScene(int index)
    {
        GameObject obj = GameObject.Find("LevelCamera");
        if (obj != null)
        {
            levelCamera = obj.GetComponent<Camera>();
            levelCamera.clearFlags = CameraClearFlags.SolidColor;
            prepareMenuCamera.gameObject.SetActive(false);
            levelCamera.gameObject.SetActive(true);
        }
    }

    private void AfterUnLoadScene(int index)
    {
        OnReturnToPrepareMenu?.Invoke();
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        gameManager = ServiceLocator.Get<GameManager>();
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
