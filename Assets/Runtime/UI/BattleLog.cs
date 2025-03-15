using Services;
using Services.Event;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleLog : MonoBehaviour
{
    private IEventSystem eventSystem;
    [SerializeField]
    private TextMeshProUGUI tmp;
    private ScrollRect scrollRect;

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

    private void AfterBattle(bool _)
        => Clear();

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        scrollRect = GetComponentInChildren<ScrollRect>();
    }

    private void OnEnable()
    {
        eventSystem.AddListener<string>(EEvent.BattleLog, Log);
        eventSystem.AddListener<bool>(EEvent.AfterBattle, AfterBattle);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<bool>(EEvent.AfterBattle, AfterBattle);
    }
}
