using Services;
using Services.Event;
using System;
using UnityEngine;

public class SkillUIManager : MonoBehaviour
{
    public static SkillUIManager FindInstance()
    {
        return GameObject.Find(nameof(SkillUIManager)).GetComponent<SkillUIManager>();
    }

    public IEventSystem EventSystem { get; private set; }
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
    /// 开始预览技能位置
    /// </summary>
    public Action<PawnAction> PreviewAction;
    /// <summary>
    /// 停止预览技能位置
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
        EventSystem = ServiceLocator.Get<IEventSystem>();
        AfterCancelSelectAction += ReselectSkill;
        AfterSelectAction += ExecuteAction;
    }

    private void OnEnable()
    {
        EventSystem.AddListener<PawnEntity>(EEvent.BeforeDoAction, BeforeDoAction);
    }

    private void OnDisable()
    {
        EventSystem.RemoveListener<PawnEntity>(EEvent.BeforeDoAction, BeforeDoAction);
    }
}
