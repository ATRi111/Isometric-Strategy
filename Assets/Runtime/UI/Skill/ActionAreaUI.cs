using Services;
using Services.ObjectPools;
using UnityEngine;

/// <summary>
/// 技能作用范围
/// </summary>
public class ActionAreaUI : MonoBehaviour
{
    private IsometricGridManager Igm => IsometricGridManager.Instance;
    private SkillUIManager skillUIManager;
    private IObjectManager objectManager;

    private void PreviewAction(PawnAction action)
    {
        ObjectPoolUtility.RecycleMyObjects(gameObject);
        for (int i = 0; i < action.area.Count; i++)
        {
            Vector3 world = Igm.CellToWorld(action.area[i]);
            objectManager.Activate("ActionAreaIcon", world, Vector3.zero, transform);
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
