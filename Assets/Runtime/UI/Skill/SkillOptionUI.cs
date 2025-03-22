using Services;
using Services.ObjectPools;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能可选范围
/// </summary>
public class SkillOptionUI : MonoBehaviour
{
    private IsometricGridManager Igm => IsometricGridManager.Instance;
    private SkillUIManager skillUIManager;
    private IObjectManager objectManager;
    private readonly List<Vector3Int> options = new();

    private void PreviewSkillOption(Skill skill)
    {
        ObjectPoolUtility.RecycleMyObjects(gameObject);
        PawnEntity pawn = skillUIManager.currentPawn;
        if (skill.CanUse(pawn, Igm))
        {
            skill.GetOptions(pawn, Igm, pawn.GridObject.CellPosition, options);
            for (int i = 0; i < options.Count; i++)
            {
                Vector3 world = Igm.CellToWorld(options[i]);
                objectManager.Activate("SkillOptionIcon", world, Vector3.zero, transform);
            }
        }
    }

    private void StopPreviewSkillOption()
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
        skillUIManager.PreviewSkillOption += PreviewSkillOption;
        skillUIManager.StopPreviewSkillOption += StopPreviewSkillOption;
        skillUIManager.AfterSelectAction += AfterSelectAction;
    }

    private void OnDisable()
    {
        skillUIManager.PreviewSkillOption -= PreviewSkillOption;
        skillUIManager.StopPreviewSkillOption -= StopPreviewSkillOption;
        skillUIManager.AfterSelectAction -= AfterSelectAction;
    }
}
