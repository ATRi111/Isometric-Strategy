using System.Collections.Generic;
using UnityEngine;

public class ParameterPanel : MonoBehaviour
{
    private IPawnReference pawnReference;
    private ParameterIcon[] icons;

    public void Refresh()
    {
        PawnEntity pawn = pawnReference.CurrentPawn;
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
        pawnReference = GetComponentInParent<IPawnReference>();
        pawnReference.OnRefresh += Refresh;
        icons = GetComponentsInChildren<ParameterIcon>();
    }
}
