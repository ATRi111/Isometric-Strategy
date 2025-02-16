using UIExtend;
using UnityEngine;

[RequireComponent(typeof(CanvasGroupPlus))]
public class LevelDescription : MonoBehaviour
{
    private LevelManager levelManager;
    private CanvasGroupPlus canvasGroup;

    private void Show()
    {
        canvasGroup.Visible = true;
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroupPlus>();
        levelManager = LevelManager.FindInstance();
    }

    private void OnEnable()
    {
        levelManager.ShowLevelDescription += Show;
    }

    private void OnDisable()
    {
        levelManager.ShowLevelDescription -= Show;
    }
}
