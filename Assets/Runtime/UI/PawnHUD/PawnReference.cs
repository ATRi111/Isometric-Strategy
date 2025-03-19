using Services;
using Services.Event;
using System;
using UIExtend;
using UnityEngine;

[RequireComponent(typeof(CanvasGroupPlus))]
public class PawnReference : MonoBehaviour,  IPawnReference
{
    protected PawnEntity currentPawn;
    protected IEventSystem eventSystem;
    protected CanvasGroupPlus canvasGroup;
    public virtual PawnEntity CurrentPawn => currentPawn;

    public Action OnRefresh { get ; set; }

    protected virtual void SetPawn(PawnEntity pawnEntity)
    {
        currentPawn = pawnEntity;
        canvasGroup.Visible = pawnEntity != null;
        if (pawnEntity != null)
            OnRefresh?.Invoke();
    }

    protected virtual void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        canvasGroup = GetComponentInChildren<CanvasGroupPlus>();
    }

    protected virtual void OnEnable()
    {
        
    }

    protected virtual void OnDisable()
    {
        
    }
}
