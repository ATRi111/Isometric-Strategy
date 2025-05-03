using MyTool;
using Services;
using Services.Event;
using Services.ObjectPools;
using System;
using UnityEngine;

public class SkillUIManager : MonoBehaviour
{
    public static SkillUIManager FindInstance()
    {
        return GameObject.Find(nameof(SkillUIManager)).GetComponent<SkillUIManager>();
    }

    private IEventSystem eventSystem;
    private IObjectManager objectManager;
    private IsometricGridManager Igm => IsometricGridManager.Instance;
    private IMyObject actorIcon;

    /// <summary>
    /// 启用技能选择面板
    /// </summary>
    public Action<PawnEntity> SelectSkill;
    /// <summary>
    /// 选中一个技能后
    /// </summary>
    public Action<Skill> AfterSelectSkill;
    /// <summary>
    /// 选择技能的释放位置后
    /// </summary>
    public Action<PawnAction> AfterSelectAction;
    /// <summary>
    /// 取消选择技能的释放位置后
    /// </summary>
    public Action AfterCancelSelectAction;
    /// <summary>
    /// 开始预览技能可选范围
    /// </summary>
    public Action<Skill> PreviewSkillOption;
    /// <summary>
    /// 停止预览技能可选范围
    /// </summary>
    public Action StopPreviewSkillOption;
    /// <summary>
    /// 开始预览行动
    /// </summary>
    public Action<PawnAction> PreviewAction;
    /// <summary>
    /// 停止预览行动
    /// </summary>
    public Action<PawnAction> StopPreviewAction;
    /// <summary>
    /// 开始预览威胁范围
    /// </summary>
    public Action<PawnEntity> PreviewOffenseArea;
    /// <summary>
    /// 停止预览威胁范围
    /// </summary>
    public Action<PawnEntity> StopPreviewOffenseArea;

    public PawnEntity currentPawn;

    private void BeforeDoAction(PawnEntity pawn)
    {
        currentPawn = pawn;
        if (pawn.Brain.humanControl)
            SelectSkill?.Invoke(currentPawn);

        actorIcon?.Recycle();
        actorIcon = objectManager.Activate("ActorIcon", Igm.CellToWorld(currentPawn.GridObject.CellPosition), Vector3.zero, transform);
        actorIcon.Transform.SetLossyScale(Vector3.one);
    }

    private void ReselectSkill()
    {
        SelectSkill?.Invoke(currentPawn);
    }

    private void ExecuteAction(PawnAction action)
    {
        currentPawn.Brain.ExecuteAction(action);
        currentPawn = null;
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        objectManager = ServiceLocator.Get<IObjectManager>();
        AfterCancelSelectAction += ReselectSkill;
        AfterSelectAction += ExecuteAction;
    }

    private void OnEnable()
    {
        eventSystem.AddListener<PawnEntity>(EEvent.BeforeDoAction, BeforeDoAction);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<PawnEntity>(EEvent.BeforeDoAction, BeforeDoAction);
    }
}
