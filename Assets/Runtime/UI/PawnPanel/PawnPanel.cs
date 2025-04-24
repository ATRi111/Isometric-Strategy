using Services.Event;
using System;
using System.Collections.Generic;

public class PawnPanel : PawnReference
{
    public IsometricGridManager Igm => IsometricGridManager.Instance;
    private LevelManager levelManager;

    /// <summary>
    /// 开始显示角色属性变化
    /// </summary>
    public Action PreviewPropertyChange;
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
    public override PawnEntity CurrentPawn => pawnList[selectedIndex];

    public float GetPropertyChange(string propertyName)
    {
        //TODO:装备切换预览
        if (propertyChangeDict.ContainsKey(propertyName))
            return propertyChangeDict[propertyName];
        return 0f;
    }

    public void Next()
    {
        if (pawnList != null && pawnList.Count > 0)
            selectedIndex = (selectedIndex + 1) % pawnList.Count;
        OnRefresh?.Invoke();
    }

    public void Previous()
    {
        if (pawnList != null && pawnList.Count > 0)
            selectedIndex = (selectedIndex + pawnList.Count - 1) % pawnList.Count;
        OnRefresh?.Invoke();
    }

    private void Show(PawnEntity pawn)
    {
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
        OnRefresh?.Invoke();
    }

    private void Hide()
    {
        canvasGroup.Visible = false;
    }
    protected override void Awake()
    {
        base.Awake();
        selectedIndex = 0;
        levelManager = LevelManager.FindInstance();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        eventSystem.AddListener<PawnEntity>(EEvent.ShowPawnPanel, Show);
        levelManager.OnReturnToPrepareMenu += Hide;
        levelManager.OnStartScout += Hide;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        eventSystem.RemoveListener(EEvent.HidePawnPanel, Hide);
        levelManager.OnReturnToPrepareMenu -= Hide;
        levelManager.OnStartScout -= Hide;
    }
}
