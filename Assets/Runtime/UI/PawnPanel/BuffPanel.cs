using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPanel : MonoBehaviour
{
    private PawnPanel pawnPanel;
    private BuffIcon[] icons;

    public void Refresh(PawnEntity pawn)
    {
        List<Buff> buffs = pawn.BuffManager.GetAll();
        int i = 0;
        for (; i < buffs.Count && i < icons.Length; i++)
        {
            icons[i].SetBuff(buffs[i]);
            icons[i].canvasGroup.Visible = true;
        }
        for
    }

    private void Awake()
    {
        pawnPanel = GetComponentInParent<PawnPanel>();
        pawnPanel.RefreshAll += Refresh;
        icons = GetComponentsInChildren<BuffIcon>();
    }
}
