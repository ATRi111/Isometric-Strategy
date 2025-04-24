using UIExtend;
using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    private CanvasGroupPlus canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroupPlus>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            canvasGroup.Visible = !canvasGroup.Visible;
        }
    }
}
