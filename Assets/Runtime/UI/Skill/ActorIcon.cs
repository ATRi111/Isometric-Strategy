using UnityEngine;

public class ActorIcon : MonoBehaviour
{
    private IsometricGridManager Igm => IsometricGridManager.Instance;

    private Canvas canvas;
    public int extraSortingOrder;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.overrideSorting = true;
    }

    private void OnEnable()
    {
        canvas.sortingOrder = Igm.CellToSortingOrder(transform.position) + extraSortingOrder;
    }
}
