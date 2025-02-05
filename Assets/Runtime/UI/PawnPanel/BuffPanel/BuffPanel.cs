using System.Collections.Generic;
using UnityEngine;

public class BuffPanel : MonoBehaviour
{
    private PawnPanel pawnPanel;
    private BuffIcon[] icons;
    private List<Buff> buffs = new();

    public void Refresh(PawnEntity pawn)
    {
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
        pawnPanel = GetComponentInParent<PawnPanel>();
        pawnPanel.RefreshAll += Refresh;
        icons = GetComponentsInChildren<BuffIcon>();
    }
}
