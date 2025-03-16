using MyTimer;
using UIExtend;
using UnityEngine;

public class AreaIcon : MonoBehaviour
{
    private IsometricGridManager Igm => IsometricGridManager.Instance;
    private CanvasGroupPlus canvasGroup;

    private Canvas canvas;
    private SineWave sineWave;
    [SerializeField]
    private float minAlpha;
    [SerializeField]
    private float maxAlpha;
    [SerializeField]
    private float duration;
    [SerializeField]
    private int extraSortingOrder;
    [SerializeField]
    private float loopDistance;

    private void OnTick(float value)
    {
        canvasGroup.Alpha = value;
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroupPlus>();
        sineWave = new SineWave();
        sineWave.Initialize(minAlpha, maxAlpha, duration, false);
        sineWave.OnTick += OnTick;
        canvas = GetComponent<Canvas>();
        canvas.overrideSorting = true;
    }

    private void OnEnable()
    {
        canvas.sortingOrder = Igm.CellToSortingOrder(transform.position) + extraSortingOrder;
        Vector3Int cell = Igm.WorldToCell(transform.position);
        sineWave.Restart();
        float normalizedTime = (cell.y % loopDistance) / loopDistance;
        sineWave.Time = normalizedTime * duration;
    }

    private void OnDisable()
    {
        sineWave.Paused = true;
    }
}
