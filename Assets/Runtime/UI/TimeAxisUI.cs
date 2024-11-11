using Services;
using Services.Event;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeAxisUI : MonoBehaviour
{
    private IEventSystem eventSystem;
    private GameManager gameManager;

    private Comparer_PawnEntity_ActionTime comparer;
    private TimeAxisIcon[] icons;
    [SerializeField]
    private GameObject prefab;
    private Image image;

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
        for(int i = count;i < maxIconCount; i++)
        {
            icons[i].canvasGrounp.Visible = false;
        }
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        gameManager = ServiceLocator.Get<GameManager>();
        image = GetComponent<Image>();
        comparer = new();
        icons = new TimeAxisIcon[maxIconCount];
        for (int i = 0; i < maxIconCount; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            icons[i] = obj.GetComponent<TimeAxisIcon>();
            icons[i].canvasGrounp.Visible = false;
        }
        Vector3[] corners = new Vector3[4];
        GetComponent<RectTransform>().GetWorldCorners(corners); //TODO:适应不同分辨率
        left = (corners[0] + corners[1]) / 2;
        right = (corners[2] + corners[3]) / 2;
    }

    private void OnEnable()
    {
        gameManager.OnTick += OnTick;
    }

    private void OnDisable()
    {
        gameManager.OnTick -= OnTick;
    }
}
