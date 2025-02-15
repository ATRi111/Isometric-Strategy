using MyTimer;
using UIExtend;
using UnityEngine;

public class ActionArea : MonoBehaviour
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
    private float period;
    [SerializeField]
    private int extraSortingOrder;

    private void OnTick(float value)
    {
        canvasGroup.Alpha = value;
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroupPlus>();
        sineWave = new SineWave();
        sineWave.Initialize(minAlpha, maxAlpha, period, false);
        sineWave.OnTick += OnTick;
        canvas = GetComponent<Canvas>();
        canvas.overrideSorting = true;
    }

    private void OnEnable()
    {
        canvas.sortingOrder = Igm.CellToSortingOrder(transform.position) + extraSortingOrder;
        sineWave.Restart();
    }

    private void OnDisable()
    {
        sineWave.Paused = true;
    }
}
