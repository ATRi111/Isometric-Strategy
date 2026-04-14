using Services.ObjectPools;
using UnityEngine;

public class GroundShadowCaster : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public bool visible;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (!visible)
        {
            spriteRenderer.enabled = false;
            enabled = false;
            return;
        }

        Vector3Int cellPosition = IsometricGridManager.Instance.WorldToCell(transform.position);
        Vector4 cell = new(cellPosition.x, cellPosition.y, cellPosition.z, 1);
        spriteRenderer.material.SetVector("_CellPosition", cell);
    }

    private void OnDestroy()
    {
        ObjectPoolUtility.RecycleMyObjects(gameObject);
    }
}
