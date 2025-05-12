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
    private TimeAxisIcon[] icons;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private int maxIconCount;

    public Vector3 left;
    public Vector3 right;

    [SerializeField]
    private float leftBlankSpace;
    [SerializeField]
    private float rightBlankSpace;
    [SerializeField]
    private float minPercent;

    public Vector3 PercentToPosition(float percent)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        left = (corners[0] + corners[1]) / 2;
        right = (corners[2] + corners[3]) / 2;
        float p = leftBlankSpace + (1f - leftBlankSpace - rightBlankSpace) * percent;
        p = Mathf.Clamp01(p);
        return Vector3.Lerp(left, right, p);
    }

    private void OnTick(int time)
    {
        List<int> timeList = new();
        int count = 0;
        for (; count < gameManager.sortedPawns.Count && count < maxIconCount; count++)
        {
            PawnEntity pawn = gameManager.sortedPawns[count];
            timeList.Add(pawn.time - gameManager.Time);
        }
        float adjustedMaxTime = Mathf.Max(timeList[^1] * (1f + minPercent * count), 10f);
        for (int i = 0; i < count; i++)
        {
            icons[i].transform.position = PercentToPosition(timeList[i] / adjustedMaxTime + i * minPercent);
            icons[i].canvasGroup.Visible = true;
            icons[i].SetPawn(gameManager.sortedPawns[i]);
        }

        for (int i = count; i < maxIconCount; i++)
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
