using MyTool;
using TMPro;
using UnityEngine;

public class PropertyValueChangeUI : MonoBehaviour
{
    private FindPawnPropertySO so;
    private PawnPanel pawnPanel;
    private TextMeshProUGUI tmp;

    public void StopPreview()
    {
        tmp.text = string.Empty;
    }

    public void Preview()
    {
        so.pawn = pawnPanel.CurrentPawn;
        float deltaValue = pawnPanel.GetPropertyChange(so.name);

        if (deltaValue == 0)
            tmp.text = string.Empty;
        else
        {
            string color;
            float k = so.negative ? -1f : 1f;
            if (k * deltaValue > 0)
                color = FontUtility.SpringGreen3;
            else
                color = FontUtility.Red;

            if (Mathf.Abs(deltaValue) >= 1)
                tmp.text = Mathf.RoundToInt(deltaValue).ToString("+0.##;-0.##").ColorText(color);
            else
                tmp.text = deltaValue.ToString("+0%;-0%").ColorText(color);
        }
    }

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        string propertyName = transform.parent.GetComponent<TextMeshProUGUI>().text;
        so = PawnPropertyUtility.GetProperty(propertyName);
        pawnPanel = GetComponentInParent<PawnPanel>();
        pawnPanel.OnRefresh += StopPreview;
        pawnPanel.PreviewPropertyChange += Preview;
    }
}
