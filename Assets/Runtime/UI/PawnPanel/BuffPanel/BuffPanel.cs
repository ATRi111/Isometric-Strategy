using System.Collections.Generic;
using UnityEngine;

public class BuffPanel : MonoBehaviour
{
    private IPawnReference pawnReference;
    private BuffIcon[] icons;
    private readonly List<Buff> buffs = new();

    public void Refresh()
    {
        PawnEntity pawn = pawnReference.CurrentPawn;
        pawn.BuffManager.GetAllEnabled(buffs);
        int i = 0;
        for (; i < buffs.Count && i < icons.Length; i++)
        {
            icons[i].SetBuff(buffs[i]);
            icons[i].canvasGroup.Visible = true;
        }
        for(; i < icons.Length;i++ )
        {
            icons[i].canvasGroup.Visible = false;
        }
    }

    private void Awake()
    {
        pawnReference = GetComponentInParent<IPawnReference>();
        pawnReference.OnRefresh += Refresh;
        icons = GetComponentsInChildren<BuffIcon>();
    }
}
