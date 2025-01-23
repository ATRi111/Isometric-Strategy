using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

public class SkillTrajectory : MonoBehaviour
{
    private IsometricGridManager Igm => IsometricGridManager.Instance;
    private SkillUIManager skillUIManager;
    private LineRenderer lineRenderer;

    private PawnAction currentAction;

    private Vector3[] points;
    [SerializeField]
    private Color color_hit;
    [SerializeField]
    private Color color_miss;

    private readonly List<Vector3> trajectory = new();

    private void PreviewAction(PawnAction action)
    {
        currentAction = action;
        ProjectileSkill projectile = action.skill as ProjectileSkill;
        if (projectile != null)
        {
            GridObject victim = projectile.HitCheck(action.agent, Igm, action.target, trajectory);
            points = new Vector3[trajectory.Count];
            for (int i = 0; i < trajectory.Count; i++)
            {
                points[i] = Igm.CellToWorld(trajectory[i]);
            }
            bool hitEntity = victim != null && victim.GetComponent<Entity>() != null;
            Color color = hitEntity ? color_hit : color_miss;
            lineRenderer.startColor = lineRenderer.endColor = color;
            lineRenderer.positionCount = points.Length;
            lineRenderer.SetPositions(points);
            lineRenderer.enabled = true;
        }
    }

    private void StopPreviewAction(PawnAction action)
    {
        if (action == currentAction)
        {
            currentAction = null;
            lineRenderer.enabled = false;
        }
    }

    private void AfterCancelSelectAction()
    {
        currentAction = null;
        lineRenderer.enabled = false;
    }

    private void AfterSelectAction(PawnAction _)
    {
        currentAction = null;
        lineRenderer.enabled = false;
    }

    private void Awake()
    {
        skillUIManager = GetComponentInParent<SkillUIManager>();
        lineRenderer = GetComponent<LineRenderer>();
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
