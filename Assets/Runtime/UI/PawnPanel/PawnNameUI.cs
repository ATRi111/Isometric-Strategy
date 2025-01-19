using TMPro;
using UnityEngine;

public class PawnNameUI : MonoBehaviour
{
    private PawnPanel pawnPanel;
    private TextMeshProUGUI tmp;

    private void Refresh(PawnEntity pawnEntity)
    {
        tmp.text = pawnEntity.gameObject.name;
    }

    private void Awake()
    {
        pawnPanel = GetComponentInParent<PawnPanel>();
        tmp = GetComponent<TextMeshProUGUI>();
        pawnPanel.RefreshAll += Refresh;
    }
}
