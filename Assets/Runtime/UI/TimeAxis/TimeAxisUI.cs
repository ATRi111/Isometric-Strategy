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

    [SerializeField]
    private float leftBlankSpace;
    [SerializeField]
    private float rightBlankSpace;

    public Vector3 TimeToPosition(int time)
    {
        float p = leftBlankSpace + (1f - leftBlankSpace - rightBlankSpace) * (time / timeSpan);
        p = Mathf.Clamp01(p);
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
            entites.Add(pawn);
        }
        entites.Sort(comparer);
        int current = entites[0].time;
        List<PawnEntity> temp = new();
        int iconIndex = 0;
        for (int i = 0; i < entites.Count; i++)
        {
            if (entites[i].time == current)
            {
                temp.Add(entites[i]);
            }
            else
            {
                icons[iconIndex].SetPawns(temp);    //具有相同WT的单位一并显示
                icons[iconIndex].transform.position = TimeToPosition(temp[0].time - time);
                icons[iconIndex].canvasGroup.Visible = true;
                iconIndex++;
                if (iconIndex == maxIconCount)
                    break;
                temp.Clear();
                current = entites[i].time;
                temp.Add(entites[i]);
            }
        }
        if (temp.Count > 0 && iconIndex < maxIconCount)
        {
            icons[iconIndex].SetPawns(temp);    //具有相同WT的单位一并显示
            icons[iconIndex].transform.position = TimeToPosition(temp[0].time - time);
            icons[iconIndex].canvasGroup.Visible = true;
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
