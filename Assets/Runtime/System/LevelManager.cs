using Services;
using Services.Event;
using Services.SceneManagement;
using System;
using UnityEngine;

/// <summary>
/// ���ƹؿ�ѡ�񡢳���/����л�
/// </summary>
public class LevelManager : MonoBehaviour
{
    private ISceneController sceneController;
    private IEventSystem eventSystem;
    private GameManager gameManager;

    public Camera prepareMenuCamera;
    public Camera levelCamera;

    public Action OnStartScout;
    public Action OnReturnToPrepareMenu;

    public void StartBattle()
    {
        SwitchToLevel();
        gameManager.StartBattle();
    }

    private void SwitchToPrepareMenu()
    {
        levelCamera.enabled = false;
        prepareMenuCamera.enabled = true;
    }

    private void SwitchToLevel()
    {
        prepareMenuCamera.enabled = false;
        levelCamera.enabled = true;
    }

    private void LoadLevel(int sceneIndex)
    {
        sceneController.LoadScene(sceneIndex, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    private void AfterLoadScene(int index)
    {
        GameObject obj = GameObject.Find("LevelCamera");
        if(obj != null)
        {
            levelCamera = obj.GetComponent<Camera>();
            levelCamera.clearFlags = CameraClearFlags.SolidColor;
            levelCamera.enabled = false;
        }
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
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<int>(EEvent.AfterLoadScene, AfterLoadScene);
    }

    private void Start()
    {
        prepareMenuCamera = Camera.main;
        LoadLevel(3);
    }
}
