using MyTool;
using Services;
using Services.ObjectPools;
using System.Collections.Generic;
using System.Text;
using UIExtend;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroupPlus))]
public class TimeAxisIcon : InfoIcon
{
    private IsometricGridManager Igm => IsometricGridManager.Instance;
    private IObjectManager objectManager;
    private readonly List<PawnEntity> pawns = new();
    private GameObject mountPoint;

    public void SetPawns(List<PawnEntity> pawns)
    {
        this.pawns.Clear();
        this.pawns.AddRange(pawns);
        Vector3 sum = Vector3.zero;
        for (int i = 0; i < pawns.Count; i++)
        {
            Color color = pawns[i].faction switch
            {
                EFaction.Ally => Color.green,
                EFaction.Enemy => Color.red,
                _ => Color.black
            };
            sum += new Vector3(color.r, color.g, color.b);
        }
        sum /= pawns.Count;
        image.color = new Color(sum.x, sum.y, sum.z, 1f);

        StringBuilder sb = new();
        for (int i = 0; i < pawns.Count; i++)
        {
            sb.Append(pawns[i].EntityNameWithColor);
            sb.Append(" ");
        }
        sb.AppendLine();
        sb.Append("Ê£ÓàµÈ´ýÊ±¼ä:");
        sb.Append(pawns[0].time - gameManager.Time);
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
        for (int i = 0; i < pawns.Count; i++)
        {
            IMyObject obj = objectManager.Activate("ActorIcon", Igm.CellToWorld(pawns[i].GridObject.CellPosition), Vector3.zero, mountPoint.transform);
            obj.Transform.SetLossyScale(Vector3.one);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        ObjectPoolUtility.RecycleMyObjects(mountPoint);
    }
}
