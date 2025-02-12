using MyTool;
using TMPro;
using UnityEngine;

public class PropertyValueChangeUI : MonoBehaviour
{
    private FindPawnPropertySO so;
    private PawnPanel pawnPanel;
    private TextMeshProUGUI tmp;

    public void Refresh()
    {
        StopPreview();
    }

    public void StopPreview()
    {
        tmp.text = string.Empty;
    }

    public void Preview()
    {
        so.pawn = pawnPanel.SelectedPawn;
        float deltaValue = pawnPanel.GetPropertyChange(so.name);

        if (deltaValue == 0)
            tmp.text = string.Empty;
        else
        {
            string color;
            if (deltaValue > 0)
                color = "red";
            else
                color = "#4EEE94";

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
        pawnPanel.RefreshAll += Refresh;
        pawnPanel.PreviewPropertyChange += Preview;
        pawnPanel.StopPreviewPropertyChange += StopPreview;
    }
}
