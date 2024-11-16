using Services;
using Services.Event;
using TMPro;
using UnityEngine;

public class TextBase : MonoBehaviour
{
    protected IEventSystem eventSystem;
    public TextMeshProUGUI TextUI { get; protected set; }

    protected virtual void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        TextUI = GetComponentInChildren<TextMeshProUGUI>();
    }
}
