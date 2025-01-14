using Services;
using Services.Event;
using System;
using System.Collections.Generic;
using UIExtend;
using UnityEngine;

public class PawnPanel : MonoBehaviour
{
    private IEventSystem eventSystem;
    private PlayerManager playerManager;

    private CanvasGroupPlus canvasGroup;

    public Action<PawnEntity> RefreshProperty;

    public Dictionary<string, float> propertyChangeDict = new();
    public Action<PawnEntity> RefreshPropertyChange;
    public Action StopPreviewPropertyChange;


    public int selectedIndex;
    public PawnEntity SelectedPawn => playerManager.spawnedPlayers[selectedIndex];

    public float GetPropertyChange(string propertyName)
    {
        if(propertyChangeDict.ContainsKey(propertyName))
            return propertyChangeDict[propertyName];
        return UnityEngine.Random.Range(-2f, 2f);
    }

    private void Show()
    {
        canvasGroup.Visible = true;
        RefreshProperty?.Invoke(SelectedPawn);
        RefreshPropertyChange?.Invoke(SelectedPawn);
    }

    private void Hide()
    {
        canvasGroup.Visible = false;
    }

    private void OnEnable()
    {
        eventSystem.AddListener(EEvent.ShowPawnPanel, Show);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener(EEvent.HidePawnPanel, Hide);
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        canvasGroup = GetComponent<CanvasGroupPlus>();
        playerManager = PlayerManager.FindInstance();
        selectedIndex = 0;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!canvasGroup.Visible)
                Show();
            else
                Hide();
        }
    }
}
