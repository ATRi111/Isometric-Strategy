using Services;
using Services.Event;
using System.Collections;
using TMPro;
using UIExtend;
using UnityEngine;
using UnityEngine.UI;

public class BattleLog : MonoBehaviour
{
    private IEventSystem eventSystem;
    [SerializeField]
    private TextMeshProUGUI tmp;
    private ScrollRect scrollRect;
    private CanvasGroupPlus canvasGroup;

    public void Log(string message)
    {
        tmp.text += message + "\n";
        StartCoroutine(Drag());
    }
    public void Clear()
    {
        tmp.text = string.Empty;
        StartCoroutine(Drag());
    }

    private IEnumerator Drag()
    {
        scrollRect.normalizedPosition = Vector2.zero;
        yield return new WaitForEndOfFrame();
        scrollRect.normalizedPosition = Vector2.zero;
    }

    private void BeforeBattle()
    {
        canvasGroup.Visible = true;
    }

    private void AfterBattle(bool _)
    {
        canvasGroup.Visible = false;
        Clear();
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        scrollRect = GetComponentInChildren<ScrollRect>();
        canvasGroup = GetComponent<CanvasGroupPlus>();
    }

    private void OnEnable()
    {
        eventSystem.AddListener<string>(EEvent.BattleLog, Log);
        eventSystem.AddListener<bool>(EEvent.AfterBattle, AfterBattle);
        eventSystem.AddListener(EEvent.BeforeBattle, BeforeBattle);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<string>(EEvent.BattleLog, Log);
        eventSystem.RemoveListener<bool>(EEvent.AfterBattle, AfterBattle);
        eventSystem.RemoveListener(EEvent.BeforeBattle, BeforeBattle);
    }
}
