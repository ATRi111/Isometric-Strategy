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
    /// ���ü���ѡ�����
    /// </summary>
    public Action<PawnEntity> SelectSkill;
    /// <summary>
    /// ѡ��һ�����ܺ�
    /// </summary>
    public Action<Skill> AfterSelectSkill;
    /// <summary>
    /// ѡ���ܵ��ͷ�λ�ú�
    /// </summary>
    public Action<PawnAction> AfterSelectAction;
    /// <summary>
    /// ȡ��ѡ���ܵ��ͷ�λ�ú�
    /// </summary>
    public Action AfterCancelSelectAction;
    /// <summary>
    /// ��ʼԤ�����ܿ�ѡ��Χ
    /// </summary>
    public Action<Skill> PreviewSkillOption;
    /// <summary>
    /// ֹͣԤ�����ܿ�ѡ��Χ
    /// </summary>
    public Action StopPreviewSkillOption;
    /// <summary>
    /// ��ʼԤ���ж�
    /// </summary>
    public Action<PawnAction> PreviewAction;
    /// <summary>
    /// ֹͣԤ���ж�
    /// </summary>
    public Action<PawnAction> StopPreviewAction;
    /// <summary>
    /// ��ʼԤ����в��Χ
    /// </summary>
    public Action<PawnEntity> PreviewOffenseArea;
    /// <summary>
    /// ֹͣԤ����в��Χ
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
