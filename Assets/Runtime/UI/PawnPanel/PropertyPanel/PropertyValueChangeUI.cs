using TMPro;
using UnityEngine;

public class PropertyValueChangeUI : MonoBehaviour
{
    private FindPawnPropertySO so;
    private PawnPanel pawnPanel;
    private TextMeshProUGUI tmp;

    public void Preview(PawnEntity pawn)
    {
        so.pawn = pawn;
        float deltaValue = pawnPanel.GetPropertyChange(pawn.name);

        if (deltaValue == 0)
            tmp.text = string.Empty;
        else
        {
            if (deltaValue > 0)
                tmp.color = Color.green;
            else
                tmp.color = Color.red;

            if (Mathf.Abs(deltaValue) >= 1)
                tmp.text = Mathf.RoundToInt(deltaValue).ToString("+0.##;-0.##");
            else
                tmp.text = deltaValue.ToString("+0%;-0%");
        }
    }

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        string propertyName = transform.parent.GetComponent<TextMeshProUGUI>().text;
        so = PawnPropertyUtility.GetProperty(propertyName);
        pawnPanel = GetComponentInParent<PawnPanel>();
        pawnPanel.RefreshAll += Preview;
        pawnPanel.PreviewPropertyChange += Preview;
    }
}
