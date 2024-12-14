using System.Text;
using UIExtend;
using UnityEngine;

[RequireComponent(typeof(CanvasGroupPlus))]
public class TimeAxisIcon : IconUI
{
    public PawnEntity entity;

    public void SetPawn(PawnEntity entity)
    {
        this.entity = entity;
        StringBuilder sb = new();
        sb.AppendLine(entity.EntityName);
        sb.Append(" £”‡ ±º‰:");
        sb.Append(entity.time - gameManager.Time);
        sb.AppendLine();
        message = sb.ToString();
    }
}
