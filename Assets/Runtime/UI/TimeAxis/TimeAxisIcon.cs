using System.Collections.Generic;
using System.Text;
using UIExtend;
using UnityEngine;

[RequireComponent(typeof(CanvasGroupPlus))]
public class TimeAxisIcon : IconUI
{
    public void SetPawns(List<PawnEntity> pawns)
    {
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
}
