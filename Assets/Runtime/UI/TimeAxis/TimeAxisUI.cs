using Services;
using Services.Event;
using System.Collections.Generic;
using UIExtend;
using UnityEngine;

public class TimeAxisUI : MonoBehaviour
{
    private GameManager gameManager;
    private IEventSystem eventSystem;

    private CanvasGroupPlus canvasGroup;
    private RectTransform rectTransform;
    private Comparer_PawnEntity_ActionTime comparer;
    private TimeAxisIcon[] icons;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private int maxIconCount;
    public float timeSpan;

    public Vector3 left;
    public Vector3 right;

    public Vector3 TimeToPosition(int time)
    {
        float p = 0.05f + 0.9f * (time / timeSpan);
        return Vector3.Lerp(left, right, p);
    }

    private void OnTick(int time)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        left = (corners[0] + corners[1]) / 2;
        right = (corners[2] + corners[3]) / 2;
        List<PawnEntity> entites = new();
        foreach (PawnEntity pawn in gameManager.pawns)
        {
            if (pawn.time - time < timeSpan)
                entites.Add(pawn);
            if (entites.Count == maxIconCount)
                break;
        }
        entites.Sort(comparer);
        for (int i = 0; i < entites.Count; i++)
        {
            icons[i].SetPawn(entites[i]);
            icons[i].transform.position = TimeToPosition(entites[i].time - time);
            icons[i].canvasGroup.Visible = true;
        }
        for (int i = entites.Count; i < maxIconCount; i++)
        {
            icons[i].canvasGroup.Visible = false;
        }
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        gameManager = ServiceLocator.Get<GameManager>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroupPlus>();
        comparer = new();
        icons = new TimeAxisIcon[maxIconCount];
        for (int i = 0; i < maxIconCount; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            icons[i] = obj.GetComponent<TimeAxisIcon>();
            icons[i].transform.localPosition = Vector3.zero;
        }
    }
    private void BeforeBattle()
    {
        canvasGroup.Visible = true;
    }

    private void OnEnable()
    {
        eventSystem.AddListener(EEvent.BeforeBattle, BeforeBattle);
        eventSystem.AddListener<int>(EEvent.OnTick, OnTick);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener(EEvent.BeforeBattle, BeforeBattle);
        eventSystem.RemoveListener<int>(EEvent.OnTick, OnTick);
    }
}
