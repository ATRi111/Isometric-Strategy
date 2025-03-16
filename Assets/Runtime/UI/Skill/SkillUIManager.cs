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
    /// ��ʼԤ������λ��
    /// </summary>
    public Action<PawnAction> PreviewAction;
    /// <summary>
    /// ֹͣԤ������λ��
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
