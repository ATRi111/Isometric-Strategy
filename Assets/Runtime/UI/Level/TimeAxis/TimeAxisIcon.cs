using MyTool;
using Services;
using Services.ObjectPools;
using System.Text;
using UIExtend;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroupPlus))]
public class TimeAxisIcon : InfoIcon
{
    private IsometricGridManager Igm => IsometricGridManager.Instance;
    private IObjectManager objectManager;
    private PawnEntity pawn;
    private GameObject mountPoint;

    public void SetPawn(PawnEntity pawn)
    {
        this.pawn = pawn;
        image.sprite = pawn.icon;
        StringBuilder sb = new();
        sb.AppendLine(pawn.EntityNameWithColor);
        sb.Append("剩余等待时间:");
        sb.Append(pawn.time - gameManager.Time);
        sb.AppendLine();
        info = sb.ToString();
    }

    protected override void Awake()
    {
        base.Awake();
        objectManager = ServiceLocator.Get<IObjectManager>();
        mountPoint = new GameObject("TimeAxisMountPoint");
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        IMyObject obj = objectManager.Activate("ActorIcon", Igm.CellToWorld(pawn.GridObject.CellPosition), Vector3.zero, mountPoint.transform);
        obj.Transform.SetLossyScale(Vector3.one);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        ObjectPoolUtility.RecycleMyObjects(mountPoint);
    }
}
