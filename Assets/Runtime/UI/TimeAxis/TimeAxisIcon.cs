using MyTool;
using System.Collections.Generic;
using System.Text;
using UIExtend;
using UnityEngine;

[RequireComponent(typeof(CanvasGroupPlus))]
public class TimeAxisIcon : IconUI
{
    public void SetPawns(List<PawnEntity> pawns)
    {
        //TODO:—’…´
        StringBuilder sb = new();
        for (int i = 0; i < pawns.Count - 1; i++)
        {
            string colorString = pawns[i].faction switch
            {
                EFaction.Ally => FontUtility.SpringGreen3,
                EFaction.Enemy => FontUtility.Red,
                _ => FontUtility.Black
            };
            sb.Append(pawns[i].EntityName.ColorText(colorString));
            sb.Append(" ");
        }
        sb.Append(pawns[^1].EntityName);
        sb.AppendLine();
        sb.Append(" £”‡ ±º‰:");
        sb.Append(pawns[0].time - gameManager.Time);
        sb.AppendLine();
        info = sb.ToString();
    }
}
