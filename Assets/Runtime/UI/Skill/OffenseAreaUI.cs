using Services;
using Services.ObjectPools;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// µÐÈËÍþÐ²·¶Î§
/// </summary>
public class OffenseAreaUI : MonoBehaviour
{
    private IsometricGridManager Igm => IsometricGridManager.Instance;
    private SkillUIManager skillUIManager;
    private IObjectManager objectManager;

    private void PreviewOffenseArea(PawnEntity pawn)
    {
        ObjectPoolUtility.RecycleMyObjects(gameObject);
        HashSet<Vector3Int> offenseArea = pawn.SkillManager.offenseArea;
        foreach (Vector3Int p in offenseArea)
        {
            Vector3 world = Igm.CellToWorld(p);
            objectManager.Activate("OffenseAreaIcon", world, Vector3.zero, transform);
        }
    }

    private void StopPreviewOffenseArea(PawnEntity _)
    {
        ObjectPoolUtility.RecycleMyObjects(gameObject);
    }

    private void AfterSelectAction(PawnAction _)
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
        skillUIManager.PreviewOffenseArea += PreviewOffenseArea;
        skillUIManager.StopPreviewOffenseArea += StopPreviewOffenseArea;
        skillUIManager.AfterSelectAction += AfterSelectAction;
    }

    private void OnDisable()
    {
        skillUIManager.PreviewOffenseArea -= PreviewOffenseArea;
        skillUIManager.StopPreviewOffenseArea -= StopPreviewOffenseArea;
        skillUIManager.AfterSelectAction -= AfterSelectAction;
    }
}
