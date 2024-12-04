using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

public class SkillTrajectory : MonoBehaviour
{
    private IsometricGridManager igm;
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
        ProjectileSkill skill = action.skill as ProjectileSkill;
        if (skill != null)
        {
            GridObject victim = skill.HitCheck(action.agent, igm, action.target, trajectory);
            points = new Vector3[trajectory.Count];
            for (int i = 0; i < trajectory.Count; i++)
            {
                points[i] = igm.CellToWorld(trajectory[i]);
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
        if(action == currentAction)
        {
            currentAction = null;
            lineRenderer.enabled = false;
        }
    }

    private void AfterSelectAction(PawnAction _)
    {
        currentAction = null;
        lineRenderer.enabled = false;
    }

    private void Awake()
    {
        igm = IsometricGridManager.FindInstance();
        skillUIManager = GetComponentInParent<SkillUIManager>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        skillUIManager.PreviewAction += PreviewAction;
        skillUIManager.StopPreviewAction += StopPreviewAction;
        skillUIManager.AfterSelectAction += AfterSelectAction;
    }

    private void OnDisable()
    {
        skillUIManager.PreviewAction -= PreviewAction;
        skillUIManager.StopPreviewAction -= StopPreviewAction;
        skillUIManager.AfterSelectAction -= AfterSelectAction;
    }
}
