using MyTool;
using UnityEngine;

public class FaceShadow : MonoBehaviour
{
    [Range(-10, 30)]
    public int extraSortingOrder;

    public void Init(ShadowManager shadowManager, ShadowVertex vertex)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        float radiance = shadowManager.GetRadiance(vertex);
        spriteRenderer.color = spriteRenderer.color.SetAlpha(1f - radiance);
        spriteRenderer.sortingOrder = IsometricGridManager.Instance.CellToSortingOrder(transform.position) + extraSortingOrder;
    }
}
