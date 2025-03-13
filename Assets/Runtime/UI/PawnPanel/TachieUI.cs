using UnityEngine;
using UnityEngine.UI;

public class TachieUI : MonoBehaviour
{
    private PawnPanel pawnPanel;
    private Image image;

    private void Refresh()
    {
        image.sprite = pawnPanel.SelectedPawn.tachie;
    }

    private void Awake()
    {
        image = GetComponentInChildren<Image>();
        pawnPanel = GetComponentInParent<PawnPanel>();
        pawnPanel.RefreshAll += Refresh;
    }
}
