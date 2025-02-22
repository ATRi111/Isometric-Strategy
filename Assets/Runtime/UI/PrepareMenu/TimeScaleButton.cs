using UIExtend;
using UnityEngine;

public class TimeScaleButton : ButtonBase
{
    private bool speedUp;

    protected override void OnClick()
    {
        if (speedUp)
        {
            Time.timeScale = 1f;
            speedUp = false;
            tmp.text = "1X";
        }
        else
        {
            Time.timeScale = 2f;
            speedUp = true;
            tmp.text = "2X";
        }
    }
}
