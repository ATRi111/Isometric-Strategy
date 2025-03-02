using UIExtend;
using UnityEngine;
using UnityEngine.EventSystems;

public class TaskTargetUI : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    private LevelManager levelManager;
    private CanvasGroupPlus canvasGroup;

    private void OnStartScout()
    {
        canvasGroup.Visible = true;
    }

    private void OnReturnToPrepareMenu()
    {
        canvasGroup.Visible = false;
    }

    private void Awake()
    {
        levelManager = LevelManager.FindInstance();
        canvasGroup = GetComponentInChildren<CanvasGroupPlus>();
    }

    private void OnEnable()
    {
        levelManager.OnStartScout += OnStartScout;
        levelManager.OnReturnToPrepareMenu += OnReturnToPrepareMenu;
    }

    private void OnDisable()
    {
        levelManager.OnStartScout -= OnStartScout;
        levelManager.OnReturnToPrepareMenu -= OnReturnToPrepareMenu;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        canvasGroup.Visible = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canvasGroup.Visible = true;
    }
}
