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
    private LevelManager levelManager;

    private CanvasGroupPlus canvasGroup;

    public Action RefreshAll;

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
    public PawnEntity SelectedPawn => pawnList[selectedIndex];

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
        RefreshAll?.Invoke();
    }

    public void Previous()
    {
        if(pawnList!= null && pawnList.Count > 0)
            selectedIndex = (selectedIndex + pawnList.Count - 1) % pawnList.Count;
        RefreshAll?.Invoke();
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
        RefreshAll?.Invoke();
    }

    private void Hide()
    {
        canvasGroup.Visible = false;
    }
    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        canvasGroup = GetComponent<CanvasGroupPlus>();
        selectedIndex = 0;
        levelManager = GameObject.Find("PrepareMenu").GetComponent<LevelManager>();
    }

    private void OnEnable()
    {
        eventSystem.AddListener<PawnEntity>(EEvent.ShowPawnPanel, Show);
        levelManager.OnReturnToPrepareMenu += Hide;
        levelManager.OnStartScout += Hide;
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener(EEvent.HidePawnPanel, Hide);
        levelManager.OnReturnToPrepareMenu -= Hide;
        levelManager.OnStartScout -= Hide;
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
