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

    public Action<PawnEntity> RefreshAll;

    public Action<PawnEntity> PreviewPropertyChange;
    public Action StopPreviewPropertyChange;

    public Dictionary<string, float> propertyChangeDict = new();


    public int selectedIndex;
    /// <summary>
    /// 当前查看的角色列表
    /// </summary>
    public List<PawnEntity> pawnList;
    public PawnEntity SelectedPawn => pawnList[selectedIndex];

    public float GetPropertyChange(string propertyName)
    {
        if(propertyChangeDict.ContainsKey(propertyName))
            return propertyChangeDict[propertyName];
        return UnityEngine.Random.Range(-2f, 2f);
    }

    private void Show()
    {
        canvasGroup.Visible = true;
        RefreshAll?.Invoke(SelectedPawn);
        PreviewPropertyChange?.Invoke(SelectedPawn);
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
            {
                pawnList.Clear();
                pawnList.AddRange(playerManager.playerList);
                selectedIndex = 0;
                Show();
            }
            else
                Hide();
        }
    }
}
