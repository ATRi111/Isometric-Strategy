using MyTool;
using Services;
using Services.ObjectPools;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 选择技能释放位置
/// </summary>
public class SelectActionUI : MonoBehaviour
{
    private IsometricGridManager Igm => IsometricGridManager.Instance;
    private SkillUIManager skillUIManager;
    private IObjectManager objectManager;

    private readonly List<PawnAction> currentActions = new();
    public bool isSelecting;

    private void AfterSelectSkill(Skill skill)
    {
        isSelecting = true;
        ObjectPoolUtility.RecycleMyObjects(gameObject);
        currentActions.Clear();
        skillUIManager.currentPawn.Brain.MakeAction(skill, currentActions);

        for (int i = 0; i < currentActions.Count; i++)
        {
            Vector3 world = Igm.CellToWorld(currentActions[i].target);
            IMyObject obj = objectManager.Activate("ActionIcon", world, Vector3.zero, transform);
            obj.Transform.SetLossyScale(Vector3.one);
            ActionIcon icon = obj.Transform.GetComponent<ActionIcon>();
            icon.SetAction(currentActions[i]);
        }
    }

    private void AfterSelectAction(PawnAction _)
    {
        ObjectPoolUtility.RecycleMyObjects(gameObject);
        isSelecting = false;
    }

    private void Awake()
    {
        skillUIManager = SkillUIManager.FindInstance();
        objectManager = ServiceLocator.Get<IObjectManager>();
    }

    private void OnEnable()
    {
        skillUIManager.AfterSelectSkill += AfterSelectSkill;
        skillUIManager.AfterSelectAction += AfterSelectAction;
    }

    private void OnDisable()
    {
        skillUIManager.AfterSelectSkill -= AfterSelectSkill;
        skillUIManager.AfterSelectAction -= AfterSelectAction;
    }

    private void Update()
    {
        if (isSelecting && Input.GetMouseButtonDown(1))
        {
            ObjectPoolUtility.RecycleMyObjects(gameObject);
            skillUIManager.AfterCancelSelectAction?.Invoke();
        }
    }
}
