using MyTimer;
using UIExtend;
using UnityEngine;

public class MoveBattleLogButton : ButtonBase
{
    private RectTransform target;
    private UniformLinearMotion ulm;
    private bool moved;

    public Vector3 offset;
    public float duration;

    protected override void OnClick()
    {
        if (moved)
        {
            ulm.Initialize(target.anchoredPosition3D, target.anchoredPosition3D - offset, duration);
            Button.interactable = false;
            moved = false;
            tmp.text = "收起";
        }
        else
        {
            ulm.Initialize(target.anchoredPosition3D, target.anchoredPosition3D + offset, duration);
            Button.interactable = false;
            moved = true;
            tmp.text = "展开";
        }
    }

    private void OnTick(Vector3 position)
    {
        target.anchoredPosition3D = position;
    }

    private void AfterComplete(Vector3 position)
    {
        Button.interactable = true;
        target.anchoredPosition3D = position;
    }

    protected override void Awake()
    {
        base.Awake();
        ulm = new();
        ulm.AfterComplete += AfterComplete;
        ulm.OnTick += OnTick;
        target = transform.parent.GetComponent<RectTransform>();
    }

    private void OnDisable()
    {
        ulm.Paused = true;
    }
}
