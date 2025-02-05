using System.Collections.Generic;
using System.Text;
using UIExtend;
using UnityEngine;

[RequireComponent(typeof(CanvasGroupPlus))]
public class TimeAxisIcon : IconUI
{
    public void SetPawns(List<PawnEntity> pawns)
    {
        StringBuilder sb = new();
        for (int i = 0; i < pawns.Count - 1; i++)
        {
            sb.Append(pawns[i].EntityName);
            sb.Append(" ");
        }
        sb.Append(pawns[^1].EntityName);
        sb.AppendLine();
        sb.Append("Ê£ÓàÊ±¼ä:");
        sb.Append(pawns[0].time - gameManager.Time);
        sb.AppendLine();
        info = sb.ToString();
    }
}
