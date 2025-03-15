using UnityEngine;

public class VirtualTimeAxisIcon : MonoBehaviour
{
    private SkillUIManager skillUIManager;
    private TimeAxisUI timeAxisUI;

    private void PreviewAction(PawnAction action)
    {
        int predictTime = action.effectUnit.timeEffect.current;
        transform.position = timeAxisUI.TimeToPosition(predictTime);
    }

    private void StopPreviewAction(PawnAction _)
    {
        transform.position = timeAxisUI.TimeToPosition(10000);
    }

    private void Awake()
    {
        timeAxisUI = GetComponentInParent<TimeAxisUI>();
        skillUIManager = SkillUIManager.FindInstance();
        skillUIManager.PreviewAction += PreviewAction;
        skillUIManager.StopPreviewAction += StopPreviewAction;
    }
}
