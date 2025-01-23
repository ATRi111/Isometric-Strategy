using MyTimer;
using UnityEngine;

public class ActionAreaIcon : IconUI
{
    private IsometricGridManager Igm => IsometricGridManager.Instance;

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

    protected override void Awake()
    {
        base.Awake();
        sineWave = new SineWave();
        sineWave.Initialize(minAlpha, maxAlpha, period, false);
        sineWave.OnTick += OnTick;
        canvas = GetComponent<Canvas>();
        canvas.overrideSorting = true;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        canvas.sortingOrder = Igm.CellToSortingOrder(transform.position) + extraSortingOrder;
        sineWave.Restart();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        sineWave.Paused = true;
    }
}
