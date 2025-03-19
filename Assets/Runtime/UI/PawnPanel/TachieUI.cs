using UnityEngine;
using UnityEngine.UI;

public class TachieUI : MonoBehaviour
{
    private IPawnReference pawnRefernce;
    private Image image;

    private void Refresh()
    {
        image.sprite = pawnRefernce.CurrentPawn.tachie;
    }

    private void Awake()
    {
        image = GetComponentInChildren<Image>();
        pawnRefernce = GetComponentInParent<IPawnReference>();
        pawnRefernce.OnRefresh += Refresh;
    }
}
