using Services;
using Services.ObjectPools;
using System.Collections.Generic;
using UnityEngine;

public class ActionAreaUI : MonoBehaviour
{
    private IsometricGridManager Igm => IsometricGridManager.Instance;
    private SkillUIManager skillUIManager;
    private IObjectManager objectManager;
    private readonly List<Vector3Int> area = new();

    private void PreviewAction(PawnAction action)
    {
        ObjectPoolUtility.RecycleMyObjects(gameObject);
        AimSkill aimSkill = action.skill as AimSkill;
        if(aimSkill != null)
        {
            aimSkill.MockArea(Igm, action.agent.GridObject.CellPosition, action.target, area);
            for (int i = 0; i < area.Count; i++)
            {
                Vector3 world = Igm.CellToWorld(area[i]);
                objectManager.Activate("ActionAreaIcon", world, Vector3.zero, transform);
            }
        }
    }

    private void StopPreviewAction(PawnAction _)
    {
        ObjectPoolUtility.RecycleMyObjects(gameObject);
    }

    private void AfterSelectAction(PawnAction _)
    {
        ObjectPoolUtility.RecycleMyObjects(gameObject);
    }

    private void AfterCancelSelectAction()
    {
        ObjectPoolUtility.RecycleMyObjects(gameObject);
    }

    private void Awake()
    {
        skillUIManager = SkillUIManager.FindInstance();
        objectManager = ServiceLocator.Get<IObjectManager>();
    }

    private void OnEnable()
    {
        skillUIManager.PreviewAction += PreviewAction;
        skillUIManager.StopPreviewAction += StopPreviewAction;
        skillUIManager.AfterSelectAction += AfterSelectAction;
        skillUIManager.AfterCancelSelectAction += AfterCancelSelectAction;
    }

    private void OnDisable()
    {
        skillUIManager.PreviewAction -= PreviewAction;
        skillUIManager.StopPreviewAction -= StopPreviewAction;
        skillUIManager.AfterSelectAction -= AfterSelectAction;
        skillUIManager.AfterCancelSelectAction -= AfterCancelSelectAction;
    }
}
