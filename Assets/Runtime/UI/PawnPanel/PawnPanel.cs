using Services.Event;
using System;
using System.Collections.Generic;

public class PawnPanel : PawnReference
{
    public IsometricGridManager Igm => IsometricGridManager.Instance;
    private LevelManager levelManager;

    /// <summary>
    /// ��ʼ��ʾ��ɫ���Ա仯
    /// </summary>
    public Action PreviewPropertyChange;
    /// <summary>
    /// ֹͣ��ʾ��ɫ���Ա仯
    /// </summary>
    public Action StopPreviewPropertyChange;
    /// <summary>
    /// ���װ����λ����ʼ��ֹͣ�л�װ��
    /// </summary>
    public Action<EquipmentSlot> ChangeEquipment;

    public Dictionary<string, float> propertyChangeDict = new();

    public int selectedIndex;
    /// <summary>
    /// ��ǰ�鿴�Ľ�ɫ�б�
    /// </summary>
    public List<PawnEntity> pawnList;
    public override PawnEntity CurrentPawn => pawnList[selectedIndex];

    public float GetPropertyChange(string propertyName)
    {
        //TODO:װ���л�Ԥ��
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
