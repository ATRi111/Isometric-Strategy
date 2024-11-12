using Services;
using Services.Event;
using System.Collections.Generic;
using UnityEngine;

public class TimeAxisUI : MonoBehaviour
{
    private GameManager gameManager;
    private IEventSystem eventSystem;

    private Comparer_PawnEntity_ActionTime comparer;
    private TimeAxisIcon[] icons;
    [SerializeField]
    private GameObject prefab;

    public int maxIconCount;
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
        GetComponent<RectTransform>().GetWorldCorners(corners);
        left = (corners[0] + corners[1]) / 2;
        right = (corners[2] + corners[3]) / 2;
        List<PawnEntity> entites = new();
        timeSpan = 0;
        foreach (PawnEntity entity in gameManager.pawns)
        {
            entites.Add(entity);
            timeSpan = Mathf.Max(timeSpan, entity.time - time);
        }
        entites.Sort(comparer);
        timeSpan = Mathf.Max(10, timeSpan);
        int count = Mathf.Min(entites.Count, maxIconCount);
        for (int i = 0; i < count; i++)
        {
            icons[i].SetPawn(entites[i]);
            icons[i].transform.position = TimeToPosition(entites[i].time - time);
            icons[i].canvasGrounp.Visible = true;
        }
        for (int i = count;i < maxIconCount; i++)
        {
            icons[i].canvasGrounp.Visible = false;
        }
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        gameManager = ServiceLocator.Get<GameManager>();
        comparer = new();
        icons = new TimeAxisIcon[maxIconCount];
        for (int i = 0; i < maxIconCount; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            icons[i] = obj.GetComponent<TimeAxisIcon>();
            icons[i].transform.localPosition = Vector3.zero;
        }
    }

    private void OnEnable()
    {
        eventSystem.AddListener<int>(EEvent.OnTick, OnTick);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<int>(EEvent.OnTick, OnTick);
    }
}
