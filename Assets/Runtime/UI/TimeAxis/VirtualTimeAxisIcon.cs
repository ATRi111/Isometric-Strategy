using UnityEngine;

public class VirtualTimeAxisIcon : MonoBehaviour
{
    private SkillUIManager skillUIManager;
    private TimeAxisUI timeAxisUI;

    public void Hide()
    {
        transform.position = new Vector3(-1000, 0, 0);
    }

    private void PreviewAction(PawnAction action)
    {
        int predictTime = action.effectUnit.timeEffect.current;
        transform.position = timeAxisUI.PercentToPosition(predictTime);
    }

    private void StopPreviewAction(PawnAction _)
        => Hide();

    private void Awake()
    {
        timeAxisUI = GetComponentInParent<TimeAxisUI>();
        skillUIManager = SkillUIManager.FindInstance();
        skillUIManager.PreviewAction += PreviewAction;
        skillUIManager.StopPreviewAction += StopPreviewAction;
        skillUIManager.AfterSelectAction += StopPreviewAction;
        skillUIManager.AfterCancelSelectAction += Hide;
    }
}
