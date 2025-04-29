using UnityEngine;

public class VFXSortingOrderController : MonoBehaviour
{
    private IsometricGridManager igm;

    [Range(-10, 30)]
    public int extraSortingOrder;

    public ParticleSystemRenderer[] front;
    public ParticleSystemRenderer[] mid;
    public ParticleSystemRenderer[] back;

    private void OnEnable()
    {
        igm = IsometricGridManager.Instance;
        foreach (ParticleSystemRenderer renderer in front)
        {
            renderer.sortingOrder = igm.CellToSortingOrder(transform.position) + extraSortingOrder + 1;
        }
        foreach (ParticleSystemRenderer renderer in mid)
        {
            renderer.sortingOrder = igm.CellToSortingOrder(transform.position) + extraSortingOrder;
        }
        foreach (ParticleSystemRenderer renderer in back)
        {
            renderer.sortingOrder = igm.CellToSortingOrder(transform.position) + extraSortingOrder - 1;
        }
    }
}