using Services;
using Services.Event;
using System;
using System.Collections.Generic;
using UIExtend;
using UnityEngine;

public class PawnPanel : MonoBehaviour
{
    public IsometricGridManager Igm => IsometricGridManager.Instance;
    private IEventSystem eventSystem;

    private CanvasGroupPlus canvasGroup;

    public Action<PawnEntity> RefreshAll;

    /// <summary>
    /// 开始显示角色属性变化
    /// </summary>
    public Action<PawnEntity> PreviewPropertyChange;
    /// <summary>
    /// 停止显示角色属性变化
    /// </summary>
    public Action StopPreviewPropertyChange;
    /// <summary>
    /// 点击装备栏位来开始或停止切换装备
    /// </summary>
    public Action<EquipmentSlot> ChangeEquipment;

    public Dictionary<string, float> propertyChangeDict = new();

    public int selectedIndex;
    /// <summary>
    /// 当前查看的角色列表
    /// </summary>
    public List<PawnEntity> pawnList;
    public PawnEntity SelectedPawn => pawnList[selectedIndex];

    public float GetPropertyChange(string propertyName)
    {
        //TODO:装备切换预览
        if (propertyChangeDict.ContainsKey(propertyName))
            return propertyChangeDict[propertyName];
        return UnityEngine.Random.Range(-2f, 2f);
    }

    public void Next()
    {
        if (pawnList != null && pawnList.Count > 0)
            selectedIndex = (selectedIndex + 1) % pawnList.Count;
        RefreshAll?.Invoke(SelectedPawn);
    }

    public void Previous()
    {
        if(pawnList!= null && pawnList.Count > 0)
            selectedIndex = (selectedIndex + pawnList.Count - 1) % pawnList.Count;
        RefreshAll?.Invoke(SelectedPawn);
    }

    private void Show(PawnEntity pawn)
    {
        //TODO:装备切换
        PawnEntity[] temp = Igm.GetComponentsInChildren<PawnEntity>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (!temp[i].hidden)
                pawnList.Add(temp[i]);
        } 
        selectedIndex = 0;
        for (int i = 0; i < pawnList.Count; i++)
        {
            if(pawnList[i] == pawn)
                selectedIndex = i;
        }
        canvasGroup.Visible = true;
        RefreshAll?.Invoke(SelectedPawn);
    }

    private void Hide()
    {
        canvasGroup.Visible = false;
    }

    private void OnEnable()
    {
        eventSystem.AddListener<PawnEntity>(EEvent.ShowPawnPanel, Show);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener(EEvent.HidePawnPanel, Hide);
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        canvasGroup = GetComponent<CanvasGroupPlus>();
        selectedIndex = 0;
    }

    private void Update()
    {
        if(canvasGroup.Visible)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Hide();
            }
        }
    }
}
