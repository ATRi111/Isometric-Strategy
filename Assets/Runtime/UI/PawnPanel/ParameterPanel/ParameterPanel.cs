using System.Collections.Generic;
using UnityEngine;

public class ParameterPanel : MonoBehaviour
{
    private PawnPanel pawnPanel;
    private ParameterIcon[] icons;

    public void Refresh()
    {
        PawnEntity pawn = pawnPanel.SelectedPawn;
        List<string> parameterNames = pawn.GetVisibleParameters();
        int i = 0;
        for (; i < parameterNames.Count && i < icons.Length; i++)
        {
            icons[i].SetParameter(pawn, parameterNames[i]);
            icons[i].canvasGroup.Visible = true;
        }
        for (; i < icons.Length; i++)
        {
            icons[i].canvasGroup.Visible = false;
        }
    }

    private void Awake()
    {
        pawnPanel = GetComponentInParent<PawnPanel>();
        pawnPanel.RefreshAll += Refresh;
        icons = GetComponentsInChildren<ParameterIcon>();
    }
}
