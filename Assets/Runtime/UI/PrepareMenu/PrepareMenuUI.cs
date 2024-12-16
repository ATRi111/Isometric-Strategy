using UIExtend;
using UnityEngine;

[RequireComponent(typeof(CanvasGroupPlus))]
public class PrepareMenuUI : MonoBehaviour
{
    protected LevelManager levelManager;
    protected CanvasGroupPlus canvasGroup;

    protected virtual void OnStartScout()
    {

    }

    protected virtual void OnReturnToPrepareMenu()
    {
        canvasGroup.Visible = true;
    }

    protected virtual void OnStartBattle()
    {
        canvasGroup.Visible = false;
    }

    protected virtual void Awake()
    {
        levelManager = GetComponentInParent<LevelManager>();
        canvasGroup = GetComponent<CanvasGroupPlus>();
    }

    protected virtual void OnEnable()
    {
        levelManager.OnStartScout += OnStartScout;
        levelManager.OnStartBattle += OnStartBattle;
        levelManager.OnReturnToPrepareMenu += OnReturnToPrepareMenu;
    }

    protected virtual void OnDisable()
    {
        levelManager.OnStartScout -= OnStartScout;
        levelManager.OnStartBattle -= OnStartBattle;
        levelManager.OnReturnToPrepareMenu -= OnReturnToPrepareMenu;
    }
}
